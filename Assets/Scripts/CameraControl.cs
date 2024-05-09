using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform[] targets;
    public float baseHeight = 5f; 
    public float heightMultiplier = 1.5f; 
    public float smoothTime = 0.5f; 
    public float minY = 5f;
    public float maxY = 40f;
    public float zoomLimiter = 50f; 

    public float minZOffset = -10f; 
    public float maxZOffset = -20f;
    public float minAngle = 30f;
    public float maxAngle = 70f;

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
        RotateCamera();
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        float heightAdjustment = CalculateHeightAdjustment();
        float currentY = Mathf.Clamp(baseHeight + heightAdjustment, minY, maxY);
        float zOffset = CalculateZOffset(currentY);

        Vector3 newPosition = centerPoint + new Vector3(0, currentY, zOffset);
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    void RotateCamera()
    {
        float currentY = Mathf.Clamp(transform.position.y, minY, maxY);
        float heightFactor = (currentY - minY) / (maxY - minY);
        float angle = Mathf.Lerp(minAngle, maxAngle, heightFactor);
        transform.rotation = Quaternion.Euler(angle, 0, 0);
    }

    float CalculateHeightAdjustment()
    {
        float maxDistance = MaxDistanceFromCenter();
        return maxDistance * heightMultiplier; 
    }

    float CalculateZOffset(float currentY)
    {
        float heightFactor = (currentY - minY) / (maxY - minY);
        return Mathf.Lerp(minZOffset, maxZOffset, heightFactor);
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