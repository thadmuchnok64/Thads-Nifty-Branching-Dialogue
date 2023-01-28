using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : InteractableObject
{
    //public string name;
    public DialogueGraph dialogueGraph;
    public List<Transform> extraTransform;

    private void Awake()
    {
        extraTransform.Insert(0, transform);
    }

    public override void DoAction()
    {
        DialogueManager.instance.StartDialogue(dialogueGraph, extraTransform.ToArray());
        if (GetComponent<Entity>() != null)
        {
            GetComponent<Entity>().currentState = Entity.EntityStates.TALKING;
            Player.instance.currentState = Entity.EntityStates.TALKING;
        }
        base.DoAction();
    }
}
