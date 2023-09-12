using UnityEngine;

public class SkeletonCombat : EnemyCombat
{
    private BoxCollider2D boxCollider2D;
    public float moveForce;
    public bool hasAttacked = false;
    // Start is called before the first frame update
    void Start()
    {
        CheckReferences();
        enemyData.InvokeEnemyData(20, 20, 10, 1, 10, true, EnemyData.MonsterType.SKELETON);
        boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerController.isRolling && !playerController.isJumping && player.transform.position != gameObject.transform.position)
        {
            Invoke(nameof(Attack_FollowCharacter), 0.9f);
            FaceCharacter(gameObject, player);
        }
    }
    public override void DamageEnemy()
    {
        base.DamageEnemy();

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DamageEnemy();
        }
    }
    public override void DestroyEnemy()
    {
        base.DestroyEnemy();
    }
    private void Attack_FollowCharacter()
    {
        if (!playerController.isRolling && !playerController.isJumping)
        {
            if (!boxCollider2D.isTrigger)
            {
                hasAttacked = true;
                animator.SetBool(nameof(hasAttacked), true);
                Vector2 direction = player.transform.position - transform.position;

                if (isDying == false)
                    rb2D.velocity = new Vector2(direction.x, 0).normalized * moveForce;
            }
            else if (boxCollider2D.isTrigger)
            {
                rb2D.gravityScale = 0;
                rb2D.constraints = RigidbodyConstraints2D.FreezePosition;
                rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }
}
