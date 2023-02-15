using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        public int ID;
        public Outcome playIfEli;
        public List<Line> afterFight;
        public List<Line> beforeNextFight;
        public int nextChapterID;

        public void OnValidate()
        {
            name = ID + ": if Eli " + playIfEli.ToString() + " -> " + nextChapterID;
            
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
    [SerializeField] Chapter currChap;

    private void OnValidate()
    {
        foreach (var chap in chapters) {
            chap.OnValidate();
        }
    }


    private void OnEnable()
    {
        
    }

    public void NextOrSkip()
    {

    }

}
