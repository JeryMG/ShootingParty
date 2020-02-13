using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    protected float health;
    [SerializeField] private float MaxHealth = 5;
    protected bool dead;

    public event Action OnDeath;

    protected virtual void Start()
    {
        health = MaxHealth;
    }

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        TakeDamage(damage);
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    [ContextMenu("Se suicider")]
    protected void Die()
    {
        dead = true;
        if (OnDeath != null)
        {
            OnDeath();
        }
        Destroy(gameObject);
    }
}
