using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Create Item")]
public class Item : ScriptableObject
{
    public string ItemName;
    public string ItemDescription;

    public ItemType itemType;

}
    public enum ItemType
    {
        None,
        Consumables,
        Equipment,
        KeyItem,
        Misc
    }
