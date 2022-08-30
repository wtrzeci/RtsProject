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
    [SerializeField] private Targetable target;
    [SerializeField] private float FireRate;
    [SerializeField] private float TurnRate;

}
