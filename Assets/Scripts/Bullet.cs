using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Telepathy;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public float BulletSpeed=0;
    public float damage =0 ;
    public Targetable target;
    
    private void Move(Targetable Target)
    {
        if (target == null  ){NetworkServer.Destroy(this.gameObject);}
        transform.position = Vector3.MoveTowards(this.transform.position, Target.AimPoint().position, BulletSpeed);
        if (Vector3.Distance(transform.position, Target.AimPoint().position) < 0.2f)
        {
            Target.gameObject.GetComponent<Health>().TakeDamage(damage); 
            NetworkServer.Destroy(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        Move(target);
    }
}
