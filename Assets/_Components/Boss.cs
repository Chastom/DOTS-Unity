using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct Boss : IComponentData
{
    public float DropSpeed;
    public float SideToSideSpeed;

    public bool IsMovingRight;
}
