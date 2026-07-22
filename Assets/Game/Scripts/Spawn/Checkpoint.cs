using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private bool isActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isActivated) return;
        if (!other.CompareTag("Player")) return;

        isActivated = true;

        Vector3 savePosition = spawnPoint != null ? spawnPoint.position : transform.position;
        SpawnManager.Instance.UpdateSpawnPoint(savePosition);
    }
}