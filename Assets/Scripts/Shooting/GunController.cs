using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
   [SerializeField] private Gun equipedGun;
   public Transform WeaponHand;
   public Gun startingGun;

   private void Start()
   {
      if (startingGun != null)
      {
         EquipGun(startingGun);
      }
   }

   public void EquipGun(Gun gunToEquip)
   {
      if (equipedGun != null)
      {
         Destroy(equipedGun);
      }
      equipedGun = Instantiate(gunToEquip, WeaponHand.position, WeaponHand.rotation,WeaponHand) as Gun;
   }
}
