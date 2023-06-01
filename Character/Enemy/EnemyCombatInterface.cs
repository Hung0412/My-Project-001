using UnityEngine;

public interface EnemyCombatInterface
{
    void CheckReferences();
    void DamageEnemy();
    void DestroyEnemy();
    void FaceCharacter(GameObject enemy, GameObject player);
}