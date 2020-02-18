using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class CookieSpawnerAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject CookiePrefab;

    public float SpawnTime;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        CookieSpawnerData spawner = new CookieSpawnerData();
        spawner.CurrentSpawnTimer = SpawnTime;
        spawner.SpawnTime = SpawnTime;
        spawner.Entity = conversionSystem.GetPrimaryEntity(CookiePrefab);

        dstManager.AddComponentData(entity, spawner);

    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(CookiePrefab);
    }
}
