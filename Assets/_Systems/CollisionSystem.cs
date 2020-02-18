using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics.Systems;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unity.Physics.Tests
{
    [UpdateBefore(typeof(StepPhysicsWorld))]
    public class CollisionSystem : JobComponentSystem
    {
        BuildPhysicsWorld m_BuildPhysicsWorldSystem;
        StepPhysicsWorld m_StepPhysicsWorldSystem;
        NativeList<CollisionDetails> collisionDetails = new NativeList<CollisionDetails>(1, Allocator.Persistent);

        //static ComponentDataFromEntity<CollisionData> myTypeFromEntity;

        protected override void OnCreate()
        {
            m_StepPhysicsWorldSystem = World.GetOrCreateSystem<StepPhysicsWorld>();
            m_BuildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
            //myTypeFromEntity = GetComponentDataFromEntity<CollisionData>(true);
        }

        public struct CollisionJob : ICollisionEventsJob
        {
            //public ComponentDataFromEntity<CollisionData> collisionData;
            //public ComponentDataFromEntity<CollisionData> TestGettingData;
            public void Execute(CollisionEvent collisionEvent)
            {

                Entity entityA = collisionEvent.Entities.EntityA;
                Entity entityB = collisionEvent.Entities.EntityB;
                Debug.Log("IS HIT!!@!@!@!@");
                //if (TestGettingData.Exists(entityA) && TestGettingData.Exists(entityB))
                //{
                //    //CollisionData collisionData1 = myTypeFromEntity[entityA];
                //    //CollisionData collisionData2 = myTypeFromEntity[entityB];
                //    //collisionData1.IsHit = true;
                //    //collisionData2.IsHit = true;
                //    CollisionData temp = new CollisionData { IsHit = false };
                //    TestGettingData[entityA] = temp;
                //    Debug.Log("IS HIT!!@!@!@!@");
                //}

            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            m_BuildPhysicsWorldSystem.FinalJobHandle.Complete();

            SimulationCallbacks.Callback testCollisionEventCallback = (ref ISimulation simulation, ref PhysicsWorld world, JobHandle inDeps) =>
            {
                var job = new CollisionJob
                {
                    //World = m_BuildPhysicsWorldSystem.PhysicsWorld,
                    //CollisionDetails = collisionDetails,
                }.Schedule(simulation, ref world, inDeps);

                job.Complete();

                foreach (CollisionDetails details in collisionDetails)
                {
                    //Debug.Log("collision at: " + details.ImpulsePoint);
                }

                return job;
            };
            m_StepPhysicsWorldSystem.EnqueueCallback(SimulationCallbacks.Phase.PostSolveJacobians, testCollisionEventCallback, inputDeps);

            return inputDeps;
        }

        protected override void OnDestroy()
        {
            collisionDetails.Dispose();
        }
        public struct CollisionDetails
        {
            public float ImpulseMagnitude;
            public float3 ImpulseDirection;
            public float3 ImpulsePoint;
        }
    }
}