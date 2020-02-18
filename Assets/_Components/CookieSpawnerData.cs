using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct CookieSpawnerData : IComponentData
{
    public Entity Entity;
    public float CurrentSpawnTimer;
    public float SpawnTime;
}
