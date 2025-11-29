using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            RestartScene();
        }
    }

    private void RestartScene()
    {
        GameManager.Instance.RestartGameScene();
    }
}

