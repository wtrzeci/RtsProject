using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class Unit : NetworkBehaviour
{
    [SerializeField] private UnityEvent onSelect = null;
    [SerializeField] private UnityEvent onDeselect = null;
    [SerializeField] private Health health;
    public static event Action<Unit> OnUnitSpawn;
    public static event Action<Unit> OnUnitDeSpawn;
    public static event Action<Unit> OnUnitSpawnClient;
    public static event Action<Unit> OnUnitDespawnClient;

    public override void OnStartServer()
    {
        OnUnitSpawn?.Invoke(this);
        health.ServerOnDie += HandleDeath;
    }
    public override void OnStopServer()
    {
        OnUnitDeSpawn?.Invoke(this);
        health.ServerOnDie -= HandleDeath;
    }

    public override void OnStartClient()
    {
        if (!isClientOnly || !hasAuthority) {return;}
        OnUnitSpawnClient?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if (!isClientOnly || !hasAuthority) {return;}
        OnUnitDespawnClient?.Invoke(this);
    }
    [Client]
    public void Select()
    {
        onSelect?.Invoke();
        
    }

    [Client]
    public void DeSelect()
    {
        onDeselect?.Invoke();
    }

    private void HandleDeath(Health _health)
    {
        OnUnitDeSpawn?.Invoke(this);
        NetworkServer.Destroy(this.gameObject);
    }
    
}
