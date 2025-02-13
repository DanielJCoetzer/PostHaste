using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class DialogueManager : MonoBehaviour
{
    private Queue<Dialogue.Sentence> sentences;

    public TMP_Text nameText;
    public TMP_Text sentenceText;
    public Image dialogueBox;
    public Image textBox;
    public Image nameBox;
    public Image nextButton;


    void Start()
    {
        sentences = new Queue<Dialogue.Sentence>();
        ViewBox(false);
    }

    public void StartDialogue(Dialogue.Sentence[] dialogue) {
        ViewBox(true);

        sentences.Clear();

        foreach (Dialogue.Sentence sentence in dialogue) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence() 
    {
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }



        Dialogue.Sentence sentence = sentences.Dequeue();
        nameText.text = sentence.name;
        sentenceText.text = sentence.text;
    }

    void EndDialogue() {
        ViewBox(false);
    }

    void ViewBox(bool decision) {
        if (decision) {
            dialogueBox.enabled = true;
            textBox.enabled = true;
            nameBox.enabled = true;
            nameText.enabled = true;
            sentenceText.enabled = true;
            nextButton.enabled = true;
        }
        else {
            dialogueBox.enabled = false;
            textBox.enabled = false;
            nameBox.enabled = false;
            nameText.enabled = false;
            sentenceText.enabled = false;
            nextButton.enabled = false;
        }
    }


}
