using UnityEngine;

public class HoverVehicle : MonoBehaviour
{
    #region Private Members

    private Vector3 frontLeft;
    private Vector3 frontRight;
    private Vector3 rearLeft;
    private Vector3 rearRight;

    //private Camera chaseCamera;

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

    //[Range(0f, 1f)]
    //public float DragMuliplier;

    //[Range(0f, 1f)]
    //public float PerpendicularDragMultiplier;

    [Range(0f, 1f)]
    public float ThrustMultiplier;

    private bool isGrounded = false;
    public Vector3 DragCoeffcients = new Vector3(1, 1, 1);

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

        //chaseCamera = Camera.main;
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

        var perpendicularVelocity = Vector3.Project(rigidbody.velocity, transform.TransformDirection(Vector3.right));

        // Apply drag in all directions
        //rigidbody.velocity = new Vector3(rigidbody.velocity.x * DragMuliplier - PerpendicularDragMultiplier * perpendicularVelocity.x, rigidbody.velocity.y, rigidbody.velocity.z * DragMuliplier - PerpendicularDragMultiplier * perpendicularVelocity.z);


        //rigidbody.velocity = new Vector3(
        //    rigidbody.velocity.x * DragCoeffcients.x - PerpendicularDragMultiplier * perpendicularVelocity.x,
        //    rigidbody.velocity.y * DragCoeffcients.y,
        //    rigidbody.velocity.z * DragCoeffcients.z - PerpendicularDragMultiplier * perpendicularVelocity.z);


        //rigidbody.velocity = new Vector3(rigidbody.velocity.x * DragCoeffcients.x, rigidbody.velocity.y * DragCoeffcients.y, rigidbody.velocity.z * DragCoeffcients.z);



        var rv = transform.InverseTransformDirection(rigidbody.velocity);
        var drag = new Vector3(rv.x * DragCoeffcients.x,
                               rv.y * DragCoeffcients.y,
                               rv.z * DragCoeffcients.z);
        rigidbody.AddRelativeForce(-drag * rigidbody.mass);


        // Add additional thrust in new direction
        //rigidbody.AddRelativeForce(Vector3.forward * speed * ThrustMultiplier, ForceMode.Acceleration);
    }

    private void ApplyHoverEngine(Vector3 pos)
    {
        RaycastHit hit;
        if (Physics.Raycast(pos, Vector3.down, out hit, HoverHeight))
        {
            var addForce = 0f;

            
            // grounded
            if (hit.distance < HoverHeight)
            {
                var heightDifference = Mathf.Abs(HoverHeight - hit.distance) / HoverHeight;
                addForce = heightDifference * SpringCoefficient * rigidbody.mass;
                addForce -= rigidbody.GetPointVelocity(pos).y * DampingForce;
                
            }
            rigidbody.AddForceAtPosition(addForce * Vector3.up / 4f, pos);

        }
    }

   

    private void OnGUI()
    {
        GUI.TextArea(new Rect(30, 30, 200, 200), Mathf.Round(rigidbody.velocity.magnitude * 10000) / 10000 + "");
    }
}