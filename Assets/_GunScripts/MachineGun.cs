using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class MachineGun : JobComponentSystem
{
    private float FireRate = 0.1f;
    private float ReloadTime = 0;
    private bool IsShooting = false;
    public static int InitialAmmo = 50;
    public static int CurrentAmmo;

    protected override void OnCreate()
    {
        CurrentAmmo = InitialAmmo;
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        if (Input.GetMouseButtonDown(0))
        {
            IsShooting = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            IsShooting = false;
        }

        if (GunManager.instance.CurrentGun == Gun.MachineGun && IsShooting && ReloadTime <= 0)
        {            
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = GunManager.instance.Camera.ScreenPointToRay(mousePosition);
            float distanceToPlane;
            Vector3 hitPos = -Vector3.one;
            if (GunManager.instance.Plane.Raycast(ray, out distanceToPlane))
            {
                hitPos = ray.GetPoint(distanceToPlane);

                var bulletSpeed = 40;
                var bulletSpawnPos = GunManager.instance.Camera.transform.position;
                bulletSpawnPos.y -= 1f;
                var moveDirection = (hitPos - bulletSpawnPos).normalized;

                var ecbSystem = World.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
                var entityCommandBuffer = ecbSystem.CreateCommandBuffer();


                quaternion bulletRot = quaternion.LookRotation(moveDirection, Vector3.up);

                Entities.WithoutBurst().ForEach((ref BulletPrefabData bulletPrefabData, ref Translation translation) =>
                {
                    var instance = entityCommandBuffer.Instantiate(bulletPrefabData.Entity);

                    entityCommandBuffer.SetComponent(instance, new Translation { Value = bulletSpawnPos });
                    entityCommandBuffer.SetComponent(instance, new Rotation { Value = bulletRot });
                    entityCommandBuffer.AddComponent(instance, new BulletMove { MoveDirection = moveDirection, Speed = bulletSpeed });
                    entityCommandBuffer.AddComponent(instance, new BulletDamage { Damage = 10 });

                }).Run();
            }

            Debug.DrawLine(GunManager.instance.Camera.transform.position, hitPos, Color.red, 10f, false);
            //after successful shot, we reset the reload time and remove ammo
            ReloadTime = FireRate;
            CurrentAmmo--;
            //if out of ammo, we reset the gun back to pistol, and reload ammo
            if (CurrentAmmo == 0)
            {
                GunManager.instance.ChangeGun(Gun.Pistol);
                CurrentAmmo = InitialAmmo;
            }
        }
        if (ReloadTime > 0)
        {
            ReloadTime -= Time.DeltaTime;
        }
        return inputDeps;
    }
}
