using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
