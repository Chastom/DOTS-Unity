using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class Pistol : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        if (GunManager.instance.CurrentGun == Gun.Pistol && Input.GetMouseButtonDown(0))
        {
            UpdateAmmoText();

            Vector3 mousePosition = Input.mousePosition;
            Ray ray = GunManager.instance.Camera.ScreenPointToRay(mousePosition);
            float distanceToPlane;
            Vector3 hitPos = -Vector3.one;
            if (GunManager.instance.Plane.Raycast(ray, out distanceToPlane))
            {
                hitPos = ray.GetPoint(distanceToPlane);

                var bulletSpeed = 30;
                var bulletSpawnPos = GunManager.instance.Camera.transform.position;
                var moveDirection = (hitPos - GunManager.instance.Camera.transform.position).normalized;

                var ecbSystem = World.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
                var entityCommandBuffer = ecbSystem.CreateCommandBuffer();


                quaternion bulletRot = quaternion.LookRotation(moveDirection, Vector3.up);

                Entities.WithoutBurst().ForEach((ref BulletPrefabData bulletPrefabData, ref Translation translation) =>
                {
                    var instance = entityCommandBuffer.Instantiate(bulletPrefabData.Entity);

                    entityCommandBuffer.SetComponent(instance, new Translation { Value = bulletSpawnPos });
                    entityCommandBuffer.SetComponent(instance, new Rotation { Value = bulletRot });
                    entityCommandBuffer.AddComponent(instance, new BulletMove { MoveDirection = moveDirection, Speed = bulletSpeed });
                    entityCommandBuffer.AddComponent(instance, new BulletDamage { Damage = 2 });

                }).Run();
            }

            Debug.DrawLine(GunManager.instance.Camera.transform.position, hitPos, Color.red, 10f, false);

        }
        return inputDeps;
    }

    public void UpdateAmmoText()
    {
        PlayerDataSingletone.instance.UpdateAmmo("∞");
    }
}
