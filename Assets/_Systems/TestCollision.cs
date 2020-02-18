using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public class TestCollision : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var ecbSystem = World.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
        var entityCommandBuffer = ecbSystem.CreateCommandBuffer().ToConcurrent();

        var forEachHandler = Entities.ForEach((int entityInQueryIndex, ref CollisionData collisionData) =>
        {
            if (collisionData.IsHit)
            {
                //destroy entity...
            }

        }).Schedule(inputDeps);

        ecbSystem.AddJobHandleForProducer(forEachHandler);

        return forEachHandler;
    }
}