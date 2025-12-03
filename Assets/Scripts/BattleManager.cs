using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.TextCore.Text;

public class BattleManager : MonoBehaviour
{
    public TeamManager TM;
    public Image[] TURNPANELIMAGES;

    public Characters[] BattleOrder;

    public int AlliedHighestValue = 0;

    public int SortingNumber = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BattleOrder[0] = TM.CHARS[0];
        BattleOrder[1] = TM.CHARS[1];
        BattleOrder[2] = TM.CHARS[2];

        BattleOrder[3] = TM.ENEMIES[0];
        BattleOrder[4] = TM.ENEMIES[1];
        BattleOrder[5] = TM.ENEMIES[2];

        while (SortingNumber != 6)
        {
            SortSideOrders();
        }

    }

    void SortSideOrders()
    {
        // Start of ordering

        if (BattleOrder[0].CharacterSpeed < BattleOrder[1].CharacterSpeed)
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

        SortingNumber++;
    }

    void SwapArray(int ArrayVal1, int ArrayVal2, Characters SavedChar)
    {
        BattleOrder[ArrayVal1] = BattleOrder[ArrayVal2];
        BattleOrder[ArrayVal2] = SavedChar; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
