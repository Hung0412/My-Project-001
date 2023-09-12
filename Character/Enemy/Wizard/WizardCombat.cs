using UnityEngine;
public class WizardCombat : EnemyCombat
{
    public GameObject fireBall;
    public Transform firePos;
    public bool hasAttacked = false;

    private float fireTimer;
    private float fireResetTime = 4;
    public Transform fireAttackPoint;
    public float fireRange;
    private Collider2D[] hitPlayer;
    public LayerMask playerLayer;
    // Start is called before the first frame update
    void Start()
    {
        CheckReferences();
        enemyData.InvokeEnemyData(60, 60, 10, 0.7f, 10, true, EnemyData.MonsterType.WIZARD);
        fireTimer = fireResetTime;
    }

    // Update is called once per frame
    void Update()
    {
        Attack_Fire();
        FaceCharacter(gameObject, player);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(fireAttackPoint.position, fireRange);
    }
    private void Attack_Fire()
    {
        if (!hasAttacked && !isDying)
        {
            hitPlayer = Physics2D.OverlapCircleAll(fireAttackPoint.position, fireRange, playerLayer);
            if (hitPlayer.Length == 0)
            {
                fireTimer = 0;
            }
            foreach (Collider2D player in hitPlayer)
            {
                if (player.CompareTag("Player"))
                {
                    fireTimer += Time.deltaTime;

                    if (fireTimer >= fireResetTime - 1.5f && hasAttacked == false)
                    {
                        animator.SetBool(nameof(hasAttacked), true);
                        Invoke(nameof(SetHasAttackedToFalse), 0.9f);
                    }
                    if (fireTimer >= fireResetTime && hasAttacked == false)
                    {
                        hasAttacked = true;

                        fireTimer = 0;
                        Instantiate(fireBall, firePos.position, Quaternion.identity);
                    }
                }
            }
        }
    }
    private void SetHasAttackedToFalse()
    {
        hasAttacked = false;
        animator.SetBool(nameof(hasAttacked), false);
    }
    public override void DamageEnemy()
    {
        base.DamageEnemy();
    }
    public override void DestroyEnemy()
    {
        base.DestroyEnemy();
    }
}
