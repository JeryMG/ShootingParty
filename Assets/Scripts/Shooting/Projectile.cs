using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float ProjoSpeed = 10f;
    public LayerMask CollisionMask;
    private float damage = 1f;
    private float Lifetime = .5f;
    private float SkinWidth = .1f;

    private void Start()
    {
        Destroy(gameObject, Lifetime);
        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, CollisionMask);
        if (initialCollisions.Length >0)
        {
            onHitObject(initialCollisions[0]);
        }
    }

    void Update()
    {
        float moveDistance = ProjoSpeed * Time.deltaTime;
        CheckCollision(moveDistance);
        
        transform.Translate(Vector3.forward * Time.deltaTime * ProjoSpeed);
    }

    private void CheckCollision(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, moveDistance + SkinWidth, CollisionMask, QueryTriggerInteraction.Collide))
        {
            onHitObject(hit);
        }
    }

    private void onHitObject(RaycastHit hit)
    {
        print(hit.collider.name);
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeHit(damage, hit);
        }
        Destroy(gameObject);
    }

    void onHitObject(Collider collider)
    {
        IDamageable damageableObject = collider.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(damage);
        }
        Destroy(gameObject);
    } //In case projo spawn inside enemy

    public void setSpeed(float newSpeed)
    {
        ProjoSpeed = newSpeed;
    }
    
    
}
