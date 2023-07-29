using UnityEngine;

public interface IEnemyCombatInterface
{
    void CheckReferences();
    void DamageEnemy();
    void DestroyEnemy();
    void FaceCharacter(GameObject enemy, GameObject player);
}