using UnityEngine;

public class FireSkullCombat : EnemyCombat
{
    private BoxCollider2D boxCollider2D;
    public float chasingForce;
    public bool isChasing = false;
    public float moveDistance;
    public bool hasPlayerLeft = true;
    public bool hasPlayerRight = false;
    public bool takeNewPos = true;
    private Vector3 playerStartPos;
    private Vector3 direction;

    public Transform attackPoint;
    public float attackRange;
    private Collider2D[] hitPlayer;
    public LayerMask playerLayer;

    public bool hasAttacked = false;
    public bool attackOneTime = false;
    // Start is called before the first frame update
    void Start()
    {
        CheckReferences();
        enemyData.InvokeEnemyData(60, 60, 30, 0.5f, 30, true, EnemyData.MonsterType.FIRE_SKULL);
        boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack_FollowCharacter();
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
    private void Attack_FollowCharacter()
    {
        if (isDying == false)
        {
            hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
            foreach (Collider2D hitPlayer in hitPlayer)
            {
                if (hitPlayer.CompareTag("Player"))
                {
                    attackOneTime = true;
                }
            }
            if (attackOneTime && !isDying)
            {
                hasAttacked = true;
                animator.SetBool(nameof(hasAttacked), true);
                if (hasPlayerLeft == true)
                {
                    if (takeNewPos == true)
                    {
                        playerStartPos = player.transform.position;
                        takeNewPos = false;
                    }
                    if (player.transform.position.x < gameObject.transform.position.x)
                    {
                        direction = player.transform.position - gameObject.transform.position;
                        rb2D.velocity = new Vector2(direction.x, 0).normalized * chasingForce;
                        if (player.transform.position.x < playerStartPos.x)
                        {
                            takeNewPos = true;
                        }
                    }
                    else if (player.transform.position.x >= gameObject.transform.position.x)
                    {
                        direction = (playerStartPos - new Vector3(moveDistance, 0, 0)) - gameObject.transform.position;
                        rb2D.velocity = new Vector2(direction.x, 0).normalized * chasingForce;
                        if (playerStartPos.x - moveDistance >= gameObject.transform.position.x)
                        {
                            isChasing = false;
                            rb2D.velocity = new Vector2(0, 0);
                            FaceCharacter(gameObject, player);
                            hasPlayerLeft = false;
                            hasPlayerRight = true;
                            takeNewPos = true;
                        }
                    }
                }
                else if (hasPlayerRight)
                {
                    if (takeNewPos)
                    {
                        playerStartPos = player.transform.position;
                        takeNewPos = false;
                    }
                    if (player.transform.position.x > gameObject.transform.position.x)
                    {
                        direction = player.transform.position - gameObject.transform.position;
                        rb2D.velocity = new Vector2(direction.x, 0).normalized * chasingForce;
                        if (player.transform.position.x > playerStartPos.x)
                        {
                            takeNewPos = true;
                        }
                    }
                    else if (player.transform.position.x <= gameObject.transform.position.x)
                    {
                        direction = (playerStartPos + new Vector3(moveDistance, 0, 0)) - gameObject.transform.position;
                        rb2D.velocity = new Vector2(direction.x, 0).normalized * chasingForce;
                        if (playerStartPos.x + moveDistance <= gameObject.transform.position.x)
                        {
                            isChasing = false;
                            rb2D.velocity = new Vector2(0, 0);
                            FaceCharacter(gameObject, player);
                            hasPlayerLeft = true;
                            hasPlayerRight = false;
                            takeNewPos = true;
                        }
                    }
                }
            }
        }
    }
}