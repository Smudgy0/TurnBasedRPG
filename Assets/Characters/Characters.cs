using UnityEngine;
using UnityEngine.UI;


public class Characters : ScriptableObject
{
    public string CharacterName;

    public int CharacterHP;
    public int CharacterMAXHP;
    public int CharacterAttack;
    public int CharacterDefense;
    public int CharacterSpeed;
    public Sprite CharacterSprite;
    public bool Allied;
    public enum StatusType
    {
        None,
        Stunned,
        Poisoned,
    }
    public StatusType Status;
}
