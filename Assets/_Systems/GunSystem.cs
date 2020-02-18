using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class GunSystem : JobComponentSystem
{
    private Camera Camera;
    private Plane Plane;

    protected override void OnCreate()
    {
        Camera = Camera.main;
        Plane = new Plane(Vector3.forward, -5f);
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.ScreenPointToRay(mousePosition);
            float distanceToPlane;
            Vector3 hitPos = -Vector3.one;
            if (Plane.Raycast(ray, out distanceToPlane))
            {
                hitPos = ray.GetPoint(distanceToPlane);

                var bulletSpeed = 30;
                var bulletSpawnPos = Camera.transform.position;
                var moveDirection = (hitPos - Camera.transform.position).normalized;
   
                var ecbSystem = World.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
                var entityCommandBuffer = ecbSystem.CreateCommandBuffer();

                quaternion bulletRot = quaternion.LookRotation(moveDirection, Vector3.up);

                Entities.WithoutBurst().ForEach((ref BulletPrefabData bulletPrefabData, ref Translation translation) =>
                {
                    var instance = entityCommandBuffer.Instantiate(bulletPrefabData.Entity);


                    entityCommandBuffer.SetComponent(instance, new Translation { Value = bulletSpawnPos });
                    entityCommandBuffer.SetComponent(instance, new Rotation { Value = bulletRot });
                    entityCommandBuffer.AddComponent(instance, new BulletMove { MoveDirection = moveDirection, Speed = bulletSpeed });
                    entityCommandBuffer.AddComponent(instance, new CollisionData { IsHit = false });

                }).Run();

            }

            //Debug.Log(distanceToPlane.ToString()+ " | X:" + hitPos.x + " | Y:" +  hitPos.y);
            Debug.DrawLine(Camera.transform.position, hitPos, Color.red, 10f, false);

        }

        return inputDeps;
    }
}
