using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class CookieSpawnerAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject[] CookiePrefabs;

    public float SpawnTime;
    public float SpawnPosY;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        CookieSpawnerData spawner = new CookieSpawnerData();
        spawner.CurrentSpawnTimer = SpawnTime;
        spawner.SpawnTime = SpawnTime;
        spawner.SpawnPosY = SpawnPosY;

        spawner.cookieNormal = conversionSystem.GetPrimaryEntity(CookiePrefabs[0]);
        spawner.cookieFat = conversionSystem.GetPrimaryEntity(CookiePrefabs[1]);
        spawner.cookieFast = conversionSystem.GetPrimaryEntity(CookiePrefabs[2]);
        
        spawner.cookieMashineGun = conversionSystem.GetPrimaryEntity(CookiePrefabs[3]);
        spawner.cookieShotgun = conversionSystem.GetPrimaryEntity(CookiePrefabs[4]);

        dstManager.AddComponentData(entity, spawner);


    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.AddRange(CookiePrefabs);
    }
}
