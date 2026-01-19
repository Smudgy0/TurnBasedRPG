using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotManager : MonoBehaviour
{
    public TextMeshProUGUI ItemText;

    public string ItemDesc;

    public int ItemAmount;

    public int MyIndexNum;

    private CombatUIManager CUIM;

    private BattleManager BM;

    private void Awake()
    {
        CUIM = FindAnyObjectByType<CombatUIManager>();
        BM = FindAnyObjectByType<BattleManager>();
    }

    public void HighlightSlot()
    {
        CUIM.HighLightItem(gameObject);
    }

    public void SwapItemCharView()
    {
        BM.HideNShowItemsToPickChar(MyIndexNum);
    }
}
