using UnityEngine;
using UnityEngine.AI;


//TODO: voir le jump d'un agent d'une plateforme a l'autre
// -Check bordure
// -Calcul distance entre plateforme

public class Enemy : MonoBehaviour, IMovement
{
    [SerializeField] private Player player;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float speed = 5f;
    private NavMeshAgent agent;
    public float JumpForce
    {
        get { return jumpForce; }
    }

    public float Speed
    {
        get { return speed; }
    }

    void Start()
    {
        player = Player.instance;
        agent = GetComponent<NavMeshAgent>();

        //Check et maj de la destination de l'agent toute les 5sec
        InvokeRepeating("UpdateDestination", 0f, 0.5f);
    }

    private void UpdateDestination()
    {
        if (player != null)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (player.isGrounded)
            {
                player.HealtHurt(-1);
                collision.rigidbody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            }
        }
    }

}
