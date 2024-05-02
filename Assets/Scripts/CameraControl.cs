using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform[] targets;
    public float baseHeight = 10f; 
    public float heightMultiplier = 0.75f; 
    public float smoothTime = 0.5f; 
    public float minY = 10f;
    public float minZoom = 40f; 
    public float maxZoom = 10f;
    public float zoomLimiter = 50f; 

    public float zoffset = -15; // added zoffset

    private Vector3 velocity;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (targets.Length == 0)
            return;

        Move();
        Zoom();
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        float heightAdjustment = CalculateHeightAdjustment();

        Vector3 newPosition = centerPoint + new Vector3(0, baseHeight + heightAdjustment, zoffset); // zoffset of -15 added
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, MaxDistanceFromCenter() / zoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }

    float CalculateHeightAdjustment()
    {
        float maxDistance = MaxDistanceFromCenter();
        return maxDistance * heightMultiplier; 
    }

    float MaxDistanceFromCenter()
    {
        float maxDistance = 0;
        Vector3 centerPoint = GetCenterPoint();
        foreach (Transform target in targets)
        {
            float distance = Vector3.Distance(centerPoint, target.position);
            maxDistance = Mathf.Max(maxDistance, distance);
        }
        return maxDistance;
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Length == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 1; i < targets.Length; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;
    }
}