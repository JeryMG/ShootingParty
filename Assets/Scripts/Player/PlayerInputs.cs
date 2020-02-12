using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public bool FireWeapon { get; private set; }
    
    public event Action OnFire = delegate {  };

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        //player inputs
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
        FireWeapon = Input.GetButton("Fire1");

        //Look inputs
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            //Debug.DrawLine(ray.origin, point,Color.red);
            LookAt(point);
        }
        
        //Tir
        if (FireWeapon)
        {
            OnFire();
        }
    }
    
    private void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectionPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectionPoint);
    }
}
