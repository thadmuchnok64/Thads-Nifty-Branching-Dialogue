using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class DialogueBranch : CoreNode {

    [Input] public int entry;
    [Output] public int choice1,choice2,choice3,choice4;
    [TextArea(2, 12)]
    public string answer="[Continue]";
    [TextArea(5, 12)]
    public string response;
  
    //public string emotion="Neutral";

    //public string GetEmotion() { return emotion; }



}