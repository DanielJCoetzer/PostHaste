using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue.Sentence[] dialogue;

    public void TriggerDialogue () 
    {
        FindAnyObjectByType<DialogueManager>().StartDialogue(dialogue);
    }
}
