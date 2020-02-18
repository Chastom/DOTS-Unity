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
        Entities.WithStructuralChanges().WithoutBurst().ForEach((Entity entity, ref Translation translation, in CookieDeleteOutScreen cookieDeleteOutScreen) => 
        {
            if (cookieDeleteOutScreen.DestroyPosY > translation.Value.y)
            {
                EntityManager.DestroyEntity(entity);
                PlayerDataSingletone.instance.InflictDamage(cookieDeleteOutScreen.Damage);
            }
        }).Run();

        return inputDeps;
    }
}
