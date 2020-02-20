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
        public ComponentDataFromEntity<BulletDamage> dmg;

        public void Execute(CollisionEvent triggerEvent)
        {
            Entity entityA = triggerEvent.Entities.EntityA;
            Entity entityB = triggerEvent.Entities.EntityB;

            BulletDamage bulletDamage = new BulletDamage { Damage = 0};

            if (bullet.HasComponent(entityA) || bullet.HasComponent(entityB))
            {
                //checking which entity is a bullet
                if (bullet.HasComponent(entityA))
                {
                    bulletDamage = dmg[entityA];
                }
                else if (bullet.HasComponent(entityB))
                {
                    bulletDamage = dmg[entityB];
                }
                // two bullets collided, we don't do any damage to them.
                if (bullet.HasComponent(entityA) && bullet.HasComponent(entityB))
                {                    
                    bulletDamage.Damage = 0;
                }


                HealthPoints coll1 = collisionData[entityA];
                HealthPoints coll2 = collisionData[entityB];

                coll1 = new HealthPoints { Hp = coll1.Hp - bulletDamage.Damage };
                coll2 = new HealthPoints { Hp = coll2.Hp - bulletDamage.Damage };

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
            bullet = GetComponentDataFromEntity<BulletTag>(),
            dmg = GetComponentDataFromEntity<BulletDamage>()
        };

        var output = triggerJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
        output.Complete();
        return output;
    }

}
