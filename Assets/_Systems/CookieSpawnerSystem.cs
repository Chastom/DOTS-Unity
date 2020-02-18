using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = System.Random;

public class CookieSpawnerSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var deltaTime = Time.DeltaTime;
        Random r = new Random();
        var randomPosRatio = r.NextDouble();

        var ecbSystem = World.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
        var entityCommandBuffer = ecbSystem.CreateCommandBuffer();

        Entities.WithoutBurst().ForEach((ref CookieSpawnerData cookieSpawner, ref Translation translation) =>
        {           
            cookieSpawner.CurrentSpawnTimer -= deltaTime;

            if (cookieSpawner.CurrentSpawnTimer <= 0)
            {
                var instance = entityCommandBuffer.Instantiate(cookieSpawner.Entity);

                var position = new float3(10 - (float)randomPosRatio * 20, 10, 5);
                entityCommandBuffer.SetComponent(instance, new Translation { Value = position });

                cookieSpawner.CurrentSpawnTimer = cookieSpawner.SpawnTime;
            }

        }).Run();

        return inputDeps;
    }
}
