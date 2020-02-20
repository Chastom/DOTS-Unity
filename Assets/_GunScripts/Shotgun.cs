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
    private float FireRate = 0.4f;
    private float ReloadTime = 0;
    public static int InitialAmmo = 10;
    public static int CurrentAmmo;
    //private readonly float spreadRatio = 1f;

    public static int Radius = 25;
    private List<Vector2i> Offsets;

    protected override void OnCreate()
    {
        CurrentAmmo = InitialAmmo;
        Offsets = new List<Vector2i>();
        int threshold = Radius * Radius;
        for (int i = -Radius; i < Radius; i++)
        {
            for (int j = -Radius; j < Radius; j++)
            {
                if (i * i + j * j < threshold)
                    Offsets.Add(new Vector2i(i, j));
            }
        }
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

                var bulletSpawnPos = GunManager.instance.Camera.transform.position;
                bulletSpawnPos.y -= 1f;
                var moveDirection = (hitPos - bulletSpawnPos).normalized;

                var ecbSystem = World.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
                var entityCommandBuffer = ecbSystem.CreateCommandBuffer();


                quaternion bulletRot;

                Random r = new Random();

                Entities.WithoutBurst().ForEach((ref BulletPrefabData bulletPrefabData, ref Translation translation) =>
                {
                    float3 temp;
                    float3 temp2;
                    for (int i = 0; i < Offsets.Count; i++)
                    {
                        var randomSpawn = (float)r.NextDouble();
                        if (randomSpawn <= 0.95f)
                            continue;

                        var offsetStr = 5;

                        temp = moveDirection;
                        temp.x += Offsets[i].x / 85;
                        temp.y += Offsets[i].y / 85;

                        bulletRot = quaternion.LookRotation(temp, Vector3.up);

                        //temp = hitPos;
                        //temp2 = hitPos;
                        //temp2.z += 10;
                        //temp.x += Offsets[i].x / 30;
                        //temp.y += Offsets[i].y / 30;
                        //var dir = temp2 - new float3(0, 0, -1) - temp;

                        //var randomOffset = new float3((0.5f - (float)r.NextDouble()) * offsetStr, (0.5f - (float)r.NextDouble()) * offsetStr, ((float)r.NextDouble()) * offsetStr);

                        var s = 0.2f;
                        float4x4 newScale = new float4x4(
                            s, 0, 0, 0, 
                            0, s, 0, 0, 
                            0, 0, s, 0, 
                            0, 0, 0, 1);
                        var instance = entityCommandBuffer.Instantiate(bulletPrefabData.Entity);
                        entityCommandBuffer.SetComponent(instance, new Translation { Value = bulletSpawnPos });
                        entityCommandBuffer.AddComponent(instance, new CompositeScale { Value = newScale });
                        entityCommandBuffer.AddComponent(instance, new BulletMove { MoveDirection = temp, Speed = 30f });
                        entityCommandBuffer.SetComponent(instance, new Rotation { Value = bulletRot });
                        entityCommandBuffer.AddComponent(instance, new BulletDamage { Damage = 5 });
                    }                    

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
        return returnDeps;
    }


    public struct Vector2i
    {
        public float x;
        public float y;
        public Vector2i(float aX, float aY)
        {
            x = aX; y = aY;
        }
    }
}
