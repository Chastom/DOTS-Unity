using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

//[GenerateAuthoringComponent]
public struct BulletMove : IComponentData
{
    public float Speed;
    public float3 MoveDirection;
}
