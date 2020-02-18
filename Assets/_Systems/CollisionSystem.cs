using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Mathematics;
using Unity.Burst;

public class CollisionSystem : JobComponentSystem
{
    private struct CollisionJob : ICollisionEventsJob {

        public ComponentDataFromEntity<CollisionData> collisionData;

        public void Execute(CollisionEvent triggerEvent)
        {
            Entity entityA = triggerEvent.Entities.EntityA;
            Entity entityB = triggerEvent.Entities.EntityB;
            if (collisionData.HasComponent(entityA) && collisionData.HasComponent(entityB))
            {
                CollisionData collisionData1 = collisionData[entityA];
                CollisionData collisionData2 = collisionData[entityB];

                collisionData1 = new CollisionData { IsHit = true, DestroyOnHit = collisionData1.DestroyOnHit };
                collisionData2 = new CollisionData { IsHit = true, DestroyOnHit = collisionData2.DestroyOnHit };

                collisionData[entityA] = collisionData1;
                collisionData[entityB] = collisionData2;
            }
        }

    }


    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;

    protected override void OnCreate()
    {
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        CollisionJob triggerJob = new CollisionJob
        {
            collisionData = GetComponentDataFromEntity<CollisionData>()
        };

        var output = triggerJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
        output.Complete();
        return output;
    }

}
