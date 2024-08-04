using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.AI;

public class DialogueManager : MonoBehaviour
{
    [HideInInspector]
    public Dialogue defaultDialogue;


    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    // public Animator animator;
    public GameObject dialogueBox;
    public GameObject player;
    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
        Initialize(defaultDialogue);
    }

    private void Initialize(Dialogue dialogue)
    {
        if (dialogue != null)
        {
            StartDialogue(dialogue, null);
        }
        else
        {
            EndDialogue();
        }
    }

    public void StartDialogue(Dialogue dialogue, UnityEvent onConversationEndEvent)
    {
        dialogueBox.SetActive(true);
        // animator.SetBool("isOpen", true);
        player.transform.Translate(Vector3.zero);
        player.GetComponent<NavMeshAgent>().enabled = false;
        player.GetComponent<PlayerScript>().enabled = false;
        player.GetComponent<Animator>().SetBool("Idle", true);
        player.GetComponent<Animator>().SetBool("Run", false);

        nameText.text = dialogue.name;
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();

        // Save the UnityEvent for later use
        onConversationEnd = onConversationEndEvent;
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    private UnityEvent onConversationEnd; // Added field to store UnityEvent

    public void EndDialogue()
    {
        // Debug.Log("End of Convo");

        // animator.enabled = false;
        dialogueBox.SetActive(false);
        // animator.SetBool("isOpen", !true);
        player.GetComponent<PlayerScript>().enabled = true;
        player.GetComponent<NavMeshAgent>().enabled = true;

        // Invoke the stored UnityEvent when conversation ends
        onConversationEnd?.Invoke();
    }
}
