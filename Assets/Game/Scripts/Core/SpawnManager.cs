using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Spawn")]
    [SerializeField] private Transform initialSpawnPoint;

    private Vector3 currentSpawnPosition;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        currentSpawnPosition = initialSpawnPoint != null ? initialSpawnPoint.position : player.position;

        RespawnPlayer();
    }

    public void UpdateSpawnPoint(Vector3 newPosition)
    {
        currentSpawnPosition = newPosition;
        Debug.Log($"[SpawnManager] Checkpoint saved: {newPosition}");
    }

    public void RespawnPlayer()
    {
        CharacterController cc = player.GetComponent<CharacterController>();

        if (cc != null) cc.enabled = false;
        player.position = currentSpawnPosition;
        if (cc != null) cc.enabled = true;
    }
}