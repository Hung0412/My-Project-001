using UnityEngine;

public class SpawnGhostFear : MonoBehaviour
{
    public GameObject ghostFearPrefab;
    private GameObject spawnedGhostFear;
    private Collider2D[] hitPlayer;
    public LayerMask playerLayer;
    public Transform spawnPoint;
    public float detectPlayer;
    private bool hasSpawned = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Spawn_GhostFear();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(spawnPoint.position, detectPlayer);
    }
    private void Spawn_GhostFear()
    {
        hitPlayer = Physics2D.OverlapCircleAll(spawnPoint.position, detectPlayer, playerLayer);
        foreach (Collider2D player in hitPlayer)
        {
            if (player.CompareTag("Player") && !hasSpawned)
            {
                spawnedGhostFear = Instantiate(ghostFearPrefab, spawnPoint.position, Quaternion.identity);
                hasSpawned = true;
            }
        }
    }
}
