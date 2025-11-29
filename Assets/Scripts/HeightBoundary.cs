using UnityEngine;
using UnityEngine.SceneManagement;

public class HeightBoundary : MonoBehaviour
{
    [SerializeField] private float maxHeight = 10f;
    [SerializeField] private float minHeight = -10f;

    private void Update()
    {
        float currentHeight = transform.position.y;

        if (currentHeight > maxHeight || currentHeight < minHeight)
        {
            RestartScene();
        }
    }

    private void RestartScene()
    {
        GameManager.Instance.RestartGameScene();
    }
}

