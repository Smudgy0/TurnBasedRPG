using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using TMPro.EditorUtilities;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    public TeamManager TM;
    public UIManager UIM;
    public GameObject[] CHOOSETARGETENEMY;
    public TMP_Text[] CHOOSETARGETENEMYTEXT;

    public Characters[] BattleOrder;
    public int SortingNumber = 0;

    private Button slimeButton;
    private bool MenuActive = true;
    private bool starting = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (!starting)
        {
            return;
        }

        Debug.Log("Awake");
        BattleOrder[0] = TM.CHARS[0];
        BattleOrder[1] = TM.CHARS[1];
        BattleOrder[2] = TM.CHARS[2];

        BattleOrder[3] = TM.ENEMIES[0];
        BattleOrder[4] = TM.ENEMIES[1];
        BattleOrder[5] = TM.ENEMIES[2];

        /*
        while (SortingNumber != 6)
        {
            SortSideOrders();
        }
        */

        SortSideOrders();

        UIM.Initialize();
        InitializeTargetOptions();

        starting = false;
    }

    public void TargetNumber(int TargetNum)
    {
        Debug.Log("TargetNumber");
        SceneManager.LoadScene(TargetNum);
        TM.ENEMIES[TargetNum].CharacterHP = TM.ENEMIES[TargetNum].CharacterHP - BattleOrder[0].CharacterAttack;
        ChangeOrder(BattleOrder[0], BattleOrder[1], BattleOrder[2], BattleOrder[3], BattleOrder[4], BattleOrder[5]);
    }

    void ChangeOrder(Characters SLOT1, Characters SLOT2, Characters SLOT3, Characters SLOT4, Characters SLOT5, Characters SLOT6)
    {
        Debug.Log("ChangeOrder");
        BattleOrder[0] = SLOT2;
        BattleOrder[1] = SLOT3;
        BattleOrder[2] = SLOT4;
        BattleOrder[3] = SLOT5;
        BattleOrder[4] = SLOT6;
        BattleOrder[5] = SLOT1;

        Debug.Log(BattleOrder[0]);
        Debug.Log(BattleOrder[1]);
        Debug.Log(BattleOrder[2]);
        Debug.Log(BattleOrder[3]);
        Debug.Log(BattleOrder[4]);
        Debug.Log(BattleOrder[5]);

        CHECKTURNUI();
    }

    void CHECKTURNUI()
    {
        Debug.Log("CHECKTURNUI");
        UIM.TURNSYSTEMSPRITES[0].sprite = BattleOrder[0].CharacterSprite;
        UIM.TURNSYSTEMSPRITES[1].sprite = BattleOrder[1].CharacterSprite;
        UIM.TURNSYSTEMSPRITES[2].sprite = BattleOrder[2].CharacterSprite;
        UIM.TURNSYSTEMSPRITES[3].sprite = BattleOrder[3].CharacterSprite;
        UIM.TURNSYSTEMSPRITES[4].sprite = BattleOrder[4].CharacterSprite;
        UIM.TURNSYSTEMSPRITES[5].sprite = BattleOrder[5].CharacterSprite;
    }

    public void ShowTargets()
    {
        Debug.Log("ShowTargets");
        CHOOSETARGETENEMY[0].SetActive(true);
        CHOOSETARGETENEMY[1].SetActive(true);
        CHOOSETARGETENEMY[2].SetActive(true);
        CHOOSETARGETENEMY[3].SetActive(true);

        UIM.PLAYERBUTTONS.SetActive(false);

        MenuActive = false;
    }

    public void HideTargets()
    {
        Debug.Log("HideTargets");
        CHOOSETARGETENEMY[0].SetActive(false);
        CHOOSETARGETENEMY[1].SetActive(false);
        CHOOSETARGETENEMY[2].SetActive(false);
        CHOOSETARGETENEMY[3].SetActive(false);

        UIM.PLAYERBUTTONS.SetActive(true);

        MenuActive = true;
    }

    void InitializeTargetOptions()
    {
        Debug.Log("InitializeTargetOptions");
        if (TM.ENEMIES[0] != null)
        {
            CHOOSETARGETENEMYTEXT[0].text = TM.ENEMIES[0].CharacterName.ToString();
        }
        if (TM.ENEMIES[1] != null)
        {
            CHOOSETARGETENEMYTEXT[1].text = TM.ENEMIES[1].CharacterName.ToString();
        }
        if (TM.ENEMIES[2] != null)
        {
            CHOOSETARGETENEMYTEXT[2].text = TM.ENEMIES[2].CharacterName.ToString();
        }
    }

    void SortSideOrders()
    {
        // Start of ordering
        Debug.Log("Sorting");
        var sorted = BattleOrder.OrderByDescending(item => item.CharacterSpeed);
        BattleOrder = sorted.ToArray();

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
        Debug.Log("Swapping");
        BattleOrder[ArrayVal1] = BattleOrder[ArrayVal2];
        BattleOrder[ArrayVal2] = SavedChar; 
    }

    // Update is called once per frame
    void Update()
    {

        if (BattleOrder[0].Allied == true && MenuActive == true)
        {
            UIM.PLAYERBUTTONS.SetActive(true);
        }
        else
        {
            UIM.PLAYERBUTTONS.SetActive(false);
        }
        
    }
}
