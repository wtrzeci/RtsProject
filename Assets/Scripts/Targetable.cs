using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Targetable : NetworkBehaviour
{
    public Targetable target;
    [SerializeField] Transform AimAtPoint;

    public Transform AimPoint()
    {
        return AimAtPoint;
    }
}
