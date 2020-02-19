using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class BulletOutOfBoundsDestorySystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var system = World.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
        var entityCommandBuffer = system.CreateCommandBuffer().ToConcurrent();

        var output = Entities.ForEach((int entityInQueryIndex, ref Entity entity, ref BulletOutOfBoundsDestroy bulletOutOfBoundsDestroy, ref Translation translation) =>
        {
            if (translation.Value.z > bulletOutOfBoundsDestroy.DestroyOffsetZ)
                entityCommandBuffer.DestroyEntity(entityInQueryIndex, entity);

        }).Schedule(inputDeps);

        output.Complete();

        system.AddJobHandleForProducer(output);

        return output;
    }
}
