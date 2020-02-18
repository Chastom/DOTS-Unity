using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct CookieTag : IComponentData 
{
    public float DestroyPosY;
    public int Damage;
}
