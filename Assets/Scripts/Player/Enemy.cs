using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Player goal;


    // Start is called before the first frame update
    void Start()
    {
        goal = FindObjectOfType<Player>();
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
