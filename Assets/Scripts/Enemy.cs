using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{
    public enum State
    {
        Idle,
        Chasing,
        Attacking
    }

    private State currentState;
    private NavMeshAgent pathFinder;
    private Material skinMat;
    private Transform target;
    private LivingEntity targetEntity;

    private Color originalColor;
    
    private float attackDistanceTreshold = 1.5f;
    private float TimeBetweenAttacks = 1;
    private float NextAttackTime;
    private float myCollisionRadius;

    private bool hasTarget;
    private float damage = 1f;

    protected override void Start()
    {
        base.Start();
        pathFinder = GetComponent<NavMeshAgent>();
        skinMat = GetComponent<Renderer>().material;
        originalColor = skinMat.color;
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            currentState = State.Chasing;
            hasTarget = true;
            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.GetComponent<LivingEntity>();
            targetEntity.OnDeath += OnTargetDeath;
            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
        
            StartCoroutine(UpdatePath());
        }
    }

    private void Update()
    {
        if (hasTarget)
        {
            if (Time.time > NextAttackTime)
            {
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrDstToTarget < Mathf.Pow(attackDistanceTreshold, 2))
                {
                    NextAttackTime = Time.time + TimeBetweenAttacks;
                    StartCoroutine(Attack());
                }
            }
        }
    }

    IEnumerator UpdatePath()
    {
        float refreshRate = 0.25f;

        while (hasTarget)
        {
            if (currentState == State.Chasing)
            {
                Vector3 targetPosition = new Vector3(target.position.x, 0 , target.position.z);
                if (!dead)
                {
                    pathFinder.SetDestination(targetPosition);
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }

    IEnumerator Attack()
    {
        currentState = State.Attacking;
        skinMat.color = Color.magenta;
        pathFinder.enabled = false;
        
        Vector3 originalPosition = transform.position;
        Vector3 attackPosition = target.position - (target.position - transform.position).normalized * myCollisionRadius;
        float attackSpeed = 3f;
        float percent = 0;
        bool appliedDamage = false;
        
        //Animation de Lunge
        while (percent <= 1)
        {
            if (percent >= .5f && !appliedDamage)
            {
                appliedDamage = true;
                targetEntity.TakeDamage(damage);
            }
            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);
            yield return null;
        }

        skinMat.color = originalColor;
        currentState = State.Chasing;
        pathFinder.enabled = true;
    }

    private void OnTargetDeath()
    {
        hasTarget = false;
        currentState = State.Idle;
    }
}
