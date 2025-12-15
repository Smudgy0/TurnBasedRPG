using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] ScenesToLoad sceneToLoad;
    [SerializeField] Vector2 positionOveride;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StartCoroutine(SmallDelay());
        }
    }

    IEnumerator SmallDelay()
    {
        PlayerMovement.sceneStartPos = positionOveride;
        yield return null;
        SceneManager.LoadScene((int)sceneToLoad);
    }
}
