using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MainBase : NetworkBehaviour
{
 [SerializeField] private Health Health;
 public static event Action<MainBase> OnBaseSpawnServer;
 public static event Action<MainBase> OnBaseDespawnServer;

 public override void OnStartServer()
 {
  
  Health.ServerOnDie += HandleDeath;
  OnBaseSpawnServer?.Invoke(this);

 }

 public override void OnStopServer()
 {
  Health.ServerOnDie -= HandleDeath;
  OnBaseDespawnServer?.Invoke(this);

 }

 private void HandleDeath(Health health)
 {
  Debug.Log("Spawner Invoked Dead event");
  NetworkServer.Destroy(this.gameObject);
  OnBaseDespawnServer?.Invoke(this);
 }
}
