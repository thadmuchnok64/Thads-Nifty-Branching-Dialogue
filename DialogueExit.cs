using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class DialogueExit : DialogueBranch {

    public override bool IsEndPoint()
    {
        return true;
    }
}