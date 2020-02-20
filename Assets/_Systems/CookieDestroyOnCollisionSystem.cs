using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public class CookieDestroyOnCollisionSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var ecbSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
        var entityCommandBuffer = ecbSystem.CreateCommandBuffer().ToConcurrent();

        var forEachHandler = Entities.WithAll<CookieTag>().ForEach((int entityInQueryIndex, Entity entity, ref HealthPoints healthPoints) =>
        {
            if (healthPoints.Hp < 1)
            {
                entityCommandBuffer.DestroyEntity(entityInQueryIndex, entity);
                PlayerDataSingletone.instance.EnemiesKilled += 1;
            }


        }).Schedule(inputDeps);

        ecbSystem.AddJobHandleForProducer(forEachHandler);

        return forEachHandler;
    }
}