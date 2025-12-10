using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [SerializeField] public List<Allies> CHARS = new();
    [SerializeField] public List<Enemies> ENEMIES = new();

    public BattleManager BM;

    void Start()
    {
        SetTeams();
    }

    public void SetTeams()
    {
        for (int i = 0; i < CurrentTeam.TeamCharacters.Count; i++)
        {
            CHARS.Add(CurrentTeam.TeamCharacters[i]);
        }

        for (int i = 0; i < ENEMIES.Count; i++)
        {
            ENEMIES[i] = Instantiate(ENEMIES[i]);
        }

        BM.InitializeStart();
    }
}
