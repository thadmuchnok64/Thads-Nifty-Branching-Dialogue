using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateCharacterDialogue : DialogueBranch
{
    public int indexOfEntity;

    public override int GetToggledEntity()
    {
        return indexOfEntity;
    }
}
