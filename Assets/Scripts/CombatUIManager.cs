using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

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

    public Image[] TURNSYSTEMSPRITES;
    public GameObject PLAYERBUTTONS;
    public GameObject PLAYERTARGETBUTTONS;

    public GameObject COMBATTEXTINFOPARENTOBJECT;
    public TMP_Text COMBATTEXTINFO;

    [SerializeField] CharacterStatDisplay[] playerStats;
    [SerializeField] CharacterStatDisplay[] enemyStats;

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
            enemyStats[characterSlot - playerStats.Length].CHARSFieldSPRITE.gameObject.SetActive(character.CharacterHP > 0);
        }
    }

    public void Initialize()
    {
        InitializeTURNUI();
        InitializeCHARS();
        InitializeENEMIES();
        InitializeCHARSImages();
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
