using UnityEngine;
public class FireBallCombat : EnemyCombat
{
    private CircleCollider2D circleCollider;

    public float fireForce;
    // Start is called before the first frame update
    void Start()
    {
        CheckReferences();
        enemyData.InvokeEnemyData(10, 10, 10, 0.5f, 10, false, EnemyData.MonsterType.WIZARD_FIREBALL);
        circleCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack_FollowPlayer();
    }
    private void Attack_FollowPlayer()
    {
        if (circleCollider.isTrigger == false)
        {
            Vector2 direction = player.transform.position - transform.position;
            rb2D.velocity = new Vector2(direction.x, direction.y).normalized * fireForce;
            float fireballRotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, fireballRotation);
        }
        else if (circleCollider.isTrigger == true)
        {
            rb2D.constraints = RigidbodyConstraints2D.FreezePosition;
            rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DamageEnemy();
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
            circleCollider.isTrigger = true;
            playerCombat.hasCollided = false;
        }
    }
    public override void DestroyEnemy()
    {
        base.DestroyEnemy();
    }
}
