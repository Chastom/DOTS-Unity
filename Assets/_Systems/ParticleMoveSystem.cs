using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Physics;

public class ParticleMoveSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var deltaTime = Time.DeltaTime;

        var system = World.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
        var entityCommandBuffer = system.CreateCommandBuffer().ToConcurrent();

        var output = Entities.ForEach((ref ParticleMove particleMove, ref Translation translation, ref PhysicsVelocity velocity) =>
        {
            //translation.Value += particleMove.MoveDirection * particleMove.Speed * deltaTime;
            velocity.Linear = particleMove.MoveDirection * particleMove.Speed;
            if (translation.Value.z < 0)
            {
                //destroy when too close to the camera
            }

        }).Schedule(inputDeps);

        output.Complete();

        system.AddJobHandleForProducer(output);

        return output;
    }
}
