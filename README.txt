Have fun with my lovely dialogue system!

The objective of this dialogue system is to create a branching dialogue system
similar to cRPG type games that allow for more flexibility and freedom in dialogue.
This dialogue system would be able to emulate games like Fallout New Vegas or Disco Elysium.

xnode junk stores all the xnode stuff you need to use nodes.

You will absolutely need:
CoreNode.cs
Dialogue.cs
DialogueBranch.cs
DialogueExit.cs
DialogueGraph.cs
DialogueManager.cs

Dialogue.cs is a node that is used to start dialogue

DialogueBranches.cs will be used to give the player options with dialogue, and
are probably the most common node you'll be working with.

DialogueExit.cs is a node that exits dialogue. Use them at the end of dialogue trees.

DialogueGraph.cs holds the dialogue tree.

DialogueManager.cs does all the heavy lifting with making the dialogue nodes work
with the actual game and the UI.

Other classes like QuestBranch.cs or AlternateCharacterDialogue.cs are optional
and can be used for things like starting or finishing quests, or shifting perspective
to a third or fourth (or technically infinite amount) member in the conversation.
