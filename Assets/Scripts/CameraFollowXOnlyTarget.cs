using UnityEngine;

public class CameraFollowXOnlyTarget : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float fixedY = 0f;
    [SerializeField] private float fixedZ = 0f;

    private void LateUpdate()
    {
        if (player == null) return;

        transform.position = new Vector3(
            player.position.x,
            fixedY,
            fixedZ
        );
    }
}


