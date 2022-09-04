using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Shooter : NetworkBehaviour
{
    [SerializeField] private Transform BulletSpawn;
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private float BulletSpeed;
    [SerializeField] private float damage;
    [SerializeField] private Targeter targeter;
    [SerializeField] private float FireRate;
    [SerializeField] private float ShootingRange;
    [SerializeField] float ShootTimer;

    private void Start()
    {
        ShootTimer = 0;
    }

    private void ShootBullet()
    {
        var bullet = Instantiate(BulletPrefab, BulletSpawn.position, Quaternion.identity);
        NetworkServer.Spawn(bullet,connectionToClient);
        var newBullet =bullet.GetComponent<Bullet>();
        newBullet.BulletSpeed = BulletSpeed;
        newBullet.damage = damage;
        newBullet.target = targeter.GetTarget();

    }

    [ClientCallback]
    private void Update()
    {
        //ShootingTheBullet
        if (targeter.GetTarget() == null){return;}
        if (Vector3.Distance(targeter.GetTarget().transform.position,transform.position)> ShootingRange ) {return;}
        if (ShootTimer<Time.time)
        {
            Debug.Log("Shooting");
            ShootTimer = Time.time + FireRate;
            ShootBullet();
        }
    }
}
