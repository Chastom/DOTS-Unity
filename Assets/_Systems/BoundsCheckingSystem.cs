using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using Unity.Transforms;

public class BoundsCheckingSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities.WithStructuralChanges().WithoutBurst().ForEach((Entity entity, ref Translation translation, in CookieTag cookieTag) => 
        {
            if (cookieTag.DestroyPosY > translation.Value.y)
            {
                EntityManager.DestroyEntity(entity);
                PlayerDataSingletone.instance.InflictDamage(cookieTag.Damage);
            }
        }).Run();

        return inputDeps;
    }
}
