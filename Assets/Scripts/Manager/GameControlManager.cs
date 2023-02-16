using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControlManager : MonoBehaviour
{
    public static GameControlManager instance;
    [SerializeField] private GameObject GameOverPanel;

    // Check/Set only 1 instance
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

    //Set GameOverPanel on Start
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

    public void RestartBtn()
    {
        Time.timeScale = 1;
        StartCoroutine(manageScene(SceneManager.GetActiveScene().buildIndex));
    }
    // Quit
    public void QuitGameBtn()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        GameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("Score");
    }
}
