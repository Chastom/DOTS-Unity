using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public class GunSystem : JobComponentSystem
{
    private Camera Camera;
    private Plane Plane;

    protected override void OnCreate()
    {
        Camera = Camera.main;
        Plane = new Plane(Vector3.forward, -5f);
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.ScreenPointToRay(mousePosition);
            float distanceToPlane;
            Vector3 currentPosition = -Vector3.one;
            if (Plane.Raycast(ray, out distanceToPlane))
            {
                currentPosition = ray.GetPoint(distanceToPlane);
            }
            Debug.Log(distanceToPlane.ToString()+ " | X:" + currentPosition.x + " | Y:" +  currentPosition.y);
            Debug.DrawLine(Camera.transform.position,
            currentPosition,
            Color.red,
            10f,
            false);
        }

        return inputDeps;
    }
}
