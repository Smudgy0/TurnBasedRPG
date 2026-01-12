using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] Image ItemIcon;
    [SerializeField] TMP_Text ItemQuantity;
    
    public void SelectItem()
    {
        InventorySystem.Instance.SelectItem(transform.GetSiblingIndex());
    }

    public void UpdateUI(Sprite _ItemIcon, int _ItemQuantity)
    {
        ItemIcon.sprite = _ItemIcon;
        ItemQuantity.SetText($"x{_ItemQuantity}"); 
    }
    
    public void UpdateUI(int _ItemQuantity)
    {
        ItemQuantity.SetText($"x{_ItemQuantity}"); 
    }
}
