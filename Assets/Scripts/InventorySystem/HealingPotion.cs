using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Create Potion")]
public class HealingPotion : Item
{
    public int HealingAmount;

    private void Awake()
    {
        HealingAmount = EffectStrength;
    }

}
