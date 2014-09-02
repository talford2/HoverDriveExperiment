using UnityEngine;

public class HoverVehicle : MonoBehaviour
{
    #region Private Members

    private Vector3 frontLeft;
    private Vector3 frontRight;
    private Vector3 rearLeft;
    private Vector3 rearRight;

    private Camera chaseCamera;

    #endregion

    public Transform Sphere1;
    public Transform Sphere2;
    public Transform Sphere3;
    public Transform Sphere4;

    public float HoverHeight;
    public float SpringCoefficient;
    public float DampingForce;

    public float ThrustAcceleration;
    public float MaxThrustVelocity;

    public float TurnAcceleration;
    public float MaxTurnVelocity;

    [Range(0f, 1f)]
    public float DragMuliplier;

    [Range(0f,1f)]
    public float ThrustMultiplier;

    private void Awake()
    {
        frontLeft = gameObject.transform.position + Sphere1.localPosition;
        frontRight = gameObject.transform.position + Sphere2.localPosition;
        rearLeft = gameObject.transform.position + Sphere3.localPosition;
        rearRight = gameObject.transform.position + Sphere4.localPosition;

        frontLeft = new Vector3(2f, 0, 2f);
        frontRight = new Vector3(-2f, 0, 2f);
        rearLeft = new Vector3(2f, 0, -2f);
        rearRight = new Vector3(-2f, 0, -2f);

        chaseCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        var forwardThrust = ThrustAcceleration * Input.GetAxis("Vertical");
        var turnTorque = TurnAcceleration * Input.GetAxis("Horizontal");

        if (rigidbody.angularVelocity.magnitude < MaxTurnVelocity)
            rigidbody.AddRelativeTorque(new Vector3(0, turnTorque, 0), ForceMode.Acceleration);

        if (rigidbody.velocity.magnitude < MaxThrustVelocity)
            rigidbody.AddRelativeForce(Vector3.forward * forwardThrust, ForceMode.Acceleration);

        ApplyHoverEngine(transform.TransformPoint(frontLeft));
        ApplyHoverEngine(transform.TransformPoint(frontRight));
        ApplyHoverEngine(transform.TransformPoint(rearLeft));
        ApplyHoverEngine(transform.TransformPoint(rearRight));

        // Drag to mimic wind resistence

        // Get the current magnitude
        var speed = rigidbody.velocity.magnitude;

        // Apply drag in all directions
        rigidbody.velocity = new Vector3(rigidbody.velocity.x*DragMuliplier, rigidbody.velocity.y, rigidbody.velocity.z*DragMuliplier);

        // Add additional thrust in new direction
        rigidbody.AddRelativeForce(Vector3.forward * speed * ThrustMultiplier, ForceMode.Acceleration);
    }

    private void ApplyHoverEngine(Vector3 pos)
    {
        RaycastHit hit;
        if (Physics.Raycast(pos, Vector3.down, out hit, HoverHeight))
        {
            var addForce = 0f;
            if (hit.distance < HoverHeight)
            {
                var heightDifference = Mathf.Abs(HoverHeight - hit.distance) / HoverHeight;
                addForce = heightDifference * SpringCoefficient * rigidbody.mass;
                addForce -= rigidbody.GetPointVelocity(pos).y * DampingForce;
            }
            rigidbody.AddForceAtPosition(addForce * Vector3.up / 4f, pos);

        }
    }

    private void Update()
    {
        chaseCamera.transform.position = Vector3.Slerp(chaseCamera.transform.position, transform.position + transform.localRotation * new Vector3(0, 0, -10f), Time.deltaTime);
        chaseCamera.transform.LookAt(transform.position);
    }

    private void OnGUI()
    {
        GUI.TextArea(new Rect(30, 30, 200, 200), Mathf.Round(rigidbody.velocity.magnitude * 10000) / 10000 + "");
    }
}