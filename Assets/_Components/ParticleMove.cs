﻿using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct ParticleMove : IComponentData
{
    public float Speed;
    public float3 MoveDirection;
}
