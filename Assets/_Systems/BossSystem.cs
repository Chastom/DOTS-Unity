using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using Unity.Transforms;

public class BossSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var deltaTime = Time.DeltaTime;
        var bossKilled = false;
        var switchLimit = -1;

        Entities.WithStructuralChanges().WithoutBurst().ForEach((Entity entity, ref Boss boss, ref Translation translation, ref HealthPoints hp) => 
        {
            if(hp.Hp < 1)
            {
                bossKilled = true;
            }
            if (translation.Value.y > switchLimit)
            {
                translation.Value.y -= boss.DropSpeed * deltaTime;
            }
            else
            {
                translation.Value.y = switchLimit;
                if (boss.IsMovingRight)
                {
                    translation.Value.x += boss.SideToSideSpeed * deltaTime;
                    if (translation.Value.x > 15)
                        boss.IsMovingRight = !boss.IsMovingRight;
                } else
                {
                    translation.Value.x -= boss.SideToSideSpeed * deltaTime;
                    if (translation.Value.x < -15)
                        boss.IsMovingRight = !boss.IsMovingRight;
                }
            }
        }).Run();

        if (bossKilled)
        {
            Entities.WithStructuralChanges().WithoutBurst().ForEach((Entity entity, ref CookieSpawnerData cookieSpawnerData) =>
            {
                cookieSpawnerData.IsBossActive = false;
            }).Run();

            Entities.WithStructuralChanges().WithoutBurst().ForEach((Entity entity, ref Boss boss) =>
            {
                EntityManager.DestroyEntity(entity);
            }).Run();

        }




        return inputDeps;
    }
}
