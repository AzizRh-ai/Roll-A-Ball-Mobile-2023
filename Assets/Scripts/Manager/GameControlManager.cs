using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControlManager : MonoBehaviour
{
    public static GameControlManager instance;
    [SerializeField] private GameObject GameOverPanel;
    [Header("Timer UI")]
    [SerializeField] private TextMeshProUGUI TimerUi;
    private float timer = 30f;

    // Check/Set only 1 instance
    void Awake()
    {
        instance = this;
    }

    //Set GameOverPanel on Start
    private void Start()
    {
        GameOverPanel.SetActive(false);
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        TimerUi.text = timer.ToString();
        if (timer <= 0)
        {
            GameOver();
        }
        else
        {
            int timeSecond = (int)timer;
            TimerUi.text = timeSecond.ToString();
        }
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
        TimerUi.text = "0";
        GameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("Score");
    }
}
