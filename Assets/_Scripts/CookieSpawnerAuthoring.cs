using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class CookieSpawnerAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject CookiePrefab;
    public float Counter;


    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        CookieSpawner spawner = new CookieSpawner();
        spawner.Counter = Counter;
        spawner.InitialCounter = Counter;
        spawner.Entity = conversionSystem.GetPrimaryEntity(CookiePrefab);

        dstManager.AddComponentData(entity, spawner);
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(CookiePrefab);
    }
}
