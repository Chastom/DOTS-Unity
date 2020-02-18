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
    private struct CollisionJob : ICollisionEventsJob
    {
        public ComponentDataFromEntity<HealthPoints> collisionData;
        public ComponentDataFromEntity<BulletTag> bullet;

        public void Execute(CollisionEvent triggerEvent)
        {
            Entity entityA = triggerEvent.Entities.EntityA;
            Entity entityB = triggerEvent.Entities.EntityB;
            if (bullet.HasComponent(entityA) || bullet.HasComponent(entityB))
            {
                HealthPoints coll1 = collisionData[entityA];
                HealthPoints coll2 = collisionData[entityB];

                coll1 = new HealthPoints { Hp = coll1.Hp - 1 };
                coll2 = new HealthPoints { Hp = coll2.Hp - 1 };
                //Debug.Log("Object 1 -> " + coll1.HealthPoints + " | Object 2 -> " + coll2.HealthPoints);

                collisionData[entityA] = coll1;
                collisionData[entityB] = coll2;
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
            collisionData = GetComponentDataFromEntity<HealthPoints>(),
            bullet = GetComponentDataFromEntity<BulletTag>()
        };

        var output = triggerJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
        output.Complete();
        return output;
    }

}
