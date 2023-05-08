using UnityEngine;

public class AngelCombat : EnemyCombat
{
    public GameObject preAttackPrefab;
    public GameObject postAttackPrefab;

    public Transform attackPoint;
    public float attackRange;

    private Collider2D[] hitPlayer;
    public LayerMask playerLayer;
    public bool hasAttacked;

    [HideInInspector]
    public GameObject spawnedLightning;
    private GameObject spawnedBewareLightning;
    public bool hasSpawnedLightning = false;
    public bool hasSpawnedBewareLightning = false;
    private float spawnTimer;
    public float spawnResetTimer = 4;
    // Start is called before the first frame update
    void Start()
    {
        enemyData = gameObject.GetComponent<EnemyData>();
        enemyData.InvokeEnemyData(40, 40, 20, 0.5f, 20, true, EnemyData.MonsterType.ANGEL);

        animator = gameObject.GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        spawnTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        FaceCharacter(gameObject, player);
        Attack_Fire();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    public override void DamageEnemy()
    {
        base.DamageEnemy();
    }
    public override void DestroyEnemy()
    {
        base.DestroyEnemy();
    }
    private void Attack_Fire()
    {

        hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        if (hitPlayer.Length == 0 || spawnTimer >= spawnResetTimer)
        {
            if (spawnedBewareLightning != null)
            {
                Destroy(spawnedBewareLightning);
            }
            spawnTimer = 0;
            hasSpawnedLightning = false;
            hasSpawnedBewareLightning = false;
        }
        foreach (Collider2D player in hitPlayer)
        {
            if (player.CompareTag("Player"))
            {
                spawnTimer += Time.deltaTime;
                if (hasSpawnedBewareLightning == false && spawnTimer >= spawnResetTimer - 3f)
                {
                    spawnedBewareLightning = Instantiate(preAttackPrefab, player.transform.position + new Vector3(0, 0, -1), Quaternion.identity);
                    hasSpawnedBewareLightning = true;
                }
                if (spawnedBewareLightning != null && hasSpawnedLightning == false && hasSpawnedBewareLightning == true
                    && spawnTimer <= spawnResetTimer - 1.5f)
                {
                    spawnedBewareLightning.transform.position = player.transform.position;
                }
                if (hasSpawnedLightning == false && spawnTimer >= spawnResetTimer)
                {
                    Destroy(spawnedBewareLightning);
                    spawnedLightning = Instantiate(postAttackPrefab, spawnedBewareLightning.transform.position + new Vector3(0, 4f, 0), Quaternion.identity);
                    hasSpawnedLightning = true;
                    spawnTimer = 0;
                }
            }
        }
    }
    private void OnDestroy()
    {
        if (spawnedBewareLightning != null)
        {
            Destroy(spawnedBewareLightning);
        }
    }
}
