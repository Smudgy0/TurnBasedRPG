using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameMenus;

    private void Awake()
    {
        gameMenus = FindFirstObjectByType<MenuController>().gameObject;

        if (FindObjectsByType<MenuController>(FindObjectsSortMode.InstanceID).Length <= 1) 
        {
            Debug.Log("DOESN'T HAVE A STORED GAMEMENUS");
            DontDestroyOnLoad(gameMenus);
        }
        else
        {
            Destroy(FindObjectsByType<MenuController>(FindObjectsSortMode.InstanceID)[1].gameObject);
            Debug.Log("HAS A DUPLICATE STORED GAMEMENU");
        }
    }
}
