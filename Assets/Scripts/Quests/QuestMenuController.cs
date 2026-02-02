using UnityEngine;
using UnityEngine.InputSystem;

public class QuestMenuController : MonoBehaviour
{
    public GameObject MenuCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //MenuCanvas.SetActive(false);
    }

    public void ToggleQuestMenu()
    {
        //MenuCanvas.SetActive(!MenuCanvas.activeSelf);
    }
}
