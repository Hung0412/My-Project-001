using UnityEngine;
public abstract class EnemyCombat : MonoBehaviour, IEnemyCombatInterface
{
    //REFERENCES
    protected GameObject player;
    protected EnemyData enemyData;
    protected Animator animator;
    protected Rigidbody2D rb2D;
    protected PlayerCombat playerCombat;
    protected PlayerController playerController;
    protected PlayerCombatController playerCombatController;

    //VARIABLES
    public bool isDying = false;
    public bool isFacingCharacter = false;

    public virtual void CheckReferences()
    {
        player = GameObject.FindWithTag("Player");
        enemyData = gameObject.GetComponent<EnemyData>();
        animator = gameObject.GetComponent<Animator>();
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        playerCombat = player.GetComponent<PlayerCombat>();
        playerController = player.GetComponent<PlayerController>();
        playerCombatController = player.GetComponent<PlayerCombatController>();
    }
    public virtual void DamageEnemy()
    {
        CharacterData characterData = GameObject.FindWithTag("Player").GetComponent<CharacterDataReference>().characterData;
        enemyData.CurrentHealthValue -= characterData.DamageValue;
        if (enemyData.CurrentHealthValue <= 0)
        {
            isDying = true;
            animator.SetBool(nameof(isDying), true);
            rb2D.bodyType = RigidbodyType2D.Static;
            playerCombat.hasCollided = false;
            if (gameObject.GetComponent<BoxCollider2D>() != null)
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
            else if (gameObject.GetComponent<CircleCollider2D>() != null)
            {
                gameObject.GetComponent<CircleCollider2D>().enabled = false;
            }
            Invoke(nameof(DestroyEnemy), enemyData.DestroyTime);
        }
    }
    public virtual void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    public virtual void FaceCharacter(GameObject enemy, GameObject player)
    {
        if (isDying == false)
        {
            if (enemy.transform.position.x > player.transform.position.x)
            {
                isFacingCharacter = true;
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (enemy.transform.position.x < player.transform.position.x)
            {
                isFacingCharacter = false;
                gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
}
