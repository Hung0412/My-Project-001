using System.Collections.Generic;
using UnityEngine;
public class PlayerCombat : MonoBehaviour
{
    //REFERENCEs
    private CharacterData characterData;
    private GameObject[] destroyedWithPlayer;

    private Animator animator;
    private Rigidbody2D rb2D;
    private PlayerController playerController;
    private PlayerCombatController playerCombatController;
    private EffectController effectController;
    public GameObject hurtEffectPrefab;

    //VARIABLES
    public float pushForce;
    private GameObject spawnedObject;
    private bool hasSpawned = false;
    public bool hasCollided = false;
    public bool isPushingAway = false;
    //[HideInInspector]
    public Collider2D[] hitEnemies;
    //[HideInInspector]
    public List<Collider2D> hitEnemiesList;
    public bool dangerDetect = false;

    private void Start()
    {
        characterData = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterDataReference>().characterData;
        destroyedWithPlayer = GameObject.FindGameObjectsWithTag("DestroyedWithPlayer");

        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        playerCombatController = GetComponent<PlayerCombatController>();
        effectController = GameObject.Find("----Game Master----").GetComponent<EffectController>();
    }
    private void Update()
    {
        //Debug.Log(rb2D.velocity);
        SetConditionsDependOnIsPushingAway();
    }
    public void Attack1(Transform attackPoint, float attackRange, LayerMask characterLayers)
    {
        hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, characterLayers);
        AttackCondition();
    }
    public void Attack2(Transform attackPoint, Vector3 attackSize, float angel, LayerMask characterLayers)
    {
        hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, attackSize, angel, characterLayers);
        AttackCondition();
    }
    private void AttackCondition()
    {
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<EnemyData>().CanBeCharged == true)
            {
                characterData.CurrentHealingChargeValue += 1;

            }
            switch (enemy.GetComponent<EnemyData>()._MonsterType)
            {
                case EnemyData.MonsterType.WIZARD:
                    enemy.GetComponent<WizardCombat>().DamageEnemy();
                    break;
                case EnemyData.MonsterType.WIZARD_FIREBALL:
                    enemy.GetComponent<FireBallCombat>().DamageEnemy();
                    break;
                case EnemyData.MonsterType.GHOST_SLOW:
                    enemy.GetComponent<GhostSlowCombat>().DamageEnemy();
                    break;
                case EnemyData.MonsterType.GHOST_FLAME:
                    enemy.GetComponent<GhostFlameCombat>().DamageEnemy();
                    break;
                case EnemyData.MonsterType.SKELETON:
                    enemy.GetComponent<SkeletonCombat>().DamageEnemy();
                    break;
                case EnemyData.MonsterType.ANGEL:
                    enemy.GetComponent<AngelCombat>().DamageEnemy();
                    break;
                case EnemyData.MonsterType.ANGEL_LIGHTNING: //Don't attack Angel_Lightning
                    break;
                case EnemyData.MonsterType.FIRE_SKULL:
                    enemy.GetComponent<FireSkullCombat>().DamageEnemy();
                    break;
            }
        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && hasCollided == false)
        {
            isPushingAway = true;
            hasCollided = true;
            DamageCharacter(other);
            PushPlayer(other);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Enemy") && isPushingAway == false
            && playerCombatController.hasPunched == false && playerCombatController.hasKicked == false
            && playerCombatController.hasFlyKicked == false && playerCombatController.hasFlyingKicked == false
            && playerCombatController.hasCrouchKicked == false)

            || (collision.gameObject.CompareTag("Enemy")
            && playerController.facingVal * collision.gameObject.GetComponent<Rigidbody2D>().velocity.x > 0

                && (playerCombatController.hasPunched == true || playerCombatController.hasKicked == true
                || playerCombatController.hasFlyKicked == true || playerCombatController.hasFlyingKicked == true
                || playerCombatController.hasCrouchKicked == true)))
        {
            isPushingAway = true;
            //SpawnHurtEffect();

            hasCollided = true;
            DamageCharacter(collision);
            PushPlayer(collision);

            //playerCombatController.RemoveHitEnemiesFromList();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            hasCollided = false;
        }
    }
    public void DamageCharacter(Collision2D collision)
    {
        EnemyData enemyData = collision.gameObject.GetComponent<EnemyData>();
        characterData.CurrentHealthValue -= enemyData.DamageValue;
        if (characterData.CurrentHealthValue <= 0)
        {
            DestroyCharacter();
        }
    }
    private void PushPlayer(Collision2D collision)
    {
        pushForce = collision.gameObject.GetComponent<EnemyData>().PushForce;
        Vector2 pushDirection = (transform.position - collision.transform.position).normalized;

        if (pushDirection.x < pushDirection.y)
        {
            if (pushDirection.x < 0)
            {
                pushDirection.x = -1;
            }
            else if (pushDirection.x > 0)
            {
                pushDirection.x = 1;
            }
        }

        rb2D.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
    }
    private void SetConditionsDependOnIsPushingAway()
    {
        if (isPushingAway == true)
        {
            animator.SetBool("isPushingAway", true);

            playerController.isWalking = false;
            animator.SetBool("isWalking", false);

            playerController.isRunning = false;
            animator.SetBool("isRunning", false);

            playerController.isJumping = false;
            playerController.animator.SetBool("isJumping", false);

            playerController.isFalling = false;
            playerController.animator.SetBool("isFalling", false);

            if (rb2D.velocity.x < 0.2f && rb2D.velocity.y == 0)
            {
                hasCollided = false;
                isPushingAway = false;
                animator.SetBool("isPushingAway", false);
            }
        }
    }
    private void SpawnHurtEffect()
    {
        if (hasSpawned == false)
        {
            if (playerController.isFacing == true)
            {
                spawnedObject = Instantiate(hurtEffectPrefab, gameObject.transform.position + new Vector3(0.5f, 0), Quaternion.identity);
                spawnedObject.transform.position = transform.position + new Vector3(0.5f, 0);
            }
            else if (playerController.isFacing == false)
            {
                spawnedObject = Instantiate(hurtEffectPrefab, gameObject.transform.position + new Vector3(0.5f, 0), Quaternion.Euler(0, 180, 0));
                spawnedObject.transform.position = transform.position - new Vector3(0.5f, 0);
            }
            StartCoroutine(effectController.DestroyEffect(spawnedObject, 0.4f));
            hasSpawned = true;
            isPushingAway = false;
        }
    }
    public void DestroyCharacter()
    {
        gameObject.SetActive(false);
        for (int i = 0; i < destroyedWithPlayer.Length; i++)
        {
            Destroy(destroyedWithPlayer[i]);
        }
        //Stop Playmode 
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
