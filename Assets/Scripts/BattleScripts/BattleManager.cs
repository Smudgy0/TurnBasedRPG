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
    public void InitializeStart()
    {
        if (!starting)
        {
            return;
        }

        Debug.Log("Awake");

        for (int i = 0; i < CurrentTeam.TeamCharacters.Count; i++)
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

        //TM.SetTeams();
        SortSideOrders();

        CUIM.Initialize();
        InitializeTargetOptions();

        //starting = false;
    }

    public void TargetNumber(int TargetNum)
    {
        HideTargets();
        ShowTargets();
        BattleOrder[0].DisableDefence();

        int TempDamage = 0;

        if (TM.ENEMIES[TargetNum].Defending == false)
        {
            TempDamage = BattleOrder[0].CharacterAttack;

            if(TempDamage < 0)
            {
                TempDamage = 0;
            }

            TM.ENEMIES[TargetNum].CharacterHP = TM.ENEMIES[TargetNum].CharacterHP - TempDamage;
            CUIM.COMBATTEXTINFO.text = $"{BattleOrder[0].CharacterName} does {BattleOrder[0].CharacterAttack} damage to {TM.ENEMIES[TargetNum].CharacterName}!";
        }
        else if(TM.ENEMIES[TargetNum].Defending == true)
        {
            TempDamage = BattleOrder[0].CharacterAttack - TM.ENEMIES[TargetNum].CharacterDefense;

            if (TempDamage < 0)
            {
                TempDamage = 0;
            }

            TM.ENEMIES[TargetNum].CharacterHP = TM.ENEMIES[TargetNum].CharacterHP - TempDamage;
            CUIM.COMBATTEXTINFO.text = $"{BattleOrder[0].CharacterName} does {TempDamage} damage to {TM.ENEMIES[TargetNum].CharacterName}!";
        }


        UpdateBattleOrder();
    }

    public void DefendButton()
    {
        BattleOrder[0].Defend();
        CUIM.COMBATTEXTINFO.text = $"{BattleOrder[0].CharacterName} Defends!";
        UpdateBattleOrder();
    }

    public void Flee()
    {
        BattleOrder[0].DisableDefence();
        int RNDChanceToFlee = 0;
        RNDChanceToFlee = UnityEngine.Random.Range(0, 101);

        if(RNDChanceToFlee > 90)
        {
            CUIM.COMBATTEXTINFO.text = "The Team Successfully Flees!";

            // add function to return to game world!
        }
        else
        {
            CUIM.COMBATTEXTINFO.text = "The Team Fails To Flee!";
            UpdateBattleOrder();
        }
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
        for (int i = 0; i < CHOOSETARGETENEMY.Length; i++)
        {
            CHOOSETARGETENEMY[i].SetActive(true);
        }

        CUIM.PLAYERBUTTONS.SetActive(false);
        CUIM.PLAYERTARGETBUTTONS.SetActive(true);

        //MenuActive = false;
    }

    public void HideTargets()
    {
        //Debug.Log("HideTargets");
        for (int i = 0; i < CHOOSETARGETENEMY.Length; i++)
        {
            CHOOSETARGETENEMY[i].SetActive(false);
        }

        CUIM.PLAYERBUTTONS.SetActive(true);
        CUIM.PLAYERTARGETBUTTONS.SetActive(false);

        //MenuActive = true;
    }

    void InitializeTargetOptions()
    {
        //Debug.Log("InitializeTargetOptions");
        for (int i = 0; i < TM.ENEMIES.Count; i++)
        {
            if (TM.ENEMIES[i] != null)
            {
                CHOOSETARGETENEMYTEXT[i].text = TM.ENEMIES[i].CharacterName.ToString();
            }
        }
    }

    void SortSideOrders()
    {
        // Start of ordering
        Debug.Log("Sorting");
        var sorted = BattleOrder.OrderByDescending(item => item.CharacterSpeed);
        BattleOrder = sorted.ToList();
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
        for (int i = 0; i < TM.ENEMIES.Count; i++)
        {
            if (TM.ENEMIES[i].CharacterHP < 0)
            {
                CHOOSETARGETENEMY[i].SetActive(false);
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

        if(EnemyActionPicker == 1) // enemy defends
        {
            BattleOrder[0].Defending = true;
            CUIM.COMBATTEXTINFO.text = $"{BattleOrder[0].CharacterName} Defends!";
            Debug.Log("Enemy Defends");
            BattleOrder[0].Defend();
        }
        else
        {
            int Tempdamage = 0;

            BattleOrder[0].DisableDefence();
            BattleOrder[0].Defending = false;
            EnemyTargetPicker = Random.Range(0,CurrentTeam.TeamCharacters.Count);

            if(TM.CHARS[EnemyTargetPicker].Defending == false) // if player does not defend, their characters defence is not taken into account
            {
                Tempdamage = BattleOrder[0].CharacterAttack;

                if(Tempdamage < 0)
                {
                    Tempdamage = 0;
                }

                TM.CHARS[EnemyTargetPicker].CharacterHP = TM.CHARS[EnemyTargetPicker].CharacterHP - Tempdamage;
                CUIM.COMBATTEXTINFO.text = $"{BattleOrder[0].CharacterName} does {Tempdamage} damage to {TM.CHARS[EnemyTargetPicker].CharacterName}!";
            }
            else if(TM.CHARS[EnemyTargetPicker].Defending == true) // if player defends, their characters defence is taken into account
            {
                Tempdamage = BattleOrder[0].CharacterAttack - TM.CHARS[EnemyTargetPicker].CharacterDefense;

                Debug.Log($"Slime Does {Tempdamage}");
                Debug.Log(TM.CHARS[EnemyTargetPicker].CharacterDefense);
                Debug.Log(TM.CHARS[EnemyTargetPicker].CharacterName);

                if (Tempdamage < 0)
                {
                    Tempdamage = 0;
                }

                TM.CHARS[EnemyTargetPicker].CharacterHP -= Tempdamage;
                CUIM.COMBATTEXTINFO.text = $"{BattleOrder[0].CharacterName} does {Tempdamage} damage to {TM.CHARS[EnemyTargetPicker].CharacterName}!";
            }
        }

        CUIM.PLAYERBUTTONS.SetActive(true);
        UpdateBattleOrder();
        DelayTimerActive = false;

    }
}
