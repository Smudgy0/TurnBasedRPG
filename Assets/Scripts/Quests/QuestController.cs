using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public static QuestController instance {  get; private set; }
    public List<QuestProgress> activateQuests = new();
    private QuestUI questUI;

    [System.Obsolete]
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        questUI = FindObjectOfType<QuestUI>();
    }

    public void AcceptQuest(Quests quest)
    {
        if (IsQuestActive(quest.questID)) return;
        
        activateQuests.Add(new QuestProgress(quest));

        questUI.UpdateQuestUI();
    }

    public bool IsQuestActive(string questID) => activateQuests.Exists(q => q.QuestID == questID);
}
