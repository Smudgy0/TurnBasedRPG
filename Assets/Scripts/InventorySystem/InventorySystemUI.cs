using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventorySystemUI : MonoBehaviour
{
    [SerializeField] GameObject inventoryMenu;

    [SerializeField] Transform inventoryParent;
    [SerializeField] InventorySlot inventorySlot;

    [SerializeField] List<InventorySlot> SpawnedInventorySlots;

    [SerializeField] TMP_Text SelectedItemName;
    [SerializeField] TMP_Text SelectedItemDescription;

    private void Start()
    {
        InvokeRepeating("TestUI", 5, 5);
    }

    void TestUI()
    {
        ResetInventoryUI();
        inventoryMenu.SetActive(!inventoryMenu.activeSelf);
    }

    public void OnOpenInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ResetInventoryUI();
            inventoryMenu.SetActive(!inventoryMenu.activeSelf);
        }
    }

    public void SetSelectedItemUI(string _SelectedItemName,string _SelectedItemDescription)
    {
        SelectedItemName.SetText(_SelectedItemName);
        SelectedItemDescription.SetText(_SelectedItemDescription);
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
