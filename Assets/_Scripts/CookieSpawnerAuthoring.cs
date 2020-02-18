using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class CookieSpawnerAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject CookiePrefab;
    public float Counter;
    public float CookieDestroyPosY;
    public int CookieDamage;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        CookieSpawnerData spawner = new CookieSpawnerData();
        spawner.Counter = Counter;
        spawner.InitialCounter = Counter;
        spawner.DestroyPosY = CookieDestroyPosY;
        spawner.Damage = CookieDamage;
        spawner.Entity = conversionSystem.GetPrimaryEntity(CookiePrefab);

        dstManager.AddComponentData(entity, spawner);
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(CookiePrefab);
    }
}
