using UnityEngine;
using UnityEngine.InputSystem;

public class QuestMenuController : MonoBehaviour
{
    public GameObject MenuCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MenuCanvas.SetActive(false);
    }

    public void QuestMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MenuCanvas.SetActive(!MenuCanvas.activeSelf);
        }
    }
}
