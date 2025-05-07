using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public string sceneName;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) // Press 'L' to load the scene
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}