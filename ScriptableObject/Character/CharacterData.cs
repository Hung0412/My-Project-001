using System;
using UnityEngine;
[CreateAssetMenu]
public class CharacterData : ScriptableObject
{
    [SerializeField]
    private float maxHealthValue;
    [SerializeField]
    private float currentHealthValue;
    [SerializeField]
    private float maxHealingChargeValue;
    [SerializeField]
    private float currentHealingChargeValue;
    [SerializeField]
    private float healthValue;
    [SerializeField]
    private float damageValue;

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
            currentHealthValue = Mathf.Max(0, Mathf.Min(value, MaxHealthValue));
        }
    }

    public float MaxHealingChargeValue
    {
        get { return maxHealingChargeValue; }
        private set { maxHealingChargeValue = value; }
    }

    public float CurrentHealingChargeValue
    {
        get { return currentHealingChargeValue; }
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Value cannot be negative");
            }
            currentHealingChargeValue = Mathf.Max(0, Mathf.Min(value, MaxHealingChargeValue));
        }
    }

    public float HealthValue
    {
        get { return healthValue; }
        set { healthValue = value; }
    }

    public float DamageValue
    {
        get { return damageValue; }
        set { damageValue = value; }
    }

    public void InvokeCharacterData(float maxHealthValue, float currentHealthValue, float maxHealingChargeValue, float currentHealingChargeValue, float healthValue, float damageValue)
    {
        MaxHealthValue = maxHealthValue;
        CurrentHealthValue = currentHealthValue;
        MaxHealingChargeValue = maxHealingChargeValue;
        CurrentHealingChargeValue = currentHealingChargeValue;
        HealthValue = healthValue;
        DamageValue = damageValue;
    }
}

