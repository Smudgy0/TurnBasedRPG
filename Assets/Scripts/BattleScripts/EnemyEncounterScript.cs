using System.Collections.Generic;
using UnityEngine;

public class EnemyEncounterScript : MonoBehaviour
{
    bool EnemyEcounterBool = false;
    public List<Enemies> EnemyLibary = new();

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.otherCollider.tag == "GrassLands")
        {
            EnemyEcounterBool = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.otherCollider.tag == "GrassLands")
        {
            EnemyEcounterBool = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyEcounterBool == true) 
        { 
            int RandomEncounterNum = Random.Range(0, 100);
            int EnemySlot1 = Random.Range(0, EnemyLibary.Count);
            int EnemySlot2 = Random.Range(0, EnemyLibary.Count);
            int EnemySlot3 = Random.Range(0, EnemyLibary.Count);

            if (RandomEncounterNum > 90)
            {

            }
        }
    }
}
