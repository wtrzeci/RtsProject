using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class Unit : NetworkBehaviour
{
    [SerializeField] private UnityEvent onSelect = null;
    [SerializeField] private UnityEvent onDeselect = null;

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
}
