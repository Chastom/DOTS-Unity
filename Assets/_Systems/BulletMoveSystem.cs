using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class BulletMoveSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        /* 
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
        */

        var deltaTime = Time.DeltaTime;

        var system = World.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
        var entityCommandBuffer = system.CreateCommandBuffer().ToConcurrent();

        var output = Entities.ForEach((ref BulletMove bulletMove, ref Translation translation) =>
        {
            translation.Value += bulletMove.MoveDirection * bulletMove.Speed * deltaTime;

        }).Schedule(inputDeps);

        output.Complete();

        system.AddJobHandleForProducer(output);

        return output;
    }
}
