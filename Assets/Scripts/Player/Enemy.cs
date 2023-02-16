using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IMovement
{
    [Header("Target")]
    [SerializeField] private Player _player;

    public float jumpForce = 10f;
    public float speed = 5f;

    public float JumpForce
    {
        get { return jumpForce; }
    }

    public float Speed
    {
        get { return speed; }
    }


    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = _player.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("touché!!!!!");
            _player.HealtHurt(-1);
            collision.rigidbody.AddForce(Vector3.up * JumpForce * 50 * Time.deltaTime);
        }
    }

}
