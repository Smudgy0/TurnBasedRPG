using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [SerializeField] public List<Allies> CHARS = new();
    [SerializeField] public List<Enemies> ENEMIES = new();

    public EnemyEncounterScript EES;

    public BattleManager BM;

    void Start()
    {
        SetTeams();
    }

    public void SetTeams()
    {
        for (int i = 0; i < WorldCharacterManager.TeamCharacters.Count; i++)
        {
            CHARS.Add(WorldCharacterManager.TeamCharacters[i]);
        }
        /*
        for (int i = 0; i < ENEMIES.Count; i++)
        {
            ENEMIES[i] = Instantiate(ENEMIES[i]);
        }
        */
        ENEMIES = EnemyEncounterScript.EESEnemyTeam;

        BM.InitializeStart();
    }
}
