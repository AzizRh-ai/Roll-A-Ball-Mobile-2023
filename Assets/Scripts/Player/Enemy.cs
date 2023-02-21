using UnityEngine;
using UnityEngine.AI;


//TODO: voir le jump d'un agent d'une plateforme a l'autre
// -Check bordure
// -Calcul distance entre plateforme

public class Enemy : MonoBehaviour, IMovement
{
    private Animator _animator;
    [SerializeField] private Player player;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float speed = 5f;
    private NavMeshAgent agent;
    private bool isWalking = false;
    private bool isRunning = false;
    private bool isEating = true;

    public float JumpForce
    {
        get { return jumpForce; }
    }

    public float Speed
    {
        get { return speed; }
    }


    private void Start()
    {
        player = Player.instance;
        _animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

    }

    private void Update()
    {

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            isWalking = true;

            if (agent.velocity.magnitude > agent.speed / 2f)
            {
                isRunning = true;
            }
            isEating = false;
        }
        else
        {
            isWalking = false;
            isRunning = false;
            isEating = true;
        }

        _animator.SetBool("Walk", isWalking);
        _animator.SetBool("Run", isRunning);
        _animator.SetBool("Eat", isEating);
        agent.SetDestination(player.transform.position);
    }
}


