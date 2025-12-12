using UnityEngine;
using System.Collections.Generic;

public class CurrentTeam : MonoBehaviour
{
    //public static List<Allies> AllCharacters = new();
    public static List<Allies> TeamCharacters = new();
    public List<Allies> DebugTeamCharacters = new();

    void Update()
    {
        DebugTeamCharacters = TeamCharacters;
    }

}
