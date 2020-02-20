using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class OnHitParticleSystem : JobComponentSystem
{
    public static bool ShouldSpawn = false;
    public static float3 Translation;

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var a = new Unity.Mathematics.Random();
        

        if (ShouldSpawn)
        {
            var ecbSystem = World.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
            var entityCommandBuffer = ecbSystem.CreateCommandBuffer();
            Entities.WithoutBurst().ForEach((ref ParticlePrefabData particlePrefab, ref Translation translation) =>
            {



                for (int i = 0; i < 20; i++)
                {

                    //var pos = new float2(0, 0);
                    //var dir = hitPoint - pos;


                    var instance = entityCommandBuffer.Instantiate(particlePrefab.Entity);
                    entityCommandBuffer.SetComponent(instance, new Translation { Value = Translation });
                }
                //entityCommandBuffer.AddComponent(instance, new BulletMove { MoveDirection = moveDirection, Speed = bulletSpeed });
            }).Run();
            ShouldSpawn = false;
        }

        return inputDeps;
    }
}
