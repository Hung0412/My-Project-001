using UnityEngine;

public class SkeletonCombat : EnemyCombat
{
    private BoxCollider2D boxCollider2D;
    public float moveForce;
    public bool hasAttacked = false;
    // Start is called before the first frame update
    void Start()
    {
        enemyData = gameObject.GetComponent<EnemyData>();
        enemyData.InvokeEnemyData(20, 20, 10, 1, 10, true, EnemyData.MonsterType.SKELETON);

        playerCombat = GameObject.FindWithTag("Player").GetComponent<PlayerCombat>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        animator = gameObject.GetComponent<Animator>();
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        boxCollider2D = gameObject.GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isRolling == false)
        {
            Invoke(nameof(Attack_FollowCharacter), 0.9f);
            FaceCharacter(gameObject, player);
        }

    }
    public override void DamageEnemy()
    {
        CharacterData characterData = GameObject.FindWithTag("Player").GetComponent<CharacterDataReference>().characterData;
        enemyData.CurrentHealthValue -= characterData.DamageValue;
        if (enemyData.CurrentHealthValue <= 0)
        {
            isDying = true;
            animator.SetBool(nameof(isDying), true);
            Invoke(nameof(DestroyEnemy), enemyData.DestroyTime);
            boxCollider2D.isTrigger = true;
            playerCombat.hasCollided = false;
            hasAttacked = false;
            animator.SetBool(nameof(hasAttacked), false);
        }
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
        if (boxCollider2D.isTrigger == false && playerController.isOnGround == true)
        {
            hasAttacked = true;
            animator.SetBool(nameof(hasAttacked), true);
            Vector2 direction = player.transform.position - transform.position;
            rb2D.velocity = new Vector2(direction.x, 0).normalized * moveForce;
        }
        else if (boxCollider2D.isTrigger == true)
        {
            rb2D.constraints = RigidbodyConstraints2D.FreezePosition;
            rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
