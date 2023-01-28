using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBranch : DialogueBranch
{
    public int questID = -1;
    public int finishedQuestId = -1;

    public override int GetGivenQuestID()
    {

        return questID;
    }

    public override int GetFinishedQuestID()
    {
        return finishedQuestId;
    }

}
