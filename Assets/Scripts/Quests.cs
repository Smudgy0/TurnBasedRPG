using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Quests;

[CreateAssetMenu(menuName = "Quests/Quest")]
public class Quests : ScriptableObject
{
    public string questID;
    public string questName;
    public string description;
    public List<QuestObjective> objectives;

    /// <summary>
    /// Called when A scriptable obj is edited
    /// </summary>
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(questID))
        {
            questID = questName + Guid.NewGuid().ToString();
        }
    }

    [System.Serializable]
    public class QuestObjective
    {
        public string objectiveID;
        public string description;
        public ObjectiveType type;
        public int requiredAmount;
        public int currentAmount;

        public bool IsCompleted => currentAmount >= requiredAmount;
    }

    public enum ObjectiveType { CollectItem, DefeatEnemy, ReachLocation, TalkNPC, Custom }  
}

[System.Serializable]
public class QuestProgress
{
    public Quests quest;
    public List<QuestObjective> objectives;

    public QuestProgress(Quests quest)
    {
        this.quest = quest;
        objectives = new List<QuestObjective>();

        foreach (var obj in quest.objectives)
        {
            objectives.Add(new QuestObjective
            {
                objectiveID = obj.objectiveID,
                description = obj.description,
                type = obj.type,
                requiredAmount = obj.requiredAmount,
                currentAmount = 0

            });
        }
    }

    public bool IsCompleted => objectives.TrueForAll(o => o.IsCompleted);

    public string QuestID => quest.questID;
}
