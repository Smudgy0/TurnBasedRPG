using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TeamManagerUIScript : MonoBehaviour
{
    public static TeamManagerUIScript Instance;

    public GameObject TeamSelectMenu;

    [SerializeField] private TeamManagerSlots UnusedTeamMemberButton, UsedTeamMemberButton;
    [SerializeField] private Transform unusedTeamParent, usedTeamParent;
    public List <TeamManagerSlots> TeamUI = new ();
    public List<TeamManagerSlots> UnusedTeamUI = new();

    [SerializeField] TeamManagerSlots currentHighlightedSlot;

    public CharacterStatSheet CharStatSheet = new();

    public string TempName;

    static bool MenuOpen = false;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadCharacterList();
    }

    public void LoadCharacterList()
    {
        //ReCheckCurrentTeam(); At the start of the game you have nobody on your team so only re-enable when adding saving
        ReCheckPossibleTeam();
        ReCheckCurrentTeam();
    }

    /// <summary>
    /// Code resets the current team UI to make sure the right team members are showing.
    /// </summary>
    public void ReCheckCurrentTeam()
    {
        // disables the character buttons in the 3x1 menu in the team manager
        foreach (var team in TeamUI)
        {
            Debug.Log("DELETE THE BUTTON");
            Destroy(team.gameObject);
        }
        TeamUI.Clear();

        for (int i = 0; i < WorldCharacterManager.TeamCharacters.Count; i++)
        {
            TeamManagerSlots usedButton = Instantiate(UsedTeamMemberButton, usedTeamParent);
            TeamUI.Add(usedButton);

            usedButton.CHARImage.sprite = WorldCharacterManager.TeamCharacters[i].CharacterSprite;
            usedButton.CHARNameTextBox.text = WorldCharacterManager.TeamCharacters[i].CharacterName;
            usedButton.HPFillBar.fillAmount = (float)WorldCharacterManager.TeamCharacters[i].CharacterHP / (float)WorldCharacterManager.TeamCharacters[i].CharacterMAXHP;
            usedButton.HPBarHPText.text = $"{WorldCharacterManager.TeamCharacters[i].CharacterHP.ToString()} / {WorldCharacterManager.TeamCharacters[i].CharacterMAXHP.ToString()}";
        }
    }

    /// <summary>
    /// Code resets the 3x3 team UI to show characters which the player can choose.
    /// </summary>
    public void ReCheckPossibleTeam()
    {
        foreach (var team in UnusedTeamUI)
        {
            Destroy(team.gameObject);
        }
        UnusedTeamUI.Clear();

        for (int i = 0; i < WorldCharacterManager.UnusedTeamCharacters.Count; i++)
        {
            TeamManagerSlots unusedButton = Instantiate(UnusedTeamMemberButton, unusedTeamParent);
            UnusedTeamUI.Add(unusedButton);
            UnusedTeamUI[i].CHARImage.sprite = WorldCharacterManager.UnusedTeamCharacters[i].CharacterSprite;
            UnusedTeamUI[i].CHARNameTextBox.text = WorldCharacterManager.UnusedTeamCharacters[i].CharacterName;
            UnusedTeamUI[i].HPFillBar.fillAmount = (float)WorldCharacterManager.UnusedTeamCharacters[i].CharacterHP / (float)WorldCharacterManager.UnusedTeamCharacters[i].CharacterMAXHP;
            UnusedTeamUI[i].HPBarHPText.text = $"{WorldCharacterManager.UnusedTeamCharacters[i].CharacterHP.ToString()} / {WorldCharacterManager.UnusedTeamCharacters[i].CharacterMAXHP.ToString()}";
        }
    }

    /// <summary>
    /// Hides the team selection menu.
    /// </summary>
    public void ToggleCharacterList()
    {
        TeamSelectMenu.SetActive(!TeamSelectMenu.activeSelf);
    }

    /// <summary>
    /// sets UI in a box (ArrayVal number is which box in the list) it sets to null
    /// </summary>
    public void RemoveUI(int ArrayVal)
    {
        TeamUI[ArrayVal].CHARImage.sprite = null;
        TeamUI[ArrayVal].CHARNameTextBox.text = "---";
        TeamUI[ArrayVal].HPFillBar.fillAmount = 1;
        TeamUI[ArrayVal].HPBarHPText.text = "? / ?";

        //ReCheckCurrentTeam();
    }

    public void ManageTeamMenu(InputAction.CallbackContext context)
    {
        //EventSystem.current.SetSelectedGameObject(UnusedTeamUI[0].gameObject);
        Debug.Log("ManageTeamMenu Read");
        ToggleCharacterList();
    }

    private void Update()
    {
        if(EventSystem.current.currentSelectedGameObject != null)
            currentHighlightedSlot = EventSystem.current.currentSelectedGameObject.GetComponent<TeamManagerSlots>();
        if (currentHighlightedSlot != null)
        {
            TempName = currentHighlightedSlot.CHARNameTextBox.text;
        }

        for (int i = 0; i < WorldCharacterManager.AllCharacters.Count; i++)
        {
            if (TempName == WorldCharacterManager.AllCharacters[i].CharacterName)
            {
                CharStatSheet.CHARName.text = WorldCharacterManager.AllCharacters[i].CharacterName;
                CharStatSheet.CHARLevel.text = $"Level: {WorldCharacterManager.AllCharacters[i].CharacterLevel} / 50";
                CharStatSheet.CHARHP.text = $"{WorldCharacterManager.AllCharacters[i].CharacterHP} / {WorldCharacterManager.AllCharacters[i].CharacterMAXHP}";
                CharStatSheet.CHARAttack.text = $"Attack: {WorldCharacterManager.AllCharacters[i].CharacterAttack}";
                CharStatSheet.CHARDefense.text = $"Defense: {WorldCharacterManager.AllCharacters[i].CharacterDefense}";
                CharStatSheet.CHARSpeed.text = $"Speed: {WorldCharacterManager.AllCharacters[i].CharacterSpeed}";

                CharStatSheet.CharIcon.sprite = WorldCharacterManager.AllCharacters[i].CharacterSprite;
                CharStatSheet.LevelProgressBar.fillAmount = (float)WorldCharacterManager.AllCharacters[i].CharacterEXP / (float)WorldCharacterManager.AllCharacters[i].CharacterEXPRequirement;
                CharStatSheet.HealthProgressBar.fillAmount = (float)WorldCharacterManager.AllCharacters[i].CharacterHP / (float)WorldCharacterManager.AllCharacters[i].CharacterMAXHP;
            }
        }

        //currentHighlightedSlot = EventSystem.current.GetComponent<TeamManagerSlots>();
        // previewCharacter.charName = CurrentTeam.TeamCharacters[currentHighlightedSlot.transform.GetSiblingIndex()].CharacterName;
    }
}

[Serializable]
public struct CharacterStatSheet
{
    public TMP_Text CHARName;
    public TMP_Text CHARLevel;
    public TMP_Text CHARHP;
    public TMP_Text CHARAttack;
    public TMP_Text CHARDefense;
    public TMP_Text CHARSpeed;

    public Image CharIcon;
    public Image LevelProgressBar;
    public Image HealthProgressBar;
}
