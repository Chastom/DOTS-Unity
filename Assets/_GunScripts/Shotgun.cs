using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = System.Random;
public class Shotgun : JobComponentSystem
{
    private float FireRate = 0.75f;
    private float ReloadTime = 0;
    public static int InitialAmmo = 10;
    public static int CurrentAmmo;
    private readonly float spreadRatio = 1f;

    private readonly Random r = new Random();
    protected override void OnCreate()
    {
        CurrentAmmo = InitialAmmo;
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        JobHandle returnDeps = inputDeps;
        if (GunManager.instance.CurrentGun == Gun.Shotgun && Input.GetMouseButtonDown(0) && ReloadTime <= 0)
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = GunManager.instance.Camera.ScreenPointToRay(mousePosition);
            float distanceToPlane;
            Vector3 hitPos = -Vector3.one;
            if (GunManager.instance.Plane.Raycast(ray, out distanceToPlane))
            {
                hitPos = ray.GetPoint(distanceToPlane);

                var bulletSpeed = 25;
                var bulletSpawnPos = GunManager.instance.Camera.transform.position;
                bulletSpawnPos.y -= 0.33f;
                var moveDirection = (hitPos - GunManager.instance.Camera.transform.position).normalized;

                var ecbSystem = World.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
                var entityCommandBuffer = ecbSystem.CreateCommandBuffer().ToConcurrent();


                quaternion bulletRot = quaternion.LookRotation(moveDirection, Vector3.up);
                float randomNumber = 0.5f - (float)r.NextDouble();
                float spreadRatioTemp = spreadRatio;
                returnDeps = Entities.WithoutBurst().ForEach((int entityInQueryIndex, ref BulletPrefabData bulletPrefabData, ref Translation translation) =>
                {
                    for (int x = -5; x < 5; x++)
                    {
                        for (int y = -5; y < 5; y++)
                        {
                            var instance = entityCommandBuffer.Instantiate(entityInQueryIndex, bulletPrefabData.Entity);
                            var offset = new Vector3(spreadRatioTemp * x / -5.0f, spreadRatioTemp * y / 5.0f, 0);
                            entityCommandBuffer.SetComponent(entityInQueryIndex, instance, new Translation { Value = bulletSpawnPos+offset });
                            entityCommandBuffer.SetComponent(entityInQueryIndex, instance, new Rotation { Value = bulletRot });
                            entityCommandBuffer.AddComponent(entityInQueryIndex, instance, new Scale { Value = 0.2f });
                            entityCommandBuffer.AddComponent(entityInQueryIndex, instance, new BulletMove { MoveDirection = moveDirection, Speed = bulletSpeed });
                            entityCommandBuffer.AddComponent(entityInQueryIndex, instance, new BulletDamage { Damage = 1 });
                        }
                    }

                }).Schedule(inputDeps);
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
        return returnDeps;
    }

}
