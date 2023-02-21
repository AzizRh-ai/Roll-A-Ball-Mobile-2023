using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button[] levelButton;
    [SerializeField] private Sprite lockedSprite;
    [SerializeField] private Sprite unlockedSprite;


    private void Start()
    {
        int unlockedLevel = PlayerPrefs.GetInt("lastLevel", 1);

        for (int i = 0; i < levelButton.Length; i++)
        {

            bool levelUnlocked = (i < unlockedLevel);
            // si lvl débloquer on affiche le sprite sinon cadenas
            levelButton[i].image.sprite = levelUnlocked ? unlockedSprite : lockedSprite;

            //levelButton[i].image.SetNativeSize();
            levelButton[i].image.preserveAspect = true;

            //on active suivant lastlevel
            levelButton[i].interactable = levelUnlocked;

            int levelIndex = i + 1;

            levelButton[i].GetComponent<Button>().onClick.AddListener(() => LoadLvlScene(levelIndex));
        }
    }

    private void LoadLvlScene(int levelIndex)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(levelIndex);

    }

}
