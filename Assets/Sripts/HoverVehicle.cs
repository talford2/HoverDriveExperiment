using UnityEditor;
using UnityEngine;

public class HoverVehicle : MonoBehaviour
{
    #region Private Members

    private Rigidbody speeder;

    private Vector3 frontLeft;
    private Vector3 frontRight;
    private Vector3 rearLeft;
    private Vector3 rearRight;
    private Vector3 centre;

    private Camera chaseCamera;

    #endregion

    public Transform Sphere1;
    public Transform Sphere2;
    public Transform Sphere3;
    public Transform Sphere4;

    public float HoverHeight = 3f;
    public float SpringCoefficient = 60f;
    public float DampingForce = 40f;

    public float MaxForwardThrust = 12000f;
    public float MaxTurnTorque = 4000f;

    private void Awake()
    {
        speeder = GetComponentInChildren<Rigidbody>();

        frontLeft = gameObject.transform.position + Sphere1.localPosition;
        frontRight = gameObject.transform.position + Sphere2.localPosition;
        rearLeft = gameObject.transform.position + Sphere3.localPosition;
        rearRight = gameObject.transform.position + Sphere4.localPosition;

        frontLeft = new Vector3(2f, 0, 2f);
        frontRight = new Vector3(-2f, 0, 2f);
        rearLeft = new Vector3(2f, 0, -2f);
        rearRight = new Vector3(-2f, 0, -2f);
        centre = Vector3.zero;

        chaseCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        var forwardThrust = MaxForwardThrust*Input.GetAxis("Vertical");
        var turnTorque = MaxTurnTorque*Input.GetAxis("Horizontal");

        rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * forwardThrust);
        rigidbody.AddRelativeTorque(new Vector3(0, turnTorque, 0));

        ApplyHoverEngine(transform.TransformPoint(frontLeft));
        ApplyHoverEngine(transform.TransformPoint(frontRight));
        ApplyHoverEngine(transform.TransformPoint(rearLeft));
        ApplyHoverEngine(transform.TransformPoint(rearRight));
    }

    private void ApplyHoverEngine(Vector3 pos)
    {
        RaycastHit hit;
        if (Physics.Raycast(pos + new Vector3(0, -4f, 0), Vector3.down, out hit, HoverHeight))
        {
            var addForce = 0f;
            if (hit.distance < HoverHeight)
            {
                var heightDifference = Mathf.Abs(HoverHeight - hit.distance)/HoverHeight;
                addForce = heightDifference*SpringCoefficient*rigidbody.mass;
                addForce -= rigidbody.GetPointVelocity(pos).y*DampingForce;
            }
            rigidbody.AddForceAtPosition(addForce*Vector3.up/4f, pos);
        }
    }

    private void Update()
    {
        chaseCamera.transform.position = Vector3.Slerp(chaseCamera.transform.position, transform.position + transform.rotation * new Vector3(0, 0, -10f), Time.deltaTime);
        chaseCamera.transform.LookAt(transform.position);
    }

    private void Stabalise(Vector3 pos, float maxForce)
    {
        RaycastHit hit;
        var isHit = Physics.Raycast(transform.TransformPoint(pos), Vector3.down, out hit);
        if (isHit && hit.distance > 0)
        {
            if (hit.distance < 5f)
            {
                var relDist = Mathf.Abs((hit.distance - HoverHeight) / HoverHeight);

                //var toCentreOfMass = pos - rigidbody.centerOfMass;
                //Debug.Log("COM: " + toCentreOfMass.magnitude);

                var force = -Physics.gravity.y*rigidbody.mass/4f;
                speeder.AddForceAtPosition(Vector3.up * force, pos);
            }
        }
        else
        {
            Debug.Log("no force!");
        }
    }
}