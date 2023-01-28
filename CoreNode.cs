using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class CoreNode : Node {
    /*
	public virtual string GetString()
    {
        return "haha node go brr";
    }
    public virtual Sprite GetSprite()
    {
        return null;
    }
    */
    public virtual bool IsStartPoint()
    {
        return false;
    }
    public virtual bool IsEndPoint()
    {
        return false;
    }

    //returns -1 by default. Override this in questbranch.
    public virtual int GetGivenQuestID()
    {
        return -1;
    }

    public virtual int GetFinishedQuestID()
    {
        return -1;
    }

    public virtual int GetToggledEntity()
    {
        return -1;
    }

    public virtual bool CheckIfActivatesShop(string name)
    {
        return false;
    }

}