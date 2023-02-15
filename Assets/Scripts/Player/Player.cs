using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //Event
    public delegate void OnScoreMessage(int value);
    public static event OnScoreMessage OnScoreUpdate;


    [Header("Ball")]
    [SerializeField] private float speed = 300f;
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

    [SerializeField] private float gravityScale = 5;

    [Header("Input")]
    [SerializeField] private InputBtn jumpButton;
    private bool isGrounded = true;
    private Vector2 inputJoystickMovement;
    public float jumpForce = 10f;
    public float smoothness = 10.0f;



    private void Start()
    {
        jumpButton = FindObjectOfType<InputBtn>();
        _rigidbody = GetComponent<Rigidbody>();
        // Instantiate();
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
                StartCoroutine(manageScene(SceneManager.GetActiveScene().buildIndex + 1));
                break;
            case "GameOverPlane":
                StartCoroutine(manageScene(SceneManager.GetActiveScene().buildIndex));
                break;
            default:
                break;
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

        // je save jusqu'a la fin de session
        // _scenario.Score = ScoreValue;

        //je save aussi dans le registre la cl� Score
        //HKEY_CURRENT_USER\Software\Unity\UnityEditor\DefaultCompany\Roll_A_Ball
        PlayerPrefs.SetInt("Score", ScoreValue);

        //J'instancie le mur a une position suivant l'index du tableau dans scenario..
        // Instantiate(_wallPrefab, _scenario.Wall[_scenario.Score - 1], Quaternion.identity);

        // si score =8 niveau suivant
        if (ScoreValue == 8)
        {
            // Je save score dans scenario pour le niveau suivant
            // _scenario.Score = ScoreValue;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (ScoreValue == 16)
        {
            gameOver.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void OnDestroy()
    {
        //On supprime la cl� de registre une fois terminer.
        PlayerPrefs.DeleteKey("Score");
    }

    IEnumerator manageScene(int lvlIndex)
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
}