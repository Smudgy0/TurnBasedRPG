using UnityEngine;
using UnityEngine.UI;


public class Characters : ScriptableObject
{
    public string CharacterName;

    public float CharacterHP;
    public float CharacterMAXHP;
    public float CharacterAttack;
    public float CharacterDefense;
    public float CharacterSpeed;
    public Sprite CharacterSprite;
    public Sprite CharacterBattleSprite;
    public bool Allied;
    public bool Dead = false;
    public enum StatusType
    {
        None,
        Stunned,
        Poisoned,
    }
    public StatusType Status;
    public bool Defending;

    public void Defend()
    {
        Defending = true;
    }

    public void DisableDefence()
    {
        Defending = false;
    }
}
