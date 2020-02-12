using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInputs))]
public class Player : LivingEntity
{
    private Rigidbody rb;
    private PlayerInputs Inputs;
    private GunController gunController;
    private Vector3 Velocity;
    [SerializeField] private float speed = 5f;

    void Awake()
    {
        Inputs = GetComponent<PlayerInputs>();
        gunController = GetComponent<GunController>();
        rb = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 movement = new Vector3(Inputs.Horizontal, 0, Inputs.Vertical);
        Velocity = movement.normalized * speed;
        rb.MovePosition(rb.position + Velocity * Time.deltaTime);
    }
    
}
