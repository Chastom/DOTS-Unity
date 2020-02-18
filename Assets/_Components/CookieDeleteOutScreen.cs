using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct CookieDeleteOutScreen : IComponentData 
{
    public float DestroyPosY;
    public int Damage;
}
