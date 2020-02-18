using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class CookieSpawnerAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject[] CookiePrefabs;

    public float SpawnTime;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        CookieSpawnerData spawner = new CookieSpawnerData();
        spawner.CurrentSpawnTimer = SpawnTime;
        spawner.SpawnTime = SpawnTime;

        spawner.cookieNormal = conversionSystem.GetPrimaryEntity(CookiePrefabs[0]);
        spawner.cookieFat = conversionSystem.GetPrimaryEntity(CookiePrefabs[1]);
        spawner.cookieFast = conversionSystem.GetPrimaryEntity(CookiePrefabs[2]);


        dstManager.AddComponentData(entity, spawner);
        
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.AddRange(CookiePrefabs);
    }
}
