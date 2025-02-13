using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheckDialogue : MonoBehaviour
{

    private bool atDialogue = false;
    private GameObject dialogueObject;
    private ManageNeeds manageNeeds;
    private bool atAlina = false;
    private bool atCassie = false;
    // Start is called before the first frame update
    void Start()
    {
        manageNeeds = GameObject.Find("Player").GetComponent<ManageNeeds>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract(InputAction.CallbackContext context) {
        if (atCassie ) {
            if (manageNeeds.hasLetter && atDialogue) {
                dialogueObject.GetComponent<DialogueTrigger>().TriggerDialogue();
            }
        }
        else {
            if (atDialogue) {
                dialogueObject.GetComponent<DialogueTrigger>().TriggerDialogue();
            }
            if (atAlina) {
                manageNeeds.hasLetter = true;
            }
        }

        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Alina")) {
            atAlina = true;
        }
        if (other.gameObject.CompareTag("Cassie")) {
            atCassie = true;
        }
        if (other.gameObject.GetComponent<DialogueTrigger>()) {
            atDialogue = true;
            dialogueObject = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.GetComponent<DialogueTrigger>()) {
            atDialogue = false;
        }
        if (other.gameObject.CompareTag("Cassie")) {
            atCassie = false;
        }
        if (other.gameObject.CompareTag("Alina")) {
            atAlina = false;
        }
    } 
}
