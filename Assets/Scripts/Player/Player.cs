using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour, IMovement
{
    //Event
    public delegate void OnScoreMessage(int value);
    public static event OnScoreMessage OnScoreUpdate;


    [Header("Ball")]
    private Rigidbody _rigidbody;

    [Header("Prefab")]
    [SerializeField] private GameObject _coinEffectPrefab;
    [SerializeField] private GameObject _keyEffectPrefab;

    [Header("Score Value")]
    private int ScoreValue = 0;

    // Score
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject LockPanelUi;

    // Score
    [Header("GameOver UI")]
    [SerializeField] private GameObject gameOver;


    [Header("Input")]
    [SerializeField] private InputBtn jumpButton;
    private float movementX;
    private float movementY;

    [Header("Player")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float speed = 5f;
    public bool isGrounded = true;

    [Header("Healt")]
    [SerializeField] private int healt = 5;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Image Keys;
    [SerializeField] private TextMeshProUGUI KeyUi;

    [Header("Animation")]
    [SerializeField] private Animation door;

    [Header("SpawnCoin")]
    [SerializeField] private WaveManager _waveManager;
    [SerializeField] private float playerDistanceSpawn = 5f;
    public float JumpForce { get { return jumpForce; } }

    public float Speed { get { return speed; } }

    public static Player instance;


    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        // Todo: tester sur Mobile 
        jumpButton = FindObjectOfType<InputBtn>();
        _rigidbody = GetComponent<Rigidbody>();
        SetHealtUi();
        Keys.enabled = false;
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;

    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0f, movementY);

        _rigidbody.AddForce(movement * speed);

        if (jumpButton.clicked && isGrounded)
        {
            _rigidbody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                if (!isGrounded)
                {
                    Destroy(collision.gameObject);
                }
                Vector3 enemySpawnPosition = collision.transform.position + playerDistanceSpawn * Random.insideUnitSphere;
                _waveManager.SpawnCoin(enemySpawnPosition);
                break;
            case "Ground":
                isGrounded = true;
                break;
            case "Finish":
                //lancer level suivant..
                StartCoroutine(GameControlManager.instance.manageScene(SceneManager.GetActiveScene().buildIndex + 1));
                break;
            case "GameOverPlane":
                GameControlManager.instance.GameOver();
                break;
            case "Door":
                if (KeyUi.text == "1")
                {
                    door.Play();
                    StartCoroutine(GameControlManager.instance.manageScene(SceneManager.GetActiveScene().buildIndex + 1));
                }
                else
                {
                    LockPanelUi.SetActive(true);
                }
                break;
            default:
                break;
        }
    }

    public void HealtHurt(int value)
    {
        healt += value;

        if (healt <= 0)
        {
            healt = 0;
            GameControlManager.instance.GameOver();
        }
        SetHealtUi();
    }

    private void SetHealtUi()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < healt;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
        if (collision.gameObject.CompareTag("Door"))
        {
            LockPanelUi.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            UpdateScore();
            //lancer particule et detruire après 2sec..
            GameObject go = Instantiate(_coinEffectPrefab, other.transform.position, Quaternion.identity);
            Destroy(go, .2f);
            //destruction de l'ojb
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Key"))
        {
            Keys.enabled = true;
            KeyUi.text = "1";
            UpdateScore(50);
            GameObject go = Instantiate(_coinEffectPrefab, other.transform.position, Quaternion.identity);
            Destroy(go, .2f);
            //destruction de l'ojb
            Destroy(other.gameObject);
        }
    }

    private void UpdateScore(int value = 1)
    {
        ScoreValue += value;

        //j'invoque OnScoreUpdate
        OnScoreUpdate?.Invoke(ScoreValue);

        //HKEY_CURRENT_USER\Software\Unity\UnityEditor\DefaultCompany\Roll_A_Ball
        PlayerPrefs.SetInt("Score", ScoreValue);
        scoreText.text = ScoreValue.ToString();
    }

}