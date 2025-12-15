using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEditor;
using Unity.VisualScripting;

public class TeamManagerUIScript : MonoBehaviour
{
    public static TeamManagerUIScript Instance;
    public WorldCharacterManager WCM;

    public GameObject TeamSelectMenu;
    //public GameObject TeamSelectMenuButton;
    [SerializeField] TeamManagerSlots currentHighlightedSlot;
    public TeamManagerSlots[] TeamUI;
    public TeamManagerSlots[] UnusedTeamUI;

    public CharacterStatSheet CharStatSheet = new();

    public string TempName;

    static bool MenuOpen = false;
    private void Awake()
    {
        Instance = this;
    }
    public void LoadCharacterList()
    {
        TeamSelectMenu.SetActive(true);

        for(int i = 0 ; i < UnusedTeamUI.Length; i++)
        {
            UnusedTeamUI[i].gameObject.SetActive(false);
        }

        for(int i = 0 ; i < TeamUI.Length; i++)
        {
            TeamUI[i].gameObject.SetActive(false);
        }

        for(int i = 0 ; i < WorldCharacterManager.AllCharacters.Count; i++)
        {
            UnusedTeamUI[i].gameObject.SetActive(true);

            UnusedTeamUI[i].CHARImage.sprite = WorldCharacterManager.AllCharacters[i].CharacterSprite;
            UnusedTeamUI[i].CHARNameTextBox.text = WorldCharacterManager.AllCharacters[i].CharacterName;
            UnusedTeamUI[i].HPFillBar.fillAmount = (float)WorldCharacterManager.AllCharacters[i].CharacterHP / (float)WorldCharacterManager.AllCharacters[i].CharacterMAXHP;
            UnusedTeamUI[i].HPBarHPText.text = $"{WorldCharacterManager.AllCharacters[i].CharacterHP.ToString()} / {WorldCharacterManager.AllCharacters[i].CharacterMAXHP.ToString()}";
        }

        for(int i = 0 ; i < CurrentTeam.TeamCharacters.Count; i++)
        {
            TeamUI[i].gameObject.SetActive(true);

            TeamUI[i].CHARImage.sprite = CurrentTeam.TeamCharacters[i].CharacterSprite;
            TeamUI[i].CHARNameTextBox.text = CurrentTeam.TeamCharacters[i].CharacterName;
            TeamUI[i].HPFillBar.fillAmount = (float)CurrentTeam.TeamCharacters[i].CharacterHP / (float)CurrentTeam.TeamCharacters[i].CharacterMAXHP;
            TeamUI[i].HPBarHPText.text = $"{CurrentTeam.TeamCharacters[i].CharacterHP.ToString()} / {CurrentTeam.TeamCharacters[i].CharacterMAXHP.ToString()}";
        }
    }

    public void CloseCharacterList()
    {
        TeamSelectMenu.SetActive(false);
    }

    public void RemoveUI(int ArrayVal)
    {
        TeamUI[ArrayVal].CHARImage.sprite = null;
        TeamUI[ArrayVal].CHARNameTextBox.text = "---";
        TeamUI[ArrayVal].HPFillBar.fillAmount = 1;
        TeamUI[ArrayVal].HPBarHPText.text = "? / ?";
    }

    public void ManageTeamMenu(InputAction.CallbackContext context)
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(UnusedTeamUI[0].gameObject);
        Debug.Log("ManageTeamMenu Read");
        if (context.performed && MenuOpen == false)
        {
            LoadCharacterList();
            MenuOpen = true; 
        }
        else if (context.performed && MenuOpen == true)
        {
            MenuOpen = false; 
            CloseCharacterList();
        }
    }

    public void ReEnableButtons(int j)
    {
        for (int i = 0; i < WorldCharacterManager.AllCharacters.Count; i++)
        {
            if (WorldCharacterManager.AllCharacters[i] == CurrentTeam.TeamCharacters[j])
            {
                UnusedTeamUI[i].gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
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
