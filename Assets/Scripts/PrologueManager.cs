using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.AI;
using System.Linq;

public class PrologueManager : MonoBehaviour
{
    public Dialogue prologueDialogue;
    public TextMeshProUGUI prologueText;
    public GameObject prologueBox;
    public GameObject player;
    private bool isTypingComplete = true;

    private Queue<(string sentence, float typingSpeed)> sentenceQueue;
    private int currentSentenceIndex = 0;
    public UnityEvent onPrologueEnd;
    public bool triggerPrologue = false;

    // Reference to the PrologueAudioManager
    public PrologueAudioManager prologueAudioManager;

    void Start()
    {
        sentenceQueue = new Queue<(string sentence, float typingSpeed)>();
        InitializePrologue(prologueDialogue);
    }

    private void InitializePrologue(Dialogue dialogue)
    {
        if (dialogue != null)
        {
            StartPrologue(dialogue);
        }
        else
        {
            EndPrologue();
        }
    }

    public void StartPrologue(Dialogue dialogue)
    {
        prologueBox.SetActive(true);
        player.GetComponent<PlayerScript>().enabled = false;
        player.GetComponent<NavMeshAgent>().enabled = false;

        if (dialogue.sentences.Length != dialogue.typingSpeeds?.Length)
        {
            Debug.LogError("Sentence and typing speed arrays in dialogue have different lengths!");
            return;
        }

        sentenceQueue.Clear();

        for (int i = 0; i < dialogue.sentences.Length; i++)
        {
            sentenceQueue.Enqueue((dialogue.sentences[i], dialogue.typingSpeeds[i]));
        }

        currentSentenceIndex = 0; // Reset the sentence index
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (isTypingComplete && sentenceQueue.Count == 0)
        {
            EndPrologue();
            return;
        }

        if (!isTypingComplete)
        {
            return;
        }

        var (sentence, speed) = sentenceQueue.Dequeue();
        StopAllCoroutines();
        isTypingComplete = false;

        // Play the corresponding voice clip based on the current sentence index
        prologueAudioManager.PlayVoiceClip(currentSentenceIndex);

        StartCoroutine(TypeSentence(sentence, speed));

        currentSentenceIndex++; // Increment the sentence index
    }

    IEnumerator TypeSentence(string sentence, float speed)
    {
        prologueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            prologueText.text += letter;
            yield return new WaitForSeconds(speed);
        }

        isTypingComplete = true;
    }

    public void EndPrologue()
    {
        prologueBox.SetActive(false);
        player.GetComponent<PlayerScript>().enabled = true;
        player.GetComponent<NavMeshAgent>().enabled = true;
        triggerPrologue = true;
    }

    void Update()
    {
        if (triggerPrologue == true)
        {
            onPrologueEnd?.Invoke();
        }
    }
}
