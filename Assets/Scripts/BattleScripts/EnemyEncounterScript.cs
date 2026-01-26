using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyEncounterScript : MonoBehaviour
{
    public bool EnemyEcounterBool = false;
    static bool EditorConverted = false;

    public List<Enemies> UnityEditorEnemyLibary = new();
    static List<Enemies> EnemyLibary = new();
    static public List<Enemies> EESEnemyTeam = new();

    public BattleTrigger BT;
    public int RandomEncounterNum = 0;

    int EnemySlot1;
    int EnemySlot2;
    int EnemySlot3;

    private void Start()
    {
        foreach (Enemies EESEnemyTeam in EESEnemyTeam)
        {
            Destroy(EESEnemyTeam);
        }
        EESEnemyTeam.Clear();
        EditorConverted = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "GrassLands")
        {
            EnemyEcounterBool = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "GrassLands")
        {
            EnemyEcounterBool = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyEcounterBool == true) 
        { 
            RandomEncounterNum = Random.Range(0, 1000000);
            EnemySlot1 = Random.Range(0, EnemyLibary.Count);
            EnemySlot2 = Random.Range(0, EnemyLibary.Count);
            EnemySlot3 = Random.Range(0, EnemyLibary.Count);

            Debug.Log("Read RandomEncounterNum and EnemySelect");

            if (RandomEncounterNum > 999000)
            {
                Debug.Log("Read StartFightFunction Trigger");
                StartFight();
            }
        }
    }

    void StartFight()
    {
        Debug.Log("Read StartFightFunction");
        EESEnemyTeam.Add(Instantiate(UnityEditorEnemyLibary[EnemySlot1]));
        EESEnemyTeam.Add(Instantiate(UnityEditorEnemyLibary[EnemySlot2]));
        EESEnemyTeam.Add(Instantiate(UnityEditorEnemyLibary[EnemySlot3]));

        BT.TriggerFight();
    }
}
