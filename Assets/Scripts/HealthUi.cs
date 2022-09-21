using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUi : MonoBehaviour
{
   [SerializeField] Image healthBar;
   [SerializeField] Health health;

   private void Awake()
   {
        health.gameObject.SetActive(false);
      health.ClientHandleHPChange += ChangeBar;
   }

   private void OnDestroy()
   {
      health.ClientHandleHPChange -= ChangeBar;
   }

   private void ChangeBar(float currentHealth,float maxHealth)
   {
        health.gameObject.SetActive(true);
      healthBar.fillAmount = currentHealth / maxHealth;
   }
}
