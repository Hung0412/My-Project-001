using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFlameCombat : EnemyCombat
{
    private CharacterData characterData;
    private SpriteRenderer spriteRenderer;
    public GameObject flameEffect;
    public bool hasSpawnedFlameEffect = false;
    private GameObject spawnedFlameEffect;
    public bool hasAppeard = false;
    public bool isIdling = false;
    public bool hasAttacked = false;

    public Transform attackPoint;
    public float attackRange;
    private Collider2D[] hitPlayer;
    public LayerMask playerLayer;

    // Start is called before the first frame update
    void Start()
    {
        enemyData = gameObject.GetComponent<EnemyData>();
        enemyData.InvokeEnemyData(40, 40, 1, 1, 10, true, EnemyData.MonsterType.GHOST_FLAME);

        animator = gameObject.GetComponent<Animator>();

        player = GameObject.FindWithTag("Player");
        spriteRenderer = player.gameObject.GetComponent<SpriteRenderer>();
        characterData = player.GetComponent<CharacterDataReference>().characterData;
        playerController = player.GetComponent<PlayerController>();
        playerCombat = player.GetComponent<PlayerCombat>();

    }

    // Update is called once per frame
    void Update()
    {
        FaceCharacter(gameObject, player);
        Attack();
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
    private void Attack()
    {
        hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        if (hitPlayer.Length == 0)
        {
            hasAttacked = false;
            animator.SetBool(nameof(hasAttacked), false);
            Destroy(spawnedFlameEffect);

        }
        foreach (Collider2D player in hitPlayer)
        {
            if (player.CompareTag("Player"))
            {
                if (hasAttacked == false)
                {
                    hasAttacked = true;
                    animator.SetBool(nameof(hasAttacked), true);
                    Invoke(nameof(FearCharacter), 1f);
                }
                if (spawnedFlameEffect != null)
                {
                    spawnedFlameEffect.transform.position = player.transform.position + new Vector3(0, 0.5f, 0);
                }
                else if (spawnedFlameEffect == null)
                {
                    hasSpawnedFlameEffect = false;
                }
            }
        }
    }
    private void FearCharacter()
    {
        hasAttacked = false;
        animator.SetBool(nameof(hasAttacked), false);
        characterData.CurrentHealthValue -= 2;
        if (hasSpawnedFlameEffect == false)
        {
            hasSpawnedFlameEffect = true;
            spawnedFlameEffect = Instantiate(flameEffect, player.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        }
    }
    private void OnDestroy()
    {
        Destroy(spawnedFlameEffect);
    }
}
