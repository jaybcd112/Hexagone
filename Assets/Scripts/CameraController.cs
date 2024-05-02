using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Transform startPoint; // Reference to the startPoint GameObject
    public Transform controlPoint; // Reference to the controlPoint GameObject
    public Transform endPoint; // Reference to the endPoint GameObject

    private Vector3 startOffset;
    private Vector3 controlOffset;
    private Vector3 endOffset;

    private float bezierT = 0.5f;
    public float zoomSensitivity = 0.2f;
    public float rotationSensitivity = 50f;

    private void Start()
    {
        // Store the initial offsets of the control points relative to the player
        startOffset = startPoint.position - player.position;
        controlOffset = controlPoint.position - player.position;
        endOffset = endPoint.position - player.position;
    }

    void Update()
    {
        // Update the control points' positions based on the player's position and the stored offsets
        startPoint.position = player.position + startOffset;
        controlPoint.position = player.position + controlOffset;
        endPoint.position = player.position + endOffset;

        HandleZoom();
        HandleRotation();
    }

    void HandleZoom()
    {
        float scroll = -Input.mouseScrollDelta.y;
        bezierT += scroll * zoomSensitivity * Time.deltaTime;
        bezierT = Mathf.Clamp(bezierT, 0, 1);

        Vector3 curvePos = CalculateBezierPoint(bezierT, startPoint.position, controlPoint.position, endPoint.position);
        transform.position = curvePos;
        transform.LookAt(player.position);
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(2)) // Middle mouse button held
        {
            float h = rotationSensitivity * Input.GetAxis("Mouse X") * Time.deltaTime;

            // Rotate the offsets based on user input
            startOffset = Quaternion.Euler(0, h, 0) * startOffset;
            controlOffset = Quaternion.Euler(0, h, 0) * controlOffset;
            endOffset = Quaternion.Euler(0, h, 0) * endOffset;
        }
    }

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return ((1 - t) * (1 - t) * p0) + (2 * (1 - t) * t * p1) + (t * t * p2);
    }

    private void OnDrawGizmos()
    {
        if (!player || !startPoint || !controlPoint || !endPoint) return;

        Gizmos.color = Color.red;

        // Calculate the distances from the player to the Bezier curve at the respective t-values for each control point
        float startRadius = (CalculateBezierPoint(0, startPoint.position, controlPoint.position, endPoint.position) - new Vector3(player.position.x, startPoint.position.y, player.position.z)).magnitude;
        float controlRadius = (CalculateBezierPoint(0.5f, startPoint.position, controlPoint.position, endPoint.position) - new Vector3(player.position.x, controlPoint.position.y, player.position.z)).magnitude;
        float endRadius = (CalculateBezierPoint(1, startPoint.position, controlPoint.position, endPoint.position) - new Vector3(player.position.x, endPoint.position.y, player.position.z)).magnitude;

        // Draw the circles using the calculated radii and the Y values of the control points
        DrawGizmoCircle(new Vector3(player.position.x, startPoint.position.y, player.position.z), startRadius);
        DrawGizmoCircle(new Vector3(player.position.x, controlPoint.position.y, player.position.z), controlRadius);
        DrawGizmoCircle(new Vector3(player.position.x, endPoint.position.y, player.position.z), endRadius);

        Gizmos.color = Color.blue;
        for (float t = 0; t <= 1; t += 0.05f)
        {
            Gizmos.DrawSphere(CalculateBezierPoint(t, startPoint.position, controlPoint.position, endPoint.position), 0.2f);
        }
    }

    private void DrawGizmoCircle(Vector3 position, float radius)
    {
        const int segments = 50;
        float angleStep = 360f / segments;
        Vector3 prevPoint = position + Quaternion.Euler(0, 0, 0) * new Vector3(radius, 0, 0);
        for (int i = 1; i <= segments; i++)
        {
            Vector3 newPoint = position + Quaternion.Euler(0, i * angleStep, 0) * new Vector3(radius, 0, 0);
            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }
    }
}