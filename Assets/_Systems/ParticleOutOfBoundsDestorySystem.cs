using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ParticleOutOfBoundsDestorySystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var system = World.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
        var entityCommandBuffer = system.CreateCommandBuffer().ToConcurrent();

        var output = Entities.WithAll<ParticleTag>().ForEach((int entityInQueryIndex, ref Entity entity, ref Translation translation) =>
        {
            if (translation.Value.y < -5)
                entityCommandBuffer.DestroyEntity(entityInQueryIndex, entity);

        }).Schedule(inputDeps);

        output.Complete();

        system.AddJobHandleForProducer(output);

        return output;
    }
}
