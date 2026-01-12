using UnityEngine;

public class QuestMenuController : MonoBehaviour
{
    public GameObject MenuCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MenuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            MenuCanvas.SetActive(!MenuCanvas.activeSelf);
        }
    }
}
