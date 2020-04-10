using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public string dialogueSpeaker;
    public ScriptedEvent scriptedEvent;
    public bool eventIsFollowing = false;
    [HideInInspector] public string[] dialogue;

    [TextArea(3, 10)]
    public string[] dialogues;

    [TextArea(3, 10)]
    public string[] dialoguesAnglais;

    private void Start()
    {
        if (GameManager.instance.langue == GameManager.langues.english)
            dialogue = dialoguesAnglais;
        else
            dialogue = dialogues;

        scriptedEvent = GetComponent<ScriptedEvent>();
    }

    public void StartDialogue()
    {
        DialogueManager.instance.myTrigger = this;
        DialogueManager.instance.StartDialogue(transform, dialogueSpeaker, dialogue);
    }

    public void StartDialogueFollowedByEvent()
    {
        eventIsFollowing = true;
        DialogueManager.instance.myTrigger = this;
        DialogueManager.instance.StartDialogue(transform, dialogueSpeaker, dialogue);
    }

    public void TriggerScriptedEvent()
    {
        scriptedEvent.Activate();
        eventIsFollowing = false;
    }
}
