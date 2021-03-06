﻿using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public class BulletDestroyOnCollisionSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var ecbSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
        var entityCommandBuffer = ecbSystem.CreateCommandBuffer().ToConcurrent();

        var forEachHandler = Entities.WithAll<BulletTag>().ForEach((int entityInQueryIndex, Entity entity, ref HealthPoints healthPoints) =>
        {
            if (healthPoints.Hp < 1 && healthPoints.DeleteOnLowHp)
            {
                entityCommandBuffer.DestroyEntity(entityInQueryIndex, entity);
            }


        }).Schedule(inputDeps);

        ecbSystem.AddJobHandleForProducer(forEachHandler);

        return forEachHandler;
    }
}