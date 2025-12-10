using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

public class WorldCharacterManager : MonoBehaviour
{
    public List<Allies> CharacterDataLog = new();
    public static List<Allies> AllCharacters = new();
    public List<Allies> DebugAllCharacters = new();
    //public List<Characters> TeamCharacters = new();
    public TeamManagerUIScript TMUI;
    private bool CanAdd = true;
    private static bool CharsLoaded = false;

    void Awake()
    {
        if(CharsLoaded == false)
        {
            for(int i = 0; i < CharacterDataLog.Count; i++)
            {
                AllCharacters.Add(Instantiate(CharacterDataLog[i]));
            }

            FoundTeam();
            CharsLoaded = true;
        }

        for(int i = 0; i < AllCharacters.Count; i++)
        {
            DebugAllCharacters.Add(AllCharacters[i]);
        }
    }

    void FoundTeam()
    {
        CurrentTeam.TeamCharacters.Add(AllCharacters[0]);
    }

    public void AddToTeam(int ArrayVal)
    {
        CanAdd = true;
        if(CurrentTeam.TeamCharacters.Count < 3)
        {
            for(int i = 0 ; i < CurrentTeam.TeamCharacters.Count; i++)
            {
                if(AllCharacters[ArrayVal].CharacterName == CurrentTeam.TeamCharacters[i].CharacterName)
                {
                    CanAdd = false;
                }
            }

            if(CanAdd)
            {
                CurrentTeam.TeamCharacters.Add(AllCharacters[ArrayVal]);
                TMUI.LoadCharacterList();
            }
            else {return;}
        }
        else {return;}
    }

    public void RemoveFromTeam(int ArrayVal)
    {
        TMUI.RemoveUI(ArrayVal);
        CurrentTeam.TeamCharacters.Remove(CurrentTeam.TeamCharacters[ArrayVal]);
        TMUI.LoadCharacterList();
    }
}
