using UnityEngine;

public class GhostSlowCombat : EnemyCombat
{
    private GameObject ghostSlowBloomVolume;
    private SpriteRenderer spriteRenderer;
    public Transform attackPoint;
    public float attackRange;

    private Collider2D[] hitPlayer;
    public LayerMask playerLayer;
    public bool hasAttacked;
    // Start is called before the first frame update
    void Start()
    {
        enemyData = gameObject.GetComponent<EnemyData>();
        enemyData.InvokeEnemyData(10, 10, 0, 0.5f, 10, true, EnemyData.MonsterType.GHOST_SLOW);

        player = GameObject.FindWithTag("Player");
        animator = gameObject.GetComponent<Animator>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        spriteRenderer = player.GetComponent<SpriteRenderer>();
        ghostSlowBloomVolume = GameObject.Find("Ghost Slow Volume");
    }

    // Update is called once per frame
    void Update()
    {
        FaceCharacter(gameObject, player);
        Attack_SlowDownCharacterSpeed();
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
    private void Attack_SlowDownCharacterSpeed()
    {
        hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        if (hitPlayer.Length == 0)
        {
            hasAttacked = false;
            animator.SetBool(nameof(hasAttacked), false);

            ghostSlowBloomVolume.gameObject.SetActive(false);
            playerController.walksSpeed = 7;
            playerController.runSpeed = 15;
        }
        foreach (Collider2D player in hitPlayer)
        {
            if (player.CompareTag("Player"))
            {
                hasAttacked = true;
                animator.SetBool(nameof(hasAttacked), true);

                ghostSlowBloomVolume.gameObject.SetActive(true);
                playerController.walksSpeed = 3;
                playerController.runSpeed = 10;
            }
        }
    }
    private void OnDestroy()
    {
        ghostSlowBloomVolume.gameObject.SetActive(false);
        playerController.walksSpeed = 7;
        playerController.runSpeed = 15;
    }
}
