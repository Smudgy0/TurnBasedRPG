using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TeamManagerUIScript : MonoBehaviour
{
    [SerializeField] UICharacterDisplay[] CharacterData;
    [SerializeField] UICurrentTeamDisplay[] TeamData;
    public WorldCharacterManager WCM;

    public GameObject TeamSelectMenu;
    public GameObject TeamSelectMenuButton;

    public void LoadCharacterList()
    {
        TeamSelectMenu.SetActive(true);
        TeamSelectMenuButton.SetActive(false);

        for(int i = 0 ; i < WorldCharacterManager.AllCharacters.Count; i++)
        {
            CharacterData[i].CHARImage.sprite = WorldCharacterManager.AllCharacters[i].CharacterSprite;
            CharacterData[i].CHARNameTextBox.text = WorldCharacterManager.AllCharacters[i].CharacterName;
            CharacterData[i].HPFillBar.fillAmount = (float)WorldCharacterManager.AllCharacters[i].CharacterHP / (float)WorldCharacterManager.AllCharacters[i].CharacterMAXHP;
            CharacterData[i].HPBarHPText.text = $"{WorldCharacterManager.AllCharacters[i].CharacterHP.ToString()} / {WorldCharacterManager.AllCharacters[i].CharacterMAXHP.ToString()}";
        }

        for(int i = 0 ; i < CurrentTeam.TeamCharacters.Count; i++)
        {
            TeamData[i].CHARImage.sprite = CurrentTeam.TeamCharacters[i].CharacterSprite;
            TeamData[i].CHARNameTextBox.text = CurrentTeam.TeamCharacters[i].CharacterName;
            TeamData[i].HPFillBar.fillAmount = (float)CurrentTeam.TeamCharacters[i].CharacterHP / (float)CurrentTeam.TeamCharacters[i].CharacterMAXHP;
            TeamData[i].HPBarHPText.text = $"{CurrentTeam.TeamCharacters[i].CharacterHP.ToString()} / {CurrentTeam.TeamCharacters[i].CharacterMAXHP.ToString()}";
        }
    }

    public void CloseCharacterList()
    {
        TeamSelectMenu.SetActive(false);
        TeamSelectMenuButton.SetActive(true);
    }

    public void RemoveUI(int ArrayVal)
    {
        TeamData[ArrayVal].CHARImage.sprite = null;
        TeamData[ArrayVal].CHARNameTextBox.text = "---";
        TeamData[ArrayVal].HPFillBar.fillAmount = 1;
        TeamData[ArrayVal].HPBarHPText.text = "? / ?";
    }
}

[Serializable]
public struct UICharacterDisplay
{
    public Image CHARImage;
    public Image HPFillBar;
    public TextMeshProUGUI CHARNameTextBox;
    public TextMeshProUGUI HPBarHPText;
}

[Serializable]
public struct UICurrentTeamDisplay
{
    public Image CHARImage;
    public Image HPFillBar;
    public TextMeshProUGUI CHARNameTextBox;
    public TextMeshProUGUI HPBarHPText;
}
