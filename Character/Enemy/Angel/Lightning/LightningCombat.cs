using UnityEngine;

public class LightningCombat : EnemyCombat
{
    private GameObject[] gameObjects;
    private AngelCombat angelCombat;
    private BoxCollider2D boxCollider2D;
    public Transform destroyNonCollide;
    private Collider2D[] hitAnything;
    public Vector3 destroySize;
    // Start is called before the first frame update
    void Start()
    {
        enemyData = gameObject.GetComponent<EnemyData>();
        enemyData.InvokeEnemyData(10, 10, 20, 0.8f, 40, false, EnemyData.MonsterType.ANGEL_LIGHTNING);

        animator = gameObject.GetComponent<Animator>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        player = GameObject.FindWithTag("Player");
        boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i].GetComponent<AngelCombat>() != null
                && gameObject.transform.position == gameObjects[i].GetComponent<AngelCombat>().spawnedLightning.transform.position)
            {
                angelCombat = gameObjects[i].GetComponent<AngelCombat>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        DestroyIfNonCollide();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(destroyNonCollide.position, destroySize);
    }
    public override void DamageEnemy()
    {
        base.DamageEnemy();
    }
    public override void DestroyEnemy()
    {
        base.DestroyEnemy();
    }
    private void DestroyIfNonCollide()
    {
        hitAnything = Physics2D.OverlapBoxAll(destroyNonCollide.position, destroySize, 0);
        foreach (Collider2D ob in hitAnything)
        {
            if (ob.CompareTag("Enemy") || ob.CompareTag("Ground") || ob.CompareTag("Player"))
            {
                DamageEnemy();
            }
        }
    }
}
