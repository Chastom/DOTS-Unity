using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct CookieSpawnerData : IComponentData
{
    public Entity Entity;
    public float Counter;
    public float InitialCounter;
}
