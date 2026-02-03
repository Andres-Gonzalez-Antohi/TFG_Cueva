using System.Collections.Generic;
using UnityEngine;

public class VRTeleporter : MonoBehaviour
{
    public GameObject positionMarker;       // marker for display ground position

    public Transform bodyTransform;         // target transferred by teleport

    public LayerMask excludeLayers;         // excluding layers for performance

    public float angle = 45f;               // arc take off angle

    public float strength = 10f;            // increasing this value will increase overall arc length

    int maxVertexcount = 100;               // limitation of vertices for performance. 

    private float vertexDelta = 0.08f;      // delta between each Vertex on arc. Decresing this value may cause performance problem.

    private LineRenderer arcRenderer;       // line renderer of the arc

    private Vector3 velocity;               // Velocity of latest vertex

    private Vector3 groundPos;              // detected ground position

    private Vector3 lastNormal;             // detected surface normal

    private bool groundDetected;            // bool for detecting ground

    private List<Vector3> vertexList = new List<Vector3>(); // vertex on arc

    private bool displayActive;             // don't update path when it's false.
    
    public float verticalThreshold = 0.1f;  // threshold between hit normal

    public float rotateSpeed = 0.1f;        // rotation speed of the transform
    
    private Quaternion targetRotation;      // rotation that must have the body transform

    public float rotationOffset = 90.0f;    // rotation offset when rotating

    public Vector2 scaleMovement = new Vector2(0.1f,0.1f);      // speed scale of the joystick movement

    public string verticalAxis;

    public string horizontalAxis;

    public string teleportButton;

    public string rotationButton;

    private void Awake()
    {
        targetRotation = bodyTransform.rotation;
        arcRenderer = GetComponent<LineRenderer>();
        arcRenderer.enabled = false;
        arcRenderer.endWidth = 0.05f;
        arcRenderer.startWidth = 0.1f;
        positionMarker.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        bodyTransform.rotation = Quaternion.Lerp(bodyTransform.rotation, targetRotation, rotateSpeed);

        // Joystick movement if we're not rotating
        if (Quaternion.Angle(bodyTransform.rotation, targetRotation) < 0.01f)
        {
            float right = Input.GetAxis(horizontalAxis) * scaleMovement.x;
            float forward = Input.GetAxis(verticalAxis) * scaleMovement.y;
            bodyTransform.position += bodyTransform.forward * forward + bodyTransform.right * right;
        }

        // Left mouse click and joystick button 1
        if (Input.GetButtonDown(teleportButton))
        {
            ToggleDisplay(true);
        }
        
        // Left mouse click and joystick button 1
        if (Input.GetButtonUp(teleportButton))
        {
            Teleport();
            ToggleDisplay(false);
        }

        // Right mouse click and joystick button 0
        if (Input.GetButtonUp(rotationButton))
        {
            Rotate();
        }
    }

    private void FixedUpdate()
    {
        if (displayActive)
        {
            UpdatePath();
        }
    }
    
    // Teleport target transform to ground position
    public void Teleport()
    {
        if (groundDetected)
        {
            bodyTransform.position = groundPos;
        }
        else
        {
            Debug.Log("Ground wasn't detected");
        }
    }

    // Active Teleporter Arc Path
    public void ToggleDisplay(bool active)
    {
        arcRenderer.enabled = active;
        positionMarker.SetActive(active);
        displayActive = active;
    }

    private void UpdatePath()
    {
        groundDetected = false;

        vertexList.Clear(); // delete all previouse vertices


        velocity = Quaternion.AngleAxis(-angle, transform.right) * transform.forward * strength;

        RaycastHit hit;


        Vector3 pos = transform.position; // take off position

        vertexList.Add(pos);

        while (!groundDetected && vertexList.Count < maxVertexcount)
        {
            Vector3 newPos = pos + velocity * vertexDelta
                + 0.5f * Physics.gravity * vertexDelta * vertexDelta;

            velocity += Physics.gravity * vertexDelta;

            vertexList.Add(newPos); // add new calculated vertex

            // linecast between last vertex and current vertex
            if (Physics.Linecast(pos, newPos, out hit, ~excludeLayers,QueryTriggerInteraction.Ignore))
            {
                if (Vector3.Dot(hit.normal,Vector3.up) > verticalThreshold)
                {
                    groundDetected = true;
                    groundPos = hit.point;
                    lastNormal = hit.normal;
                }
            }
            pos = newPos; // update current vertex as last vertex
        }


        positionMarker.SetActive(groundDetected);

        if (groundDetected)
        {
            positionMarker.transform.position = groundPos + lastNormal * 0.05f;
            positionMarker.transform.LookAt(groundPos);
        }

        // Update Line Renderer

        arcRenderer.positionCount = vertexList.Count;
        arcRenderer.SetPositions(vertexList.ToArray());
    }
    
    public void Rotate()
    {
        Vector3 prod = Vector3.Cross(bodyTransform.forward, transform.forward);

        float direction = Vector3.Dot(bodyTransform.up, prod);
        
        targetRotation = targetRotation * Quaternion.Euler(0, rotationOffset * (direction > 0 ? 1 : -1), 0);
    }
}