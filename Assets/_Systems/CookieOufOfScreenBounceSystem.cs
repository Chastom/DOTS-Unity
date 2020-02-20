using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class CookieOutOfScreenBounceSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var deltaTime = Time.DeltaTime;

        var system = World.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
        var entityCommandBuffer = system.CreateCommandBuffer().ToConcurrent();

        var output = Entities.ForEach((ref CookieTag cookieTag, ref Translation translation, ref PhysicsVelocity physicsVelocity) =>
        {
            // Bounce on X axis
            if (translation.Value.x > 10)
                physicsVelocity.Linear = new float3(-3, physicsVelocity.Linear.y, physicsVelocity.Linear.z);

            if (translation.Value.x < -10)
                physicsVelocity.Linear = new float3(3, physicsVelocity.Linear.y, physicsVelocity.Linear.z);

            // Bounce on Y axis
            if (translation.Value.y > 15)
                physicsVelocity.Linear = new float3(physicsVelocity.Linear.x, -15, physicsVelocity.Linear.z);

            // Bounce on Z axis
            if (translation.Value.z > 5)
                physicsVelocity.Linear = new float3(physicsVelocity.Linear.x, physicsVelocity.Linear.y, -2);

            if (translation.Value.z < -5)
                physicsVelocity.Linear = new float3(physicsVelocity.Linear.x, physicsVelocity.Linear.y, 2);

            //translation.Value += bulletMove.MoveDirection * bulletMove.Speed * deltaTime;
        }).Schedule(inputDeps);

        output.Complete();

        system.AddJobHandleForProducer(output);

        return output;
    }
}
