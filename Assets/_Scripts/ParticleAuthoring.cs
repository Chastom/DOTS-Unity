using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ParticleAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject ParticlePrefab;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        ParticlePrefabData data = new ParticlePrefabData();
        data.Entity = conversionSystem.GetPrimaryEntity(ParticlePrefab);
        dstManager.AddComponentData(entity, data);
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(ParticlePrefab);
    }
}
