using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    public TeamManager TM;
    public CombatUIManager CUIM;
    public GameObject[] CHOOSETARGETENEMY;
    public TMP_Text[] CHOOSETARGETENEMYTEXT;

    public List<Characters> BattleOrder = new();
    public Characters[] charactersInBattle;
    public int SortingNumber = 0;

    private Button slimeButton;
    private bool MenuActive = true;
    private bool starting = true;

    private int EnemyActionPicker;
    private int EnemyTargetPicker;
    private bool DelayTimerActive = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!starting)
        {
            return;
        }

        Debug.Log("Awake");

        for (int i = 0; i < TM.CHARS.Count; i++)
        {
            BattleOrder.Add(TM.CHARS[i]);
        }

        for (int i = 0; i < TM.ENEMIES.Count; i++)
        {
            BattleOrder.Add(TM.ENEMIES[i]);
        }
        charactersInBattle = BattleOrder.ToArray();

        for (int i = 0; i < BattleOrder.Count; i++)
        {
            Debug.Log(i);
            CUIM.UIMUpdateCharacterSprites(i, BattleOrder[i]);
        }

        SortSideOrders();

        CUIM.Initialize();
        InitializeTargetOptions();

        starting = false;
    }

    public void TargetNumber(int TargetNum)
    {
        HideTargets();
        ShowTargets();
        BattleOrder[0].DisableDefence();
        TM.ENEMIES[TargetNum].CharacterHP = TM.ENEMIES[TargetNum].CharacterHP - BattleOrder[0].CharacterAttack;
        CUIM.COMBATTEXTINFO.text = $"{BattleOrder[0].CharacterName} does {BattleOrder[0].CharacterAttack} damage to {TM.ENEMIES[TargetNum].CharacterName}!";
        UpdateBattleOrder();
    }

    public void DefendButton()
    {
        BattleOrder[0].Defend();
    }

    void UpdateBattleOrder()
    {
        Characters tempChar = BattleOrder[0];
        BattleOrder.RemoveAt(0);
        BattleOrder.Add(tempChar);
    }

    void CHECKTURNUI()
    {
        //Debug.Log("CHECKTURNUI");
        for (int i = 0; i < BattleOrder.Count; i++)
        {
            CUIM.TURNSYSTEMSPRITES[i].sprite = BattleOrder[i].CharacterBattleSprite;
        }

        /*
        for (int i = 0; i < UIM.CHARSFieldSPRITE.Length; i++)
        {
            UIM.CHARSFieldSPRITE.sprite = TM.CHARS[i].CharacterBattleSprite;
        }
        for (int i = 0; i < UIM.ENEMIESSPRITE.Length; i++)
        {
            UIM.ENEMIESSPRITE.sprite = TM.ENEMIES[i].CharacterBattleSprite;
        }
        */
    }

    public void ShowTargets()
    {
        //Debug.Log("ShowTargets");
        CHOOSETARGETENEMY[0].SetActive(true);
        CHOOSETARGETENEMY[1].SetActive(true);
        CHOOSETARGETENEMY[2].SetActive(true);
        CHOOSETARGETENEMY[3].SetActive(true);

        CUIM.PLAYERBUTTONS.SetActive(false);
        CUIM.PLAYERTARGETBUTTONS.SetActive(true);

        MenuActive = false;
    }

    public void HideTargets()
    {
        //Debug.Log("HideTargets");
        CHOOSETARGETENEMY[0].SetActive(false);
        CHOOSETARGETENEMY[1].SetActive(false);
        CHOOSETARGETENEMY[2].SetActive(false);
        CHOOSETARGETENEMY[3].SetActive(false);

        CUIM.PLAYERBUTTONS.SetActive(true);
        CUIM.PLAYERTARGETBUTTONS.SetActive(false);

        MenuActive = true;
    }

    void InitializeTargetOptions()
    {
        //Debug.Log("InitializeTargetOptions");
        if (TM.ENEMIES[0] != null)
        {
            CHOOSETARGETENEMYTEXT[0].text = TM.ENEMIES[0].CharacterName.ToString();
        }
        if (TM.ENEMIES[1] != null)
        {
            CHOOSETARGETENEMYTEXT[1].text = TM.ENEMIES[1].CharacterName.ToString();
        }
        /*if (TM.ENEMIES[2] != null)
        {
            CHOOSETARGETENEMYTEXT[2].text = TM.ENEMIES[2].CharacterName.ToString();
        }*/
    }

    void SortSideOrders()
    {
        // Start of ordering
        Debug.Log("Sorting");
        var sorted = BattleOrder.OrderByDescending(item => item.CharacterSpeed);
        BattleOrder = sorted.ToList();

        /*if (BattleOrder[0].CharacterSpeed < BattleOrder[1].CharacterSpeed)
        {
            SwapArray(0 , 1, BattleOrder[0]);
        }

        if (BattleOrder[1].CharacterSpeed < BattleOrder[2].CharacterSpeed)
        {
            SwapArray(1 , 2, BattleOrder[1]);
        }

        if (BattleOrder[2].CharacterSpeed < BattleOrder[3].CharacterSpeed)
        {
            SwapArray(2 , 3, BattleOrder[2]);
        }

        if (BattleOrder[3].CharacterSpeed < BattleOrder[4].CharacterSpeed)
        {
            SwapArray(3 , 4, BattleOrder[3]);
        }

        if (BattleOrder[4].CharacterSpeed < BattleOrder[5].CharacterSpeed)
        {
            SwapArray(4 , 5, BattleOrder[4]);
        }

        SortingNumber++;*/
    }

    void SwapArray(int ArrayVal1, int ArrayVal2, Characters SavedChar)
    {
        //Debug.Log("Swapping");
        BattleOrder[ArrayVal1] = BattleOrder[ArrayVal2];
        BattleOrder[ArrayVal2] = SavedChar; 
    }

    // Update is called once per frame
    void Update()
    {
        CHECKTURNUI();

        // Player Turn
        if (BattleOrder[0].Allied == true && BattleOrder[0].CharacterHP > 0)
        {
            CUIM.PLAYERBUTTONS.SetActive(true);
        }

        // Enemy Turn
        if(BattleOrder[0].Allied == false && BattleOrder[0].CharacterHP > 0 && DelayTimerActive == false)
        {
            CUIM.PLAYERBUTTONS.SetActive(false);
            CUIM.PLAYERTARGETBUTTONS.SetActive(false);
            DelayTimerActive = true;
            StartCoroutine (EnemyTurn());
        }

        // If character is dead, skip their turn DEPRECATED WHEN BATTLE ORDER REMOVES THEM FROM THE LIST
        if (BattleOrder[0].CharacterHP !< 0)
        {
            UpdateBattleOrder();
        }

        // Kill a character if their health is 0 or lower
        for (int i = 0; i < charactersInBattle.Length; i++)
        {
            if (charactersInBattle[i].CharacterHP <= 0)
            {
                for (int j = 0; j < BattleOrder.Count; j++)
                {
                    if (charactersInBattle[i] == BattleOrder[j])
                    {
                        CharacterDeath(i, j, charactersInBattle[i]);
                    }
                }
            }
        }
    }

    public void CharacterDeath(int whichSlot, int currentPositionInBattleOrder, Characters character)
    {
        RemoveFromBattleOrder(currentPositionInBattleOrder);
        CUIM.UIMUpdateCharacterSprites(whichSlot, character);
    }

    void RemoveFromBattleOrder(int whichSlot)
    {
        BattleOrder.RemoveAt(whichSlot);
    }

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(3);


        EnemyActionPicker = UnityEngine.Random.Range(0,2);

        if(EnemyActionPicker == 1)
        {
            BattleOrder[0].Defending = true;
            CUIM.COMBATTEXTINFO.text = $"{BattleOrder[0].CharacterName} Defends!";
            Debug.Log("Enemy Defends");
        }
        else
        {
            BattleOrder[0].Defending = false;
            EnemyTargetPicker = UnityEngine.Random.Range(0,3);
            TM.CHARS[EnemyTargetPicker].CharacterHP = TM.CHARS[EnemyTargetPicker].CharacterHP - BattleOrder[0].CharacterAttack;
            Debug.Log("Enemy Attacks");
            CUIM.COMBATTEXTINFO.text = $"{BattleOrder[0].CharacterName} does {BattleOrder[0].CharacterAttack} damage to {TM.CHARS[EnemyTargetPicker].CharacterName}!";
        }

        CUIM.PLAYERBUTTONS.SetActive(true);
        UpdateBattleOrder();
        DelayTimerActive = false;

    }
}
