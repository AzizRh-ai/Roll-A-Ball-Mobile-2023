using UnityEngine;

public class ACoin : MonoBehaviour
{
    public Vector3 dir;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(dir * Time.deltaTime);
    }
}
