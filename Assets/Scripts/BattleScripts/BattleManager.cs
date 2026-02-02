using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public TeamManager TM;
    public CombatUIManager CUIM;
    private int ItemToBeUsed;

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

    public int AlliedDeaths;
    public int EnemyDeaths;

    private int XPReward = 250;

    public int TempArrayMarker;

    public GameObject BATTLEENDBUTTON;
    public TMP_Text BATTLEENDTEXT;

    private bool BattleEnded = false;

    // Initializes the battle order to see who goes first and it also resets values to ensure that the player can do multiple battles
    public void InitializeStart()
    {
        BattleEnded = false;

        BATTLEENDBUTTON.SetActive(false);
        AlliedDeaths = 0;
        EnemyDeaths = 0;

        if (!starting)
        {
            return;
        }

        Debug.Log("Awake");

        for (int i = 0; i < WorldCharacterManager.TeamCharacters.Count; i++)
        {
            TM.CHARS[i].Dead = false;
            BattleOrder.Add(TM.CHARS[i]);
        }

        for (int i = 0; i < TM.ENEMIES.Count; i++)
        {
            TM.ENEMIES[i].Dead = false;
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

    // deals damage to the selected enemy
    public void TargetNumber(int TargetNum)
    {
        HideTargets();
        ShowTargets();
        BattleOrder[0].DisableDefence();

        float TempDamage = 0;

        // checks if enemy is defending or not to add the defence stat to the calculation
        if (TM.ENEMIES[TargetNum].Defending == false)
        {
            // calculation is run as normal as target is not defending
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
            // checks what the new damage is when character attack is reduced by the targeted characters defense
            TempDamage = BattleOrder[0].CharacterAttack - TM.ENEMIES[TargetNum].CharacterDefense;

            if (TempDamage < 0)
            {
                TempDamage = 0;
            }

            TM.ENEMIES[TargetNum].CharacterHP = TM.ENEMIES[TargetNum].CharacterHP - TempDamage;
            CUIM.COMBATTEXTINFO.text = $"{BattleOrder[0].CharacterName} does {TempDamage} damage to {TM.ENEMIES[TargetNum].CharacterName}!";
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
                        EnemyDeaths++;
                        CharacterDeath(i, j, charactersInBattle[i]);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        UpdateBattleOrder();
    }

    // when chosen the character will have their defend value set to true, reduced damage from enemies
    public void DefendButton()
    {
        BattleOrder[0].Defend();
        CUIM.COMBATTEXTINFO.text = $"{BattleOrder[0].CharacterName} Defends!";
        UpdateBattleOrder();
    }

    // gets the value from the consumable to run the HealChar function
    public void HealCharButton(int CharChosen)
    {
        HealChar(InventorySystem.Instance.GetInventory()[ItemToBeUsed].EffectStrength, CharChosen);
        //Debug.Log(WorldCharacterManager.TeamCharacters[CharChosen].ToString());
        InventorySystem.Instance.RemoveItem(ItemToBeUsed, 1);
        CUIM.ExitHighLightItem();
        ItemToBeUsed = 0;
    }

    // uses a consumable to heal the character chosen
    public void HealChar(int HealAmount, int CharChosen) // uses item to heal character
    {
        TM.CHARS[CharChosen].CharacterHP += HealAmount;

        if (TM.CHARS[CharChosen].CharacterHP > TM.CHARS[CharChosen].CharacterMAXHP)
        {
            TM.CHARS[CharChosen].CharacterHP = TM.CHARS[CharChosen].CharacterMAXHP;
        }

        CUIM.COMBATTEXTINFO.text = $"{TM.CHARS[CharChosen].CharacterName} Heals for {HealAmount}!";
        CUIM.PLAYERITEMBUTTONS.SetActive(false);
        CUIM.PickCharItemButtons.SetActive(false);
        CUIM.BackGroundStuff[0].SetActive(true);
        CUIM.BackGroundStuff[1].SetActive(true);
        CUIM.PLAYERBUTTONS.SetActive(true);

        UpdateBattleOrder();
    }

    // hides consumables when player picks which one to use to let them select a character or if they go back from character selection,
    // it goes back to pick a different item.
    public void HideNShowItemsToPickChar(int itemIndex)
    {
        CUIM.PLAYERITEMBUTTONS.SetActive(!CUIM.PLAYERITEMBUTTONS.activeSelf);
        CUIM.PickCharItemButtons.SetActive(!CUIM.PickCharItemButtons.activeSelf);
        if (itemIndex < 0) return;
        ItemToBeUsed = itemIndex;
    }

    // runs a random number to see if the player flees the battle
    public void Flee()
    {
        BattleOrder[0].DisableDefence();
        int RNDChanceToFlee = 0;
        RNDChanceToFlee = UnityEngine.Random.Range(0, 101);

        if(RNDChanceToFlee > 90)
        {
            CUIM.COMBATTEXTINFO.text = "The Team Successfully Flees!";
            BATTLEENDBUTTON.SetActive(true);

            // add function to return to game world!
        }
        else
        {
            CUIM.COMBATTEXTINFO.text = "The Team Fails To Flee!";
            UpdateBattleOrder();
        }
    }

    // sets the character or enemy in the front of the list to the back when an action is performed
    void UpdateBattleOrder()
    {
        Characters tempChar = BattleOrder[0];
        BattleOrder.RemoveAt(0);
        BattleOrder.Add(tempChar);
    }

    // re applies sprites when a ally or enemy peformes a action and the game moves onto the next turn
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

    // shows the enemy targets and hides the player action buttons
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

    // hides the enemy targets and goes back to the player action buttons
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

    // tells the UI manager to show the items and hide the attack, defend etc buttons
    public void ShowItems()
    {
        for (int i = 0; i < CUIM.TripleItemButtons.Length; i++)
        {
            CUIM.TripleItemButtons[i].SetActive(false);
        }

        for (int i = 0; i < TM.CHARS.Count; i++)
        {
            CUIM.TripleItemButtons[i].SetActive(true);
        }

        CUIM.InitializeItemButtons();
        CUIM.PLAYERITEMBUTTONS.SetActive(true);
        CUIM.PLAYERBUTTONS.SetActive(false);

        for (int i = 0; i < CUIM.BackGroundStuff.Length; i++)
        {
            CUIM.BackGroundStuff[i].SetActive(false);
        }
    }

    // tells the UI manager to hide the items and go back to the attack, defend etc buttons
    public void HideItems()
    {
        for (int i = 0; i < CUIM.TripleItemButtons.Length; i++)
        {
            CUIM.TripleItemButtons[i].SetActive(false);
        }

        Debug.Log("Read HideItemsFunction");
        CUIM.PLAYERITEMBUTTONS.SetActive(false);
        CUIM.PLAYERBUTTONS.SetActive(true);

        for (int i = 0; i < CUIM.BackGroundStuff.Length; i++)
        {
            CUIM.BackGroundStuff[i].SetActive(true);
        }
    }

    // checks which enemies are still alive for the player to target after clicking the attack button
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

    // sorts which ally or enemy goes first, second, third etc
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

        TeamConditionChecker();

        for (int i = 0; i < TM.ENEMIES.Count; i++)
        {
            if (TM.ENEMIES[i].CharacterHP < 0)
            {
                CHOOSETARGETENEMY[i].SetActive(false);
            }
        }
    }


    // this function checks if either side has lost all its team members and after the check it will see if the player has won or loss
    void TeamConditionChecker()
    {
        if (BattleEnded == false)
        { 
            if (AlliedDeaths >= TM.CHARS.Count)
            {
                BattleEnded = true;

                CUIM.EXPBoxGameObjectGroup.SetActive(true);
                CUIM.EXPBoxDeathText.SetActive(true);
                CUIM.EXPBoxVictoryGroup.SetActive(false);

                //BATTLEENDBUTTON.SetActive(true);
                //BATTLEENDTEXT.text = "You Lose!";

                EndOfBattleTimer();
            }

            if (EnemyDeaths >= TM.ENEMIES.Count)
            {
                BattleEnded = true;

                CUIM.EXPBoxGameObjectGroup.SetActive(true);
                CUIM.EXPBoxDeathText.SetActive(false);
                CUIM.EXPBoxVictoryGroup.SetActive(true);
                for (int i = 0; i < CUIM.EXPInfoBoxCharLevelUpText.Length; i++)
                {
                    CUIM.EXPInfoBoxCharLevelUpGameObject[i].SetActive(false);
                }

                string expReward = $"XP Gained: {XPReward * TM.ENEMIES.Count}";
                for (int i = 0; i < TM.CHARS.Count; i++)
                {
                    CUIM.EXPInfoBoxes[i].SetActive(true);
                    CUIM.EXPInfoBoxCharNameText[i].text = TM.CHARS[i].CharacterName;
                    CUIM.EXPInfoBoxCharEXPGainText[i].text = expReward;
                    CUIM.EXPInfoBoxCharLevelUpText[i].text = $"Leveled Up To Level {TM.CHARS[i].CharacterLevel}, Stats Have Increased by 20%!";
                    CUIM.EXPInfoBoxCharLevelUpGameObject[i].SetActive(true);

                    Rewards();
                    StartCoroutine(EndOfBattleTimer());
                }

                //BATTLEENDBUTTON.SetActive(true);
                //BATTLEENDTEXT.text = "You Win!";
            }
        }
    }

    public void RevertToMap() // button which appears after the player wins or loses a battle.
    {
        SceneManager.LoadScene(1);
    }

    // when the player wins a battle they will be given XP for their level up's based on the amount of enemies
    public void Rewards()
    {
        for (int i = 0; i < TM.CHARS.Count; i++)
        {
            TM.CHARS[i].CharacterEXP = TM.CHARS[i].CharacterEXP + (XPReward * TM.ENEMIES.Count);

            if(TM.CHARS[i].CharacterEXP >= TM.CHARS[i].CharacterEXPRequirement) // Charater LevelUp stuff
            {
                LevelUp(i);

                TM.CHARS[i].CharacterEXP = 0;
                TM.CHARS[i].CharacterEXPRequirement = TM.CHARS[i].CharacterEXPRequirement * 2;
            }
        }
    }

    // if the player has enough xp to level up their stats go up by 20%
    public void LevelUp(int CharArray)
    {
        TM.CHARS[CharArray].CharacterLevel++;

        TM.CHARS[CharArray].CharacterMAXHP = Mathf.Round(TM.CHARS[CharArray].CharacterMAXHP * 1.2f);
        TM.CHARS[CharArray].CharacterHP = TM.CHARS[CharArray].CharacterMAXHP;

        TM.CHARS[CharArray].CharacterAttack = Mathf.Round(TM.CHARS[CharArray].CharacterAttack * 1.2f);
        TM.CHARS[CharArray].CharacterDefense = Mathf.Round(TM.CHARS[CharArray].CharacterDefense * 1.2f);
        TM.CHARS[CharArray].CharacterSpeed = Mathf.Round(TM.CHARS[CharArray].CharacterSpeed * 1.2f);
    }

    // if a character dies they are removed from the combat system
    public void CharacterDeath(int whichSlot, int currentPositionInBattleOrder, Characters character)
    {
        if (!charactersInBattle[whichSlot].Allied)
        {
            Debug.Log("1 ENEMY DIES");
            QuestController.instance.TryProgressKillQuest(charactersInBattle[whichSlot].characterID);
        }
        RemoveFromBattleOrder(currentPositionInBattleOrder);
        CUIM.UIMUpdateCharacterSprites(whichSlot, character);
    }

    // once a character dies this function remove them from the battle order
    void RemoveFromBattleOrder(int whichSlot)
    {
        BattleOrder.RemoveAt(whichSlot);
    }

    // enemy decides what it wants to do, attack or defend
    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(2);

        EnemyActionPicker = UnityEngine.Random.Range(0,2);

        if (EnemyActionPicker == 1) // enemy defends
        {
            BattleOrder[0].Defending = true;
            CUIM.COMBATTEXTINFO.text = $"{BattleOrder[0].CharacterName} Defends!";
            Debug.Log("Enemy Defends");
            BattleOrder[0].Defend();
        }
        else
        {
            float Tempdamage = 0;

            BattleOrder[0].DisableDefence();
            BattleOrder[0].Defending = false;
            EnemyTargetPicker = Random.Range(0, WorldCharacterManager.TeamCharacters.Count);

            if (TM.CHARS[EnemyTargetPicker].Defending == false) // if player does not defend, their characters defence is not taken into account
            {
                Tempdamage = BattleOrder[0].CharacterAttack;

                if (Tempdamage < 0)
                {
                    Tempdamage = 0;
                }

                TM.CHARS[EnemyTargetPicker].CharacterHP = TM.CHARS[EnemyTargetPicker].CharacterHP - Tempdamage;
                CUIM.COMBATTEXTINFO.text = $"{BattleOrder[0].CharacterName} does {Tempdamage} damage to {TM.CHARS[EnemyTargetPicker].CharacterName}!";
            }
            else if (TM.CHARS[EnemyTargetPicker].Defending == true) // if player defends, their characters defence is taken into account
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

            // Kill a character if their health is 0 or lower
            if (TM.CHARS[EnemyTargetPicker].CharacterHP <= 0)
            {
                for (int i = 0; i < BattleOrder.Count; i++)
                {
                    if (BattleOrder[i] == TM.CHARS[EnemyTargetPicker]) 
                    {
                        AlliedDeaths++;
                        CharacterDeath(EnemyTargetPicker, i, TM.CHARS[EnemyTargetPicker]); 
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        CUIM.PLAYERBUTTONS.SetActive(true);
        UpdateBattleOrder();
        DelayTimerActive = false;

    }

    // runs a timer before the battle ends
    private IEnumerator EndOfBattleTimer()
    {
        yield return new WaitForSeconds(5);
        RevertToMap();
    }
}
