using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct CookieSpawnerData : IComponentData
{
    public Entity cookieNormal;
    public Entity cookieFat;
    public Entity cookieFast;

    public Entity cookieMashineGun;
    public Entity cookieShotgun;

    public Entity cookieBoss;

    public bool IsBoosActive;
    public float BossSpawnFrequency;
    public float BossNextSpawnTime;

    public float CurrentSpawnTimer;
    public float SpawnTime;
    public float SpawnPosY;
}
