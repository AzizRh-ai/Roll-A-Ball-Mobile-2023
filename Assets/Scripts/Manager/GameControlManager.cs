using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControlManager : MonoBehaviour
{
    public static GameControlManager instance;
    [SerializeField] private GameObject GameOverPanel;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    //Set GameOverPanel Hidden
    private void Start()
    {
        GameOverPanel.SetActive(false);
    }

    // Load/Reload Scene
    public IEnumerator manageScene(int lvlIndex)
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {

            SceneManager.LoadScene(lvlIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
        yield return null;
    }

    // Quit
    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("Score");
    }
}
