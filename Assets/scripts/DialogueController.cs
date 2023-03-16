using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueController : MonoBehaviour {
    [System.Serializable]
    public class Background
    {
        [HideInInspector] public string name;
        public Sprite sprite, groundSprite;
    }

    [System.Serializable]
    public class Chapter {
        public enum Character { Eil, Maxwell };
        public enum Outcome { Wins, Loses};

        [System.Serializable]
        public class Line
        {
            [HideInInspector] public string name;
            
            public Character speaker;
            [TextArea(3, 7)]
            public string line;
        }

        [HideInInspector] public string name;
        public string displayName;
        public int ID;
        public Outcome playIfEli;
        public List<Line> afterFight;
        public List<Line> beforeNextFight;
        public int nextChapterID;

        public void OnValidate()
        {
            if (!string.IsNullOrEmpty(displayName)) name = ID +": " + displayName;
            else name = ID + ": if Eli " + playIfEli.ToString() + " -> " + nextChapterID;
            
            foreach (var line in afterFight) {
                line.name = line.speaker + ": " + line.line;
            }
            foreach (var line in beforeNextFight) {
                line.name = line.speaker + ": " + line.line;
            }
        }
    }


    [SerializeField] List<Chapter> chapters = new List<Chapter>();
    [SerializeField] List<Background> backgrounds = new List<Background>();

    [SerializeField] SpriteRenderer background;
    [SerializeField] List<SpriteRenderer> floors = new List<SpriteRenderer>(); 
    [SerializeField] float waitTime, textSpeed = 0.01f;
    public Chapter currChap;
    List<Chapter.Line> currentLines = new List<Chapter.Line>();
    [SerializeField] TextMeshProUGUI eliText, MaxwellText;
    [SerializeField] CanvasGroup eliGroup, maxwellGroup;
    [SerializeField] Image eliSpeaker, maxwellSpeaker;
    [SerializeField] List<Sprite> maxwells, elis;
    public GameObject blackWipe;
    public Vector2 wipeLeftRightPos;

    [Header("Sounds")]
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip eliSpeak, maxSpeak;
    [SerializeField] float pitchRange, textVol;
     
    bool eliSpeaking, eliSpokeLast, switchingToNextFight;
    int spriteIndex = 0, chapID = -1;
    string lineRemaining = "";

    private void OnValidate()
    {
        foreach (var chap in chapters) {
            chap.OnValidate();
        }
        for (int i = 0; i < backgrounds.Count; i++) {
            backgrounds[i].name = i+ ": " + (backgrounds[i].sprite == null ? "" : backgrounds[i].sprite.name);
        }
    }

    void Init() {
        currChap = chapters[0];
        currentLines = currChap.afterFight;
        chapID = -1;
    }

    public void StartDialogue() {
        if (currChap == null || chapID == -1) Init();
        gameObject.SetActive(true);
        DisplayNext();
    }

    public bool StartDialogue(Chapter.Outcome eliStatus) {
        Chapter newChap = null;

        for (int i = 0; i < chapters.Count; i++) {
            if (chapters[i].ID == chapID && chapters[i].playIfEli == eliStatus) newChap = chapters[i];
        }
        if (newChap == null) {
            GameManager.instance.StartFight();
            return false;
        }
        maxwellSpeaker.sprite = (chapID == 1 || chapID == 2) ? maxwells[1] : chapID == 0 ? maxwells[0] : maxwells[2];
        eliSpeaker.sprite = (chapID == 1 || chapID == 2) ? elis[1] : chapID == 0 ? elis[0] : elis[2];

        currChap = newChap;
        currentLines = currChap.afterFight;
        StartDialogue();
        return true;
    }
    

    bool DisplayNext() {
        if (switchingToNextFight) return true;

        if (currentLines.Count == 0) {
            MaxwellText.text = eliText.text = "";
            if (currChap.beforeNextFight.Count == 0) return false;
            StartCoroutine(SwitchToNextFIght());
            return true;
        }

        if (lineRemaining.Length > 0) {
            StopAllCoroutines();
            if (eliSpeaking) eliText.text += lineRemaining;
            else MaxwellText.text += lineRemaining;
            currentLines.RemoveAt(0);
            lineRemaining = "";

            return true;
        }

        maxwellSpeaker.sprite = maxwells[spriteIndex];
        eliSpeaker.sprite = elis[spriteIndex];

        var line = currentLines[0];
        eliSpeaking = (line.speaker == Chapter.Character.Eil);

        eliGroup.alpha = eliSpeaking ? 1 : 0.4f;
        maxwellGroup.alpha = eliSpeaking ? 0.4f : 1;
        eliSpokeLast = currentLines[0].speaker == Chapter.Character.Eil;

        lineRemaining = line.line;
        if (eliSpeaking) eliText.text = "";
        else MaxwellText.text = "";
        StartCoroutine(AnimateText());

        return true;
    }

    IEnumerator AnimateText() {
        while (lineRemaining.Length > 0) {
            if (eliSpeaking) eliText.text += lineRemaining[0];
            else MaxwellText.text += lineRemaining[0];
            lineRemaining = lineRemaining.Remove(0, 1);
            
            yield return new WaitForSeconds(textSpeed); 
            PlayTextSound();
        }
        
        if (currentLines.Count > 0) currentLines.RemoveAt(0);
        lineRemaining = "";
    }

    void PlayTextSound() {
        source.clip = eliSpeaking ? eliSpeak : maxSpeak;
        source.pitch = 1 + Random.Range(-pitchRange, pitchRange);
        source.Play();
    }

    IEnumerator SwitchToNextFIght()
    {
        switchingToNextFight = true;

        blackWipe.transform.localPosition = Vector3.right * wipeLeftRightPos.y;
        while (blackWipe.transform.localPosition.x > 50) {
            var Pos = blackWipe.transform.localPosition;
            Pos.x = 0;
            GameManager.instance.musicSource.volume = Mathf.Lerp(GameManager.instance.musicSource.volume, 0, 0.025f);
            yield return new WaitForEndOfFrame();
            blackWipe.transform.localPosition = Vector3.Lerp(blackWipe.transform.localPosition, Pos, 0.025f);
        }

        GameManager.instance.musicSource.Stop();
        StartCoroutine(GameManager.instance.StartMusic());
        GameManager.instance.SetUpPlayerSprites();

        background.sprite = backgrounds[currChap.nextChapterID].sprite;
        foreach(var floor in floors) floor.sprite = backgrounds[currChap.nextChapterID].groundSprite;
        spriteIndex += 1;
        maxwellSpeaker.sprite = maxwells[spriteIndex];
        eliSpeaker.sprite = elis[spriteIndex];
        currentLines = currChap.beforeNextFight;
        MaxwellText.text = eliText.text = "";
        DisplayNext();

        while (System.MathF.Abs(blackWipe.transform.localPosition.x - wipeLeftRightPos.x) > 150) {
            var Pos = blackWipe.transform.localPosition;
            Pos.x = wipeLeftRightPos.x;
            yield return new WaitForEndOfFrame();
            blackWipe.transform.localPosition = Vector3.Lerp(blackWipe.transform.localPosition, Pos, 0.025f);
        }
        blackWipe.transform.localPosition = Vector3.right * wipeLeftRightPos.x;

        switchingToNextFight = false;
    }

    public void NextOrSkip (bool eli) {
        if (currentLines.Count == 0) NextOrSkip();
        else if (eliSpokeLast == eli) NextOrSkip();
    }

    public void NextOrSkip() {
        if (DisplayNext()) return;

        chapID = currChap.nextChapterID;
        GameManager.instance.StartFight();
    }

}
