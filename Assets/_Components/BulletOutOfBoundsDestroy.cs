﻿using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct BulletOutOfBoundsDestroy : IComponentData
{
    public float DestroyOffsetZ;
}
