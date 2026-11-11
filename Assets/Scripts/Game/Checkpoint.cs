
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    public int checkpointNumber = 1;
    public Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerMovement player =
            other.GetComponent<PlayerMovement>();

        if (player == null || respawnPoint == null)
            return;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ActivateCheckpoint(
                checkpointNumber,
                respawnPoint.position,
                player
            );
        }
    }
}
