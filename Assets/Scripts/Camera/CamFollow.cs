using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    public GameObject player;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
