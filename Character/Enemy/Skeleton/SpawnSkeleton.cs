using UnityEngine;

public class SpawnSkeleton : MonoBehaviour
{
    //REFERENCES
    public GameObject skeletonPrefabs;
    private GameObject spawnedSkeleton;
    private Collider2D[] hitPlayer;
    public LayerMask playerLayer;
    //VARIABLES
    public Transform spawnPoint;
    public float detectPlayerRange;
    private bool hasSpawned = false;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        Spawn_Skeleton();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(spawnPoint.position, detectPlayerRange);
    }
    private void Spawn_Skeleton()
    {
        hitPlayer = Physics2D.OverlapCircleAll(spawnPoint.position, detectPlayerRange, playerLayer);
        foreach (Collider2D player in hitPlayer)
        {
            if (player.CompareTag("Player") && hasSpawned == false)
            {
                spawnedSkeleton = Instantiate(skeletonPrefabs, spawnPoint.position + new Vector3(0, 1f, 0), Quaternion.identity);
                hasSpawned = true;
            }
        }
    }
}
