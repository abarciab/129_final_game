using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DialogueController : MonoBehaviour {
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
            if (!string.IsNullOrEmpty(displayName)) name = displayName;
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
    [SerializeField] float waitTime, textSpeed = 0.01f;
    public Chapter currChap;
    List<Chapter.Line> currentLines = new List<Chapter.Line>();
    bool eliSpeaking;
    [SerializeField] TextMeshProUGUI eliText, MaxwellText;
    [SerializeField] CanvasGroup eliGroup, maxwellGroup;
    [SerializeField] Image eliSpeaker, maxwellSpeaker;

    [SerializeField] List<Sprite> maxwells, elis;
    int spriteIndex = 0;

    public int chapID = -1;
    
    private void OnValidate()
    {
        foreach (var chap in chapters) {
            chap.OnValidate();
        }
    }

    void Init() {
        currChap = chapters[0];
        currentLines = currChap.afterFight;
        chapID = -1;
    }

    private void Update() {
        if (Input.anyKeyDown) NextOrSkip();
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
    bool startedCoroutine;

    bool DisplayNext() {
        if (startedCoroutine) return true;

        if (currentLines.Count == 0) {
            if (currChap.beforeNextFight.Count == 0) return false;
            StartCoroutine(SwitchToNextFIght());
            return true;
        }

        maxwellSpeaker.sprite = maxwells[spriteIndex];
        eliSpeaker.sprite = elis[spriteIndex];

        var line = currentLines[0];
        eliSpeaking = (line.speaker == Chapter.Character.Eil);

        eliGroup.alpha = eliSpeaking ? 1 : 0.4f;
        maxwellGroup.alpha = eliSpeaking ? 0.4f : 1;
        if (eliSpeaking) eliText.text = line.line;
        else MaxwellText.text = line.line;
        currentLines.RemoveAt(0);
        return true;
    }

    IEnumerator SwitchToNextFIght()
    {
        startedCoroutine = true;
        yield return new WaitForSeconds(2);

        GameManager.instance.background.sprite = GameManager.instance.backgroundSprites[1];
        spriteIndex += 1;
        maxwellSpeaker.sprite = maxwells[spriteIndex];
        eliSpeaker.sprite = elis[spriteIndex];

        currentLines = currChap.beforeNextFight;
        startedCoroutine = false;
    }

    public void NextOrSkip() {
        if (DisplayNext()) return;

        chapID = currChap.nextChapterID;
        GameManager.instance.StartFight();
    }

}
