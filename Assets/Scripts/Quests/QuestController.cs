using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public static QuestController instance {  get; private set; }
    public List<QuestProgress> activateQuests = new();
    private QuestUI questUI;

    public List<string> handinQuestIDs = new();

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

    public void TryProgressKillQuest(int killedEnemyID)
    {
        foreach(QuestProgress quest in activateQuests)
        {
            foreach(Quests.QuestObjective questObjective in quest.objectives)
            {
                if (questObjective.type != Quests.ObjectiveType.DefeatEnemy) continue;
                if (!int.TryParse(questObjective.objectiveID, out int enemyID)) continue;
                if (killedEnemyID != enemyID) continue;
                questObjective.currentAmount++;
            }
        }
        questUI?.UpdateQuestUI();
    }
    
    public bool IsQuestCompleted(string questID)
    {
        QuestProgress quest = activateQuests.Find(q  => q.QuestID == questID);
        return quest != null && quest.objectives.TrueForAll(o => o.IsCompleted);
    }

    public void HandInQuest(string questID)
    {
        QuestProgress quest = activateQuests.Find(q => q.QuestID == questID);
        if(quest != null)
        {
            handinQuestIDs.Add(questID);
            activateQuests.Remove(quest);
            questUI?.UpdateQuestUI();
        }
    }

    public bool IsQuestHandedIn(string questID)
    {
        return handinQuestIDs.Contains(questID);
    }

    public void LoadQuestProgress(List<QuestProgress> savedQuests)
    {
        activateQuests = savedQuests ?? new();

        questUI?.UpdateQuestUI();
    }

}
