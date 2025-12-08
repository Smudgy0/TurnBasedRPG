using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [SerializeField] public List<Allies> CHARS = new();
    [SerializeField] public List<Enemies> ENEMIES = new();

    void Awake()
    {
        for (int i = 0; i < CHARS.Count; i++)
        {
            CHARS[i] = Instantiate(CHARS[i]);
        }

        for (int i = 0; i < ENEMIES.Count; i++)
        {
            ENEMIES[i] = Instantiate(ENEMIES[i]);
        }
    }
}
