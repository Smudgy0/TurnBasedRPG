using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldCharacterManager : MonoBehaviour
{
    public static WorldCharacterManager Instance;

    public List<Allies> CharacterDataLog = new(); // inspector assignable characters list

    public static List<Allies> AllCharacters = new(); // static list of character clones
    public List<Allies> DebugAllCharacters = new();

    public static List<Allies> TeamCharacters = new(); // static list of team character clones
    public List<Allies> DebugTeamCharacters = new();

    public static List<Allies> UnusedTeamCharacters = new(); // static list of team character clones
    public List<Allies> DebugUTeamCharacters = new();


    public TeamManagerUIScript TMUI;

    static bool IReadNewGame = false;

    void Awake()
    {
        Instance = this;

        if (IReadNewGame == false)
        {
            for (int i = 0; i < CharacterDataLog.Count; i++)
            {
                AllCharacters.Add(Instantiate(CharacterDataLog[i]));
            }
            UnusedTeamCharacters = AllCharacters;
            TeamCharacters.Add(UnusedTeamCharacters[0]);
            TeamCharacters.Add(UnusedTeamCharacters[1]);
            TeamCharacters.Add(UnusedTeamCharacters[2]);
            UnusedTeamCharacters.RemoveAt(0);
            UnusedTeamCharacters.RemoveAt(1);
            UnusedTeamCharacters.RemoveAt(2);

            for (int i = 0; i < AllCharacters.Count; i++)
            {
                DebugAllCharacters.Add(AllCharacters[i]);
            }
            //FoundTeam();
        }

        IReadNewGame = true;
    }

    private void Update()
    {
        DebugTeamCharacters = TeamCharacters;
        DebugUTeamCharacters = UnusedTeamCharacters;
    }

    void FoundTeam()
    {
        TeamCharacters.Add(AllCharacters[0]);
    }

    // this function is ran to add a member to the team
    public void AddToTeam(int ArrayVal) 
    {
        if(TeamCharacters.Count < 3)
        {
            if (TeamCharacters.Contains(AllCharacters[ArrayVal])) // makes sure the character isn't already on the team
            {
                Debug.Log("Already have team member on my team!");
                return;
            }
            Debug.Log("Added to team");
            TeamCharacters.Add(UnusedTeamCharacters[ArrayVal]);
            UnusedTeamCharacters.RemoveAt(ArrayVal);
            TMUI.ReCheckCurrentTeam(); // reloads the current team UI
            TMUI.ReCheckPossibleTeam(); // reloads the buttons on the character selection
        }
        else {return;}
    }

    public void RemoveFromTeam(int ArrayVal) // removes the selected character from the team
    {
        UnusedTeamCharacters.Add(TeamCharacters[ArrayVal]);
        TeamCharacters.RemoveAt(ArrayVal);
        TMUI.ReCheckPossibleTeam(); // reloads the buttons on the character selection
        TMUI.ReCheckCurrentTeam(); // reloads the current team UI
    }
}
