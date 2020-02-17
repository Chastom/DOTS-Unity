using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = System.Random;

public class BulletSpawnerSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var deltaTime = Time.DeltaTime;
        Random r = new Random();

        var ecbSystem = World.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
        var entityCommandBuffer = ecbSystem.CreateCommandBuffer();

        Entities.WithoutBurst().ForEach((ref CookieSpawnerData cookieSpawner, ref Translation translation) =>
        {           
            cookieSpawner.Counter -= deltaTime;

            if (cookieSpawner.Counter <= 0)
            {
                var instance = entityCommandBuffer.Instantiate(cookieSpawner.Entity);

                var position = new float3(10 - (float)r.NextDouble() * 20, 10, 5);
                //var position = new float3(0, 10, 5);

                entityCommandBuffer.SetComponent(instance, new Translation { Value = position });

                cookieSpawner.Counter = cookieSpawner.InitialCounter;
            }

        }).Run();

        return inputDeps;
    }
}
