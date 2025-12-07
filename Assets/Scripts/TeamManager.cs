using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [SerializeField] public Allies[] CHARS;
    [SerializeField] public Enemies[] ENEMIES;

    void Awake()
    {
        for (int i = 0; i < CHARS.Length; i++)
        {
            CHARS[i] = Instantiate(CHARS[i]);
        }

        for (int i = 0; i < ENEMIES.Length; i++)
        {
            ENEMIES[i] = Instantiate(ENEMIES[i]);
        }
    }
}
