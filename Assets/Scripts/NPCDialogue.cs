using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public string npcName;
    public string[] dialogueLines;
    public bool[] autoProgressLines;
    public bool[] endDialogueLines; // Marks where the dialogue ends
    public float autoProgressDelay = 1.5f;
    public float typingSpeed = 0.05f;

    public DialogueChoice[] choices;

    public int questInProgressIndex; // What the NPC says while quest is in progress
    public int questCompletedIndex; // What the NPC says when the quest is completed
    public Quests quest; // Quest the NPC gives
}

[System.Serializable]
public class DialogueChoice
{
    public int dialogueIndex; //Dialogue line where choices appear
    public string[] choices; // Player response options
    public int[] nextDialogueIndexes; //Where choices leads
    public bool[] givesQuest; //Id choices gives quest
}
