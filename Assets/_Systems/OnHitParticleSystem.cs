using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = System.Random;

public class OnHitParticleSystem : JobComponentSystem
{
    public static bool ShouldSpawn = false;
    public static float3 Position;
    public static int Radius = 20;

    private List<Vector2i> Offsets;

    protected override void OnCreate()
    {
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
        if (ShouldSpawn)
        {
            var ecbSystem = World.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
            var entityCommandBuffer = ecbSystem.CreateCommandBuffer();

            Random r = new Random();

            Entities.WithoutBurst().ForEach((ref ParticlePrefabData particlePrefab, ref Translation translation) =>
            {
                float3 temp;
                float3 temp2;
                for (int i = 0; i < Offsets.Count; i++)
                {
                    var randomSpawn = (float)r.NextDouble();
                    if (randomSpawn <= 0.95f)
                        continue;

                    var offsetStr = 20;

                    temp = Position;
                    temp2 = Position;
                    temp2.z += 10;
                    temp.x += Offsets[i].x / 40;
                    temp.y += Offsets[i].y / 40;
                    var dir = temp2 - new float3(0, 0, -1) - temp;

                    var randomOffset = new float3((0.5f - (float)r.NextDouble()) * offsetStr, (0.5f - (float)r.NextDouble()) * offsetStr, ((float)r.NextDouble()) * offsetStr);

                    var instance = entityCommandBuffer.Instantiate(particlePrefab.Entity);
                    entityCommandBuffer.SetComponent(instance, new Translation { Value = temp });
                    entityCommandBuffer.AddComponent(instance, new ParticleMove { MoveDirection = -dir + randomOffset, Speed = 0.1f });
                }

            }).Run();
            ShouldSpawn = false;
        }

        return inputDeps;
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
