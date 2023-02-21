using UnityEngine;

public class Ground : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "Player"))
        {
            Player.instance.isGrounded = true;
            Debug.Log("isGrounded");
        }
    }
}
