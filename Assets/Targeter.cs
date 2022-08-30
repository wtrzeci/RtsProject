using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Targeter : NetworkBehaviour
{
    public Targetable target;
    [Command]
    public void CmdSetTarget(GameObject TargetedObject)
    {
        if (!TargetedObject.TryGetComponent<Targetable>(out Targetable newTarget)) {return;}

        target = newTarget;
    }

    [Server]
    public void CmdClearTarget()
    {
        target = null;
    }
    
}
