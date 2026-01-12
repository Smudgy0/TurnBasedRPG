using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
}
