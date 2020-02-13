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
            onHitObject(initialCollisions[0], transform.position);
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
            onHitObject(hit.collider, hit.point);
        }
    }

    void onHitObject(Collider collider, Vector3 hitPoint)
    {
        IDamageable damageableObject = collider.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeHit(damage, hitPoint, transform.forward);
        }
        Destroy(gameObject);
    } //In case projo spawn inside enemy

    public void setSpeed(float newSpeed)
    {
        ProjoSpeed = newSpeed;
    }
    
    
}
