using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CombatUIManager : MonoBehaviour
{
    public TeamManager TM;
    public BattleManager BM;

    /*
    public TextMeshProUGUI[] CHARSHPUI;
    public TextMeshProUGUI[] CHARSNAMEUI;

    
    public Image[] CHARSSPRITE;
    public Image[] CHARSFieldSPRITE;

    public Image[] ENEMIESSPRITE;
    */
    public Sprite DEADORINVALID;

    //[SerializeField] public Image[] CHARSHPBARUI;

    [SerializeField] public int FakeItemAmount;

    [SerializeField] private GameObject ItemButtonsParentObject;
    [SerializeField] private Transform ItemButtonsParent;
    [SerializeField] private ItemSlotManager ItemButton;
    public List<ItemSlotManager> ItemUI = new();

    public Image[] TURNSYSTEMSPRITES;
    public GameObject PLAYERBUTTONS;
    public GameObject PLAYERTARGETBUTTONS;
    public GameObject PLAYERITEMBUTTONS;

    public GameObject[] BackGroundStuff;
    public GameObject PickCharItemButtons;

    public GameObject COMBATTEXTINFOPARENTOBJECT;
    public TMP_Text COMBATTEXTINFO;

    public TMP_Text ITEMNAMETEXT;
    public TMP_Text ITEMDESCTEXT;
    public TMP_Text ITEMAMOUNTTEXT;

    public TMP_Text[] CHARITEMSELECTARRAY;

    [SerializeField] CharacterStatDisplay[] playerStats;
    [SerializeField] CharacterStatDisplay[] enemyStats;

    [SerializeField] ItemSlotManager currentHighlightedSlot;
    public string TempDesc;
    public int TempAmount;

    public void UIMUpdateCharacterSprites(int characterSlot, Characters character)
    {
        // Timeline bar sprites disappear when character dies
        foreach (Image turnSprite in TURNSYSTEMSPRITES)
        {
            turnSprite.gameObject.SetActive(false);
        }
        for (int i = 0; i < BM.BattleOrder.Count; i++)
        {
            TURNSYSTEMSPRITES[i].gameObject.SetActive(true);
        }

        // Field sprites disappear left/right if hp is == 0
        if (character.Allied)
        {
            playerStats[characterSlot].CHARSFieldSPRITE.gameObject.SetActive(character.CharacterHP > 0);
        }
        else
        {
            enemyStats[characterSlot - WorldCharacterManager.TeamCharacters.Count].CHARSFieldSPRITE.gameObject.SetActive(character.CharacterHP > 0);
        }
    }

    public void Initialize()
    {
        ITEMNAMETEXT.text = "---";
        TempDesc = "---";
        TempAmount = 0;
        ITEMDESCTEXT.text = TempDesc;
        ITEMAMOUNTTEXT.text = TempAmount.ToString();

        ItemUI.Clear();

        InitializeTURNUI();
        InitializeCHARS();
        InitializeENEMIES();
        InitializeCHARSImages();
        InitializeItemButtons();
    }

    public void InitializeItemButtons()
    {
        foreach(ItemSlotManager ItemButton in ItemUI)
        {
            Destroy(ItemButton.gameObject);
        }
        ItemUI.Clear();
            for (int i = 0; i < InventorySystem.Instance.GetInventory().Length; i++) // replace 5 with inventory size
            {
                Debug.Log(i);
                if (InventorySystem.Instance.GetItemQuantity()[i] > 0 && InventorySystem.Instance.GetInventory()[i].itemType == ItemType.Consumables)
                {
                    ItemSlotManager usedButton = Instantiate(ItemButton, ItemButtonsParent);
                    ItemUI.Add(usedButton);
                    usedButton.ItemText.text = InventorySystem.Instance.GetInventory()[i].ItemName;
                    usedButton.ItemDesc = InventorySystem.Instance.GetInventory()[i].ItemDescription;
                    usedButton.ItemAmount = InventorySystem.Instance.GetItemQuantity()[i];

                    /*
                    if(usedButton.ItemText.text == "Lesser Potion")
                    {
                        usedButton.ItemHealingEffect = 500;
                    }
                    */

                    usedButton.MyIndexNum = i;
                }
            }

        for(int i = 0; i < WorldCharacterManager.TeamCharacters.Count; i++)
        {
            CHARITEMSELECTARRAY[i].text = WorldCharacterManager.TeamCharacters[i].CharacterName;
        }
    }


    void InitializeCHARSImages()
    {
        for (int i = 0; i < TM.CHARS.Count; i++)
        {
            playerStats[i].CHARSFieldSPRITE.gameObject.SetActive(true);
        }

        for (int i = 0; i < TM.ENEMIES.Count; i++)
        {
            enemyStats[i].CHARSFieldSPRITE.gameObject.SetActive(true);
        }
    }

    void InitializeCHARS()
    {
        for (int i = 0; i < TM.CHARS.Count; i++)
        {
            playerStats[i].CHARSHPUI.text = $"{TM.CHARS[i].CharacterHP.ToString()} / {TM.CHARS[i].CharacterHP.ToString()}";
            playerStats[i].CHARSNAMEUI.text = $"{TM.CHARS[i].CharacterName.ToString()}";
            playerStats[i].CHARSSPRITE.sprite = TM.CHARS[i].CharacterBattleSprite;
            playerStats[i].CHARSFieldSPRITE.sprite = TM.CHARS[i].CharacterBattleSprite;
        }
    }

    public void InitializeENEMIES()
    {
        for (int i = 0; i < TM.ENEMIES.Count; i++)
        {
            enemyStats[i].CHARSSPRITE.sprite = TM.ENEMIES[i].CharacterBattleSprite;
            enemyStats[i].CHARSNAMEUI.text = TM.ENEMIES[i].CharacterName;
        }
    }

    public void InitializeTURNUI()
    {

        for (int i = 0; i < BM.BattleOrder.Count; i++)
        {
            BM.BattleOrder[i].CharacterBattleSprite = BM.BattleOrder[i].CharacterSprite;
            TURNSYSTEMSPRITES[i].sprite = BM.BattleOrder[i].CharacterBattleSprite;
        }
    }

    void Update()
    {
        for (int i = 0; i < TM.CHARS.Count; i++)
        {
            playerStats[i].CHARSHPBARUI.fillAmount = (float)TM.CHARS[i].CharacterHP / (float)TM.CHARS[i].CharacterMAXHP;
            playerStats[i].CHARSHPUI.text = $"{TM.CHARS[i].CharacterHP.ToString()} / {TM.CHARS[i].CharacterMAXHP.ToString()}";
        }
    }

    public void HighLightItem(GameObject ItemButton)
    {
        EventSystem.current.SetSelectedGameObject(ItemButton);
        currentHighlightedSlot = EventSystem.current.currentSelectedGameObject.GetComponent<ItemSlotManager>();
        ITEMNAMETEXT.text = "---";
        TempDesc = "---";
        TempAmount = 0;
        if (!InventorySystem.Instance.CheckIfSlotIsEmpty(currentHighlightedSlot.MyIndexNum))
        {
            ITEMNAMETEXT.text = currentHighlightedSlot.ItemText.text;
            TempDesc = currentHighlightedSlot.ItemDesc;
            TempAmount = currentHighlightedSlot.ItemAmount;
            BM.TempArrayMarker = currentHighlightedSlot.MyIndexNum;
        }

        ITEMDESCTEXT.text = TempDesc;
        ITEMAMOUNTTEXT.text = TempAmount.ToString();

    }

    public void ExitHighLightItem()
    {
        ITEMNAMETEXT.text = "---";
        TempDesc = "---";
        TempAmount = 0;
        ITEMDESCTEXT.text = TempDesc;
        ITEMAMOUNTTEXT.text = TempAmount.ToString();
    }
}

[Serializable]
public struct CharacterStatDisplay
{
    public TextMeshProUGUI CHARSHPUI;
    public TextMeshProUGUI CHARSNAMEUI;


    public Image CHARSSPRITE;
    public Image CHARSFieldSPRITE;

    public Image CHARSHPBARUI;
}
