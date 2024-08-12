using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public PlayerScript player;
    public TextMeshProUGUI descriptionText;
    public Button missionButton;
    public bool questCompleted = false;

    // Reference to the next QuestGiver
    public QuestGiver nextQuestGiver;

    public void ShowQuest()
    {
        if (!questCompleted)
        {
            descriptionText.text = quest.description;
            // AcceptQuest();
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Quest is already completed. Cannot show again.");
#endif
        }
    }

    public void AcceptQuest()
    {
        if (!questCompleted && !quest.isActive)
        {
            quest.isActive = true;
            player.quest = quest;
            Debug.Log("Quest accepted: " + quest.description);
        }
    }

    public void MarkQuestCompleted()
    {
        questCompleted = true;
        quest.Complete();
        Debug.Log("Quest marked as completed");

        // Deactivate this quest giver's UI elements
        if (descriptionText != null)
            descriptionText.gameObject.SetActive(false);
        if (missionButton != null)
            missionButton.gameObject.SetActive(false);

        // Activate the next quest if available
        if (nextQuestGiver != null)
        {
            nextQuestGiver.gameObject.SetActive(true);
            nextQuestGiver.ShowQuest();
        }
        else
        {
            Debug.Log("All quests completed!");
        }
    }

    public void AssignMethod()
    {
        if (!questCompleted)
        {
            missionButton.onClick.AddListener(ShowQuest);
            Debug.Log("Mission button assignment successful");
        }
    }
}
