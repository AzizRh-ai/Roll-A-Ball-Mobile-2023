using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IMovement
{
    [Header("Target")]
    [SerializeField] private Player _player;

    [SerializeField] private float jumpForce = 50f;
    [SerializeField] private float speed = 5f;

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
            if (_player.isGrounded)
                _player.HealtHurt(-1);
            collision.rigidbody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }
    }

}
