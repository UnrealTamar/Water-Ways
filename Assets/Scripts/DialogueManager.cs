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
    public bool tryDialogue = false;


    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    // public Animator animator;
    public GameObject dialogueBox;
    public GameObject player;
    private Queue<(string sentence, float typingSpeed)> sentences; // Changed queue type

    void Start()
    {
        sentences = new Queue<(string sentence, float typingSpeed)>(); // Initialize queue with tuples
        Initialize(defaultDialogue);
    }

    private void Update()
    {
        if (tryDialogue == true)
        {
            player.transform.Translate(Vector3.zero);
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.GetComponent<PlayerScript>().enabled = false;
            player.GetComponent<Animator>().SetBool("Idle", true);
            player.GetComponent<Animator>().SetBool("Run", false);
        }
        else
        {
            player.GetComponent<NavMeshAgent>().enabled = true;
            player.GetComponent<PlayerScript>().enabled = true;
        }
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

        tryDialogue = true;
        player.transform.Translate(Vector3.zero);
        player.GetComponent<NavMeshAgent>().enabled = false;
        player.GetComponent<PlayerScript>().enabled = false;
        player.GetComponent<Animator>().SetBool("Idle", true);
        player.GetComponent<Animator>().SetBool("Run", false);

        nameText.text = dialogue.name;
        sentences.Clear();

        // Enqueue sentences with their corresponding typing speeds (assuming typingSpeeds exists in Dialogue)
        for (int i = 0; i < dialogue.sentences.Length; i++)
        {
            sentences.Enqueue((dialogue.sentences[i], dialogue.typingSpeeds?[i] ?? 0.05f)); // Use default if typingSpeeds is null
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

        // Dequeue the tuple containing sentence and typing speed
        var sentenceTuple = sentences.Dequeue();
        string sentence = sentenceTuple.sentence;
        float typingSpeed = sentenceTuple.typingSpeed;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence, typingSpeed));
    }

    IEnumerator TypeSentence(string sentence, float typingSpeed)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private UnityEvent onConversationEnd; // Added field to store UnityEvent

    public void EndDialogue()
    {
        // Debug.Log("End of Convo");

        // animator.enabled = false;
        dialogueBox.SetActive(false);
        // animator.SetBool("isOpen", !true);
        //player.GetComponent<PlayerScript>().enabled = true;
        //player.GetComponent<NavMeshAgent>().enabled = true;
        tryDialogue = false;

        // Invoke the stored UnityEvent when conversation ends
        onConversationEnd?.Invoke();
    }
}
