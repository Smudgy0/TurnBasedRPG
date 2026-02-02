using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventorySystemUI : MonoBehaviour
{
    [SerializeField] GameObject inventoryMenu;

    [SerializeField] Transform inventoryParent;
    [SerializeField] InventorySlot inventorySlot;

    [SerializeField] List<InventorySlot> SpawnedInventorySlots;

    [SerializeField] TMP_Text SelectedItemName;
    [SerializeField] TMP_Text SelectedItemDescription;
    [SerializeField] Image SelectedItemIcon;

    private void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("inventory");
        print(objs.Length);

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void OpenInventory()
    {
        ResetInventoryUI();
    }

    public void SetSelectedItemUI(string _SelectedItemName,string _SelectedItemDescription, Sprite ItemIcon)
    {
        SelectedItemName.SetText(_SelectedItemName);
        SelectedItemDescription.SetText(_SelectedItemDescription);
        SelectedItemIcon.gameObject.SetActive(!InventorySystem.Instance.CheckIfSlotIsEmpty(InventorySystem.Instance.SelectedItemIndex));
        SelectedItemIcon.sprite = ItemIcon;
    }

    public void ResetInventoryUI()
    {
        foreach(var slot in SpawnedInventorySlots)
        {
            Destroy(slot.gameObject);
        }
        SpawnedInventorySlots.Clear();

        for (int i = 0; i < InventorySystem.Instance.GetInventory().Length; i++)
        {
            if (InventorySystem.Instance.GetInventory()[i].itemType == ItemType.None) continue;
            SpawnedInventorySlots.Add(Instantiate(inventorySlot, inventoryParent));
        }

        for (int i = 0; i < SpawnedInventorySlots.Count; i++)
        {
            SpawnedInventorySlots[i].UpdateUI(InventorySystem.Instance.GetInventory()[i].ItemSprite,
                InventorySystem.Instance.GetItemQuantity()[i]);
        }
    }

    public void ResetItemQuantityUI(int SlotIndex)
    {
        SpawnedInventorySlots[SlotIndex].UpdateUI(InventorySystem.Instance.GetItemQuantity()[SlotIndex]);
    }
}
