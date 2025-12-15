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
        if (CurrentTeam.TeamCharacters.Count < 3)
        {
            this.gameObject.SetActive(false);
            WorldCharacterManager.Instance.AddToTeam(transform.GetSiblingIndex());
        }
    }

    public void RemoveCharacterFromTeam_Button()
    {
        TeamManagerUIScript.Instance.ReEnableButtons(transform.GetSiblingIndex());
        WorldCharacterManager.Instance.RemoveFromTeam(transform.GetSiblingIndex());
    }
}
