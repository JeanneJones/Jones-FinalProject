
using UnityEngine;



public class MovingPlatform : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Make the player a child of the moving platform
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Remove the player from being a child of the moving platform
            collision.transform.SetParent(null);
        }
    }
}