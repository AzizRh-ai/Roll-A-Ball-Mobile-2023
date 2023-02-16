using System.Collections.Generic;
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
    [SerializeField] private List<GameObject> ennemyPrefab;

    [Header("Score Value")]
    private int ScoreValue = 0;

    // Score
    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI scoreText;

    // Score
    [Header("GameOver UI")]
    [SerializeField] private GameObject gameOver;


    [Header("Input")]
    [SerializeField] private InputBtn jumpButton;
    private bool isGrounded = true;
    private Vector2 inputJoystickMovement;

    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float speed = 5f;

    [SerializeField] private int healt = 5;
    [SerializeField] private Image[] hearts;

    public float JumpForce { get { return jumpForce; } }

    public float Speed { get { return speed; } }

    private void Start()
    {
        jumpButton = FindObjectOfType<InputBtn>();
        _rigidbody = GetComponent<Rigidbody>();
        SetHealtUi();
    }

    private void OnMove(InputValue movementValue)
    {
        inputJoystickMovement = movementValue.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(inputJoystickMovement.x * speed, 0f, inputJoystickMovement.y * speed);

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
                Destroy(collision.gameObject);

                UpdateScore();
                break;
            case "Ground":
                isGrounded = true;
                break;
            case "Finish":
                //lancer level suivant..
                StartCoroutine(GameControlManager.instance.manageScene(SceneManager.GetActiveScene().buildIndex + 1));
                break;
            case "GameOverPlane":
                gameOver.SetActive(true);
                //Time.timeScale = 0;
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
            gameOver.SetActive(true);
            healt = 0;
            Time.timeScale = 0;
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
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Debug.Log("Coin touché!");
            //lancer particule et detruire après 2sec..
            GameObject go = Instantiate(_coinEffectPrefab, other.transform.position, Quaternion.identity);
            Destroy(go, .2f);
            //destruction de l'ojb
            Destroy(other.gameObject);
        }
    }

    private void UpdateScore()
    {
        ScoreValue++;

        //j'invoque OnScoreUpdate
        OnScoreUpdate?.Invoke(ScoreValue);

        //je save aussi dans le registre la cl� Score
        //HKEY_CURRENT_USER\Software\Unity\UnityEditor\DefaultCompany\Roll_A_Ball
        PlayerPrefs.SetInt("Score", ScoreValue);

        //TODO:Instancier 3 mur autour du player
        //Instantiate(_wallPrefab, _scenario.Wall[_scenario.Score - 1], Quaternion.identity);

    }

}