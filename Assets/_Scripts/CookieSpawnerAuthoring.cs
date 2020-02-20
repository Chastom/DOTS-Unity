using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class CookieSpawnerAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject[] CookiePrefabs;

    public GameObject BossPrefab;


    public float SpawnTime;
    public float SpawnPosY;

    [Header("Wave variables")]
    public float WaveDuration = 4;
    public float WaveFrequency = 10;

    [Header("Boss variables")]
    public float BossSpawnFrequency = 10;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        CookieSpawnerData spawner = new CookieSpawnerData();

        // Timers
        spawner.CurrentSpawnTimer = SpawnTime;
        spawner.SpawnTime = SpawnTime;
        spawner.SpawnPosY = SpawnPosY;

        // Waves
        spawner.WaveSpawnActive = false;
        spawner.WaveDuration = WaveDuration;   //Increase this to increase the Difficuly
        spawner.WaveFrequency = WaveFrequency;

        // Boss
        spawner.BossSpawnFrequency = BossSpawnFrequency;

        // Prefabs to Entities
        spawner.cookieNormal = conversionSystem.GetPrimaryEntity(CookiePrefabs[0]);
        spawner.cookieFat = conversionSystem.GetPrimaryEntity(CookiePrefabs[1]);
        spawner.cookieFast = conversionSystem.GetPrimaryEntity(CookiePrefabs[2]);
        
        spawner.cookieMashineGun = conversionSystem.GetPrimaryEntity(CookiePrefabs[3]);
        spawner.cookieShotgun = conversionSystem.GetPrimaryEntity(CookiePrefabs[4]);

        spawner.cookieBoss = conversionSystem.GetPrimaryEntity(BossPrefab);


        dstManager.AddComponentData(entity, spawner);
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(BossPrefab);
        referencedPrefabs.AddRange(CookiePrefabs);

    }
}
