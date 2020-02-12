using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform muzzle;
    public Projectile projectileBullet;
    [SerializeField] private float TimeBetweenShots = 0.1f;
    [SerializeField] float muzzleSpeed = 35f;
    
    private float nextShotTime;

    private void Awake()
    {
        GetComponentInParent<PlayerInputs>().OnFire += Shoot;
    }

    public void Shoot()
    {
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + TimeBetweenShots;
            Projectile newProjectile = Instantiate(projectileBullet, muzzle.position, muzzle.rotation);
            newProjectile.setSpeed(muzzleSpeed);
        }
    }
}
