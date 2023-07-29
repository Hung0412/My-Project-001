public class DragonCombat : EnemyCombat
{
    // Start is called before the first frame update
    void Start()
    {

        CheckReferences();
        enemyData.InvokeEnemyData(300, 300, 15, 1f, 20, true, EnemyData.MonsterType.DRAGON);
    }

    // Update is called once per frame
    void Update()
    {
        FaceCharacter(gameObject, player);
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
