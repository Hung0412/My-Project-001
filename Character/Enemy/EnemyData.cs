using System;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    [SerializeField]
    private float maxHealthValue;
    [SerializeField]
    private float currentHealthValue;
    [SerializeField]
    private float damageValue;
    [SerializeField]
    private float destroyTime;
    [SerializeField]
    private float pushForce;
    [SerializeField]
    private bool canBeAttacked;
    public float MaxHealthValue
    {
        get { return maxHealthValue; }
        private set { maxHealthValue = value; }
    }
    public float CurrentHealthValue
    {
        get { return currentHealthValue; }
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Value cannot be negative");
            }
            currentHealthValue = Mathf.Max(0, Mathf.Min(value, MaxHealthValue));
        }
    }
    public float DamageValue
    {
        get { return damageValue; }
        set { damageValue = value; }
    }
    public float DestroyTime
    {
        get { return destroyTime; }
        private set { destroyTime = value; }
    }
    public float PushForce
    {
        get { return pushForce; }
        set { pushForce = value; }
    }
    public bool CanBeCharged
    {
        get { return canBeAttacked; }
        set { canBeAttacked = value; }
    }
    public enum MonsterType
    {
        WIZARD,
        WIZARD_FIREBALL,

        GHOST_SLOW,
        GHOST_FLAME,

        SKELETON,

        ANGEL,
        ANGEL_LIGHTNING,

        FIRE_SKULL,

        DRAGON
    }
    public MonsterType monsterType;
    public MonsterType _MonsterType
    {
        get { return monsterType; }
        private set { monsterType = value; }
    }
    public void InvokeEnemyData(float maxHealthValue, float currentHealthValue, float damageValue, float destroyTime, float pushForce, bool canBeCharged, MonsterType monsterType)
    {
        MaxHealthValue = maxHealthValue;
        CurrentHealthValue = currentHealthValue;
        DamageValue = damageValue;
        DestroyTime = destroyTime;
        PushForce = pushForce;
        CanBeCharged = canBeCharged;
        _MonsterType = monsterType;
    }
}
