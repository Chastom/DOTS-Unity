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
        var randomCookieIndex = r.Next(3);


        var ecbSystem = World.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
        var entityCommandBuffer = ecbSystem.CreateCommandBuffer();

        Entities.WithoutBurst().ForEach((ref CookieSpawnerData cookieSpawner) =>
        { 
            cookieSpawner.CurrentSpawnTimer -= deltaTime;

            if (cookieSpawner.CurrentSpawnTimer <= 0)
            {
                Entity instance = new Entity();

                Debug.Log("randomCookieIndex: " + randomCookieIndex);

                switch (randomCookieIndex)
                {
                    case 0: instance = entityCommandBuffer.Instantiate(cookieSpawner.cookieNormal); break;
                    case 1: instance = entityCommandBuffer.Instantiate(cookieSpawner.cookieFat); break;
                    case 2: instance = entityCommandBuffer.Instantiate(cookieSpawner.cookieFast); break;

                    default: instance = entityCommandBuffer.Instantiate(cookieSpawner.cookieNormal); Debug.Log("Bad!@!@!@!@ Wrong cookie index"); break;

                }

                var position = new float3(10 - (float)randomPosRatio * 20, 10, 5);
                entityCommandBuffer.SetComponent(instance, new Translation { Value = position });

                cookieSpawner.CurrentSpawnTimer = cookieSpawner.SpawnTime;
            }

        }).Run();

        return inputDeps;
    }
}
