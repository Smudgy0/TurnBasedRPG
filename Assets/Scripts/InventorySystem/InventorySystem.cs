using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;
    [SerializeField] Item testItem;
    [SerializeField] Item[] Inventory;
    [SerializeField] int[] AmountCarried;

    [SerializeField] Item EmptyItem;

    public int SelectedItemIndex;

    InventorySystemUI inventorySystemUI;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        inventorySystemUI = GetComponent<InventorySystemUI>();
    }

    private void Start()
    {
        AddItem(testItem, 5);
    }

    public Item[] GetInventory() => Inventory;
    public int[] GetItemQuantity() => AmountCarried;
    public bool CheckIfSlotIsEmpty(int slotIndex) => Inventory[slotIndex] == EmptyItem;

    public void SelectItem(int SelectedIndex)
    {
        SelectedItemIndex = SelectedIndex;
        inventorySystemUI.SetSelectedItemUI(Inventory[SelectedIndex].ItemName, Inventory[SelectedIndex].ItemDescription);
    }

    public bool AddItem(Item ItemToAdd, int AmountToAdd)
    {
        for (int i = 0; i < Inventory.Length; i++) 
        {
            if (Inventory[i] == ItemToAdd)
            {
                AmountCarried[i] = Mathf.Clamp(AmountCarried[i] + AmountToAdd, 0, 99);
                return true;
            }
            if (Inventory[i].itemType == ItemType.None)
            {
                Inventory[i] = ItemToAdd;
                AmountCarried[i] = Mathf.Clamp(AmountCarried[i] + AmountToAdd, 0, 99);
                return true;
            }
        }
        return false;
    }

    public void RemoveItem(int ItemIndex, int AmountToRemove)
    {
        if (AmountCarried[ItemIndex] - AmountToRemove > 0)
        {
            AmountCarried[ItemIndex]-= AmountToRemove;
        }
        else
        {
            Inventory[ItemIndex] = EmptyItem;
            AmountCarried[ItemIndex] = 0;
        }
    }
}
