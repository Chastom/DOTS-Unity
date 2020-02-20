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
    private readonly float spreadRatio = 0.25f;

    Random r = new Random();
    protected override void OnCreate()
    {
        CurrentAmmo = InitialAmmo;
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

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
                var moveDirection = (hitPos - GunManager.instance.Camera.transform.position).normalized;

                var ecbSystem = World.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
                var entityCommandBuffer = ecbSystem.CreateCommandBuffer();


                quaternion bulletRot = quaternion.LookRotation(moveDirection, Vector3.up);

                Entities.WithoutBurst().ForEach((ref BulletPrefabData bulletPrefabData, ref Translation translation) =>
                {
                    for (int x = 0; x < 100; x++)
                    {
                        var instance = entityCommandBuffer.Instantiate(bulletPrefabData.Entity);
                        var offset = new Vector3((-0.5f+(float)r.NextDouble()) * spreadRatio, (-0.5f+(float)r.NextDouble())*spreadRatio, 0);
                        entityCommandBuffer.SetComponent(instance, new Translation { Value = bulletSpawnPos });
                        entityCommandBuffer.SetComponent(instance, new Rotation { Value = bulletRot });
                        //entityCommandBuffer.SetComponent(instance, new HealthPoints { Hp = 9000 });
                        entityCommandBuffer.AddComponent(instance, new Scale { Value = 0.15f });
                        entityCommandBuffer.AddComponent(instance, new BulletMove { MoveDirection = moveDirection+offset, Speed = bulletSpeed });
                        entityCommandBuffer.AddComponent(instance, new BulletDamage { Damage = 1 });
                    }

                }).Run();
            }

            Debug.DrawLine(GunManager.instance.Camera.transform.position, hitPos, Color.red, 10f, false);
            //after successful shot, we reset the reload time and remove ammo
            ReloadTime = FireRate;
            CurrentAmmo--;
            PlayerDataSingletone.instance.UpdateAmmo(CurrentAmmo + "/" + InitialAmmo); //Updates Ammo Text
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

    public void UpdateAmmoText()
    {
        PlayerDataSingletone.instance.UpdateAmmo(CurrentAmmo + "/" + InitialAmmo);
    }
}
