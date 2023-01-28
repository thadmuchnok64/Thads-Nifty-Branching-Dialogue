using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [TextArea(2,12)]
    public string response;
    public bool isHub;
    public Choice[] DialogueChoices;
    public AudioClip[] characterVoice;

    // A dialogue hub is a place where dialogue branches can connect to older dialogue branches.


    [System.Serializable]
    public struct Choice
    {
        [TextArea(2, 5)] public string Answer;
        public Dialogue dialogue;
        public int returnsToPreviousHubs;
    }

 
}
