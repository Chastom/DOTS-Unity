using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public class CookieDestroyOnCollisionSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var ecbSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
        var entityCommandBuffer = ecbSystem.CreateCommandBuffer().ToConcurrent();

        var forEachHandler = Entities.WithAll<CookieTag>().ForEach((int entityInQueryIndex, Entity entity, ref HealthPoints healthPoints, ref CookieChangeWeapon changeWeapon) =>
        {
            if (healthPoints.Hp < 1)
            {
                entityCommandBuffer.DestroyEntity(entityInQueryIndex, entity);
                PlayerDataSingletone.instance.EnemiesKilled += 1;

                //if it's a weapon cookie, we change the current gun
                if (changeWeapon.ReceiveGun)
                {
                    ////it already has the same gun, so we need to reset its ammo
                    //Gun gun = GunManager.instance.CurrentGun;
                    //switch (gun)
                    //{
                    //    case Gun.MachineGun:
                    //        MachineGun.CurrentAmmo = MachineGun.InitialAmmo;
                    //        PlayerDataSingletone.instance.UpdateAmmo(MachineGun.CurrentAmmo + "/" + MachineGun.InitialAmmo);
                    //        break;
                    //    default:
                    //        break;
                    //}

                    GunManager.instance.ChangeGun(changeWeapon.GunOnDeath);
                }
            }


        }).Schedule(inputDeps);

        ecbSystem.AddJobHandleForProducer(forEachHandler);

        return forEachHandler;
    }
}