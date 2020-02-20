using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = System.Random;

public class CookieSpawnerSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var deltaTime = Time.DeltaTime;
        Random r = new Random();
        var randomPosRatio = r.NextDouble();
        var randomCookieIndex = r.Next(5);

        bool testIsInputPressed = Input.GetKeyDown(KeyCode.Space);

        var ecbSystem = World.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
        var entityCommandBuffer = ecbSystem.CreateCommandBuffer();

        Entities.WithoutBurst().ForEach((ref CookieSpawnerData cookieSpawner) =>
        {
            
            if (testIsInputPressed)
            {
                cookieSpawner.IsBossActive = !cookieSpawner.IsBossActive;
            }

            // Modify timers
            cookieSpawner.CurrentSpawnTimer -= deltaTime;   // Decrease single cookie spawn timer
            cookieSpawner.WaveTimer += deltaTime;           // Decrease Wave timer

            if (!cookieSpawner.IsBossActive)
                cookieSpawner.BossNextSpawnTimer += deltaTime;           // Decrease Wave timer



            // Make sure the Duration is not longer than the whole Wave Frequency
            if (cookieSpawner.WaveDuration > cookieSpawner.WaveFrequency)
            {
                Debug.Log("Wave Duration is larger than Wave Frequency. All Good, fixed");
                cookieSpawner.WaveDuration = cookieSpawner.WaveFrequency;
            }

            // Wave Logic
            if (cookieSpawner.WaveTimer < cookieSpawner.WaveDuration)
            {
                cookieSpawner.WaveSpawnActive = true;
            } else if (cookieSpawner.WaveTimer < cookieSpawner.WaveFrequency)
            {
                cookieSpawner.WaveSpawnActive = false;
            } else
            {
                if (!cookieSpawner.IsBossActive)
                    cookieSpawner.WaveTimer = 0;
            }

            // Boss Logic
            if (cookieSpawner.BossNextSpawnTimer >= cookieSpawner.BossSpawnFrequency)
            {
                Debug.Log("Spawning boss");
                var instance = entityCommandBuffer.Instantiate(cookieSpawner.cookieBoss);
                var bossPos = new float3(0, cookieSpawner.SpawnPosY, 5);
                entityCommandBuffer.SetComponent(instance, new Translation { Value = bossPos });


                cookieSpawner.IsBossActive = true;
                cookieSpawner.BossNextSpawnTimer = 0;
            }



            // Spawn logic
            if (cookieSpawner.CurrentSpawnTimer <= 0 && cookieSpawner.WaveSpawnActive && !cookieSpawner.IsBossActive)
            {
                Entity instance = new Entity();

                switch (randomCookieIndex)
                {
                    case 0: instance = entityCommandBuffer.Instantiate(cookieSpawner.cookieNormal); break;
                    case 1: instance = entityCommandBuffer.Instantiate(cookieSpawner.cookieFat); break;
                    case 2: instance = entityCommandBuffer.Instantiate(cookieSpawner.cookieFast); break;
                    case 3: instance = entityCommandBuffer.Instantiate(cookieSpawner.cookieMashineGun); break;
                    case 4: instance = entityCommandBuffer.Instantiate(cookieSpawner.cookieShotgun); break;

                    default: instance = entityCommandBuffer.Instantiate(cookieSpawner.cookieNormal); Debug.Log("Bad!@!@!@!@ Wrong cookie index"); break;

                }

                var position = new float3(10 - (float)randomPosRatio * 20, cookieSpawner.SpawnPosY, 5);
                entityCommandBuffer.SetComponent(instance, new Translation { Value = position });

                cookieSpawner.CurrentSpawnTimer = cookieSpawner.SpawnTime;
            }

           

        }).Run();

        return inputDeps;
    }
}
