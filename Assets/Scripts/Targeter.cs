using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Targeter : NetworkBehaviour
{
    [SerializeField] Targetable target;
    [Command]
    public void CmdSetTarget(GameObject TargetedObject)
    {
        if (!TargetedObject.TryGetComponent<Targetable>(out Targetable newTarget)) {return;}

        target = newTarget;
    }
    [Command]
    public void CmdClearTarget()
    {
        target = null;
    }

    public Targetable GetTarget()
    {
        return target;
    }
    
}
