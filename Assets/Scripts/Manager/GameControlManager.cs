using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControlManager : MonoBehaviour
{

    public static GameControlManager instance;

    [Header("Timer UI")]
    [SerializeField] private TextMeshProUGUI TimerUi;
    [SerializeField] private float timer = 30f;

    [Header("GameOver UI")]
    [SerializeField] private GameObject GameOverPanel;


    [Header("Win UI")]
    [SerializeField] private GameObject WinPanel;

    [SerializeField] private AudioClip[] _audioClips;
    private AudioSource audioSource;
    // Check/Set only 1 instance
    void Awake()
    {
        instance = this;
    }

    //Set GameOverPanel on Start
    private void Start()
    {
        GameOverPanel.SetActive(false);
        audioSource = GetComponent<AudioSource>();
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
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {

            SceneManager.LoadScene(lvlIndex);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
        Time.timeScale = 1;

        yield return null;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
        audioSource.PlayOneShot(_audioClips[1]);
        TimerUi.text = "0";
        GameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void LoadWinMenu()
    {
        audioSource.PlayOneShot(_audioClips[0]);
        TimerUi.text = "";
        WinPanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("Score");
    }
}
