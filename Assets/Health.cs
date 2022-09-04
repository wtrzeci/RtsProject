using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Health : NetworkBehaviour
{
  [SerializeField] float startHealthPoints = 100;
  [SyncVar (hook = nameof(HandleHealthChange))]
  [SerializeField] float CurrentHealth;

  public event Action<Health> ServerOnDie;
  public event Action<float, float> ClientHandleHPChange ; 

  public override void OnStartServer()
  {
    CurrentHealth = startHealthPoints;
  }
[Server]
  public void TakeDamage(float damage)
  {
    CurrentHealth -= damage;
    Mathf.Clamp(CurrentHealth, 0, startHealthPoints);
    if (CurrentHealth == 0)
    {
      ServerOnDie?.Invoke(this);
      Debug.Log("Object destroyed");
      
    }

  }

  private void HandleHealthChange(float oldHealth, float newHealth)
  {
    ClientHandleHPChange?.Invoke(CurrentHealth,startHealthPoints);
  }

}
