using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void TriggerFight()
    {
        if(WorldCharacterManager.TeamCharacters.Count > 0)
        {
            SceneManager.LoadScene(6);
        }
    }
}
