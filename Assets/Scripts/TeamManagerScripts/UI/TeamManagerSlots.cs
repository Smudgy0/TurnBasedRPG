using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamManagerSlots : MonoBehaviour
{
    public Image CHARImage;
    public Image HPFillBar;
    public TextMeshProUGUI CHARNameTextBox;
    public TextMeshProUGUI HPBarHPText;

    public void GetButtonID()
    {
        CHARNameTextBox.text = "vnuidnk";
        CHARNameTextBox.text = "vnudnk";
    }

    public void AddCharacterToTeam_Button()
    {
        WorldCharacterManager.Instance.AddToTeam(transform.GetSiblingIndex());
    }

    public void RemoveCharacterFromTeam_Button()
    {
        WorldCharacterManager.Instance.RemoveFromTeam(transform.GetSiblingIndex());
    }
}
