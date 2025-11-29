using UnityEngine;

public class ScorePoint : MonoBehaviour
{
    [SerializeField] private int pointValue = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ScorePoint: OnTriggerEnter2D");
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("ScorePoint: Adding score");
            GameManager.Instance.AddScore(pointValue);
        }
    }
}

