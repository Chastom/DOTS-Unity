//using System.Collections;
//using System.Collections.Generic;
//using Unity.Entities;
//using Unity.Physics;
//using UnityEngine;

//public class TestOnCollisionEnter : ICollisionEventsJob
//{
//    public ComponentDataFromEntity<CollisionData> CollisionData;
//    public void Execute(CollisionEvent collisionEvent)
//    {
//        var entityA = collisionEvent.Entities.EntityA;
//        var entityB = collisionEvent.Entities.EntityB;
//        Debug.Log("IS HIT!!@!@!@!@");
//        if (CollisionData.Exists(entityA) && CollisionData.Exists(entityB))
//        {
//            CollisionData collisionData1 = CollisionData[entityA];
//            CollisionData collisionData2 = CollisionData[entityB];
//            collisionData1.IsHit = true;
//            collisionData2.IsHit = true;
//            Debug.Log("IS HIT!!@!@!@!@");
//        }
//    }
//}
