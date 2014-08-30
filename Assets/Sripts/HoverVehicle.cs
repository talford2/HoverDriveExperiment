using UnityEngine;

public class HoverVehicle : MonoBehaviour
{
    public Transform FrontLeftSphere;
    public Transform FrontRightSphere;

    public Transform LeftSphere;
    public Transform RightSphere;

    public Transform CentreSphere;

    public float StabaliseForce;
    public float AntiGravityForce;

    private Camera chaseCamera;
    private float forwardPower;

    private void Awake()
    {
        chaseCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        var hoverHeight = 5f;

        var up = Vector3.up;
        var down = Vector3.down;

        var frontLeftPoint = new Vector3(-2, 0, 2f);
        var frontRightPoint = new Vector3(2, 0, 2f);

        var leftPoint = new Vector3(-2f, 0, -2);
        var rightPoint = new Vector3(2f, 0, -2);

        var centre = new Vector3(0, 0, 0);

        FrontLeftSphere.localPosition = frontLeftPoint;
        FrontRightSphere.localPosition = frontRightPoint;

        LeftSphere.localPosition = leftPoint;
        RightSphere.localPosition = rightPoint;

        CentreSphere.localPosition = centre;

        RaycastHit frontLeftHit;
        RaycastHit frontRightHit;
        RaycastHit rearLeftHit;
        RaycastHit rearRightHit;
        RaycastHit centreHit;

        var isFrontLeftFound = Physics.Raycast(FrontLeftSphere.position, down, out frontLeftHit);
        var isFrontRightFound = Physics.Raycast(FrontRightSphere.position, down, out frontRightHit);
        var isBackLeftFound = Physics.Raycast(LeftSphere.position, down, out rearLeftHit);
        var isBackRightFound = Physics.Raycast(RightSphere.position, down, out rearRightHit);

        var isCentreFound = Physics.Raycast(centre, down, out centreHit);

        var frontLeftLift = isFrontLeftFound
            ? -Physics.gravity.y * (hoverHeight - frontLeftHit.distance * 2)
            : 0f;
        var frontRightLift = isFrontRightFound
            ? -Physics.gravity.y * (hoverHeight - frontRightHit.distance * 2)
            : 0f;
        var leftLift = isBackLeftFound
            ? -Physics.gravity.y * (hoverHeight - rearLeftHit.distance * 2)
            : 0f;
        var rightLift = isBackRightFound
            ? -Physics.gravity.y * (hoverHeight - rearRightHit.distance * 2)
            : 0f;

        var centreLift = -Physics.gravity.y * (hoverHeight - centreHit.distance);

        // Debug.Log(isCentreFound);

        GetComponentInChildren<Rigidbody>().AddForceAtPosition(up * frontLeftLift * StabaliseForce, FrontLeftSphere.position);
        GetComponentInChildren<Rigidbody>().AddForceAtPosition(up * frontRightLift * StabaliseForce, FrontRightSphere.position);
        GetComponentInChildren<Rigidbody>().AddForceAtPosition(up * leftLift * StabaliseForce, LeftSphere.position);
        GetComponentInChildren<Rigidbody>().AddForceAtPosition(up * rightLift * StabaliseForce, RightSphere.position);

        GetComponentInChildren<Rigidbody>().AddRelativeForce(up * centreLift * AntiGravityForce);

        GetComponentInChildren<Rigidbody>().AddRelativeForce(Vector3.forward *forwardPower * 2000f);
    }

    void Update()
    {
        forwardPower = Input.GetAxis("Vertical");
        Debug.Log(forwardPower);
        chaseCamera.transform.position = Vector3.Slerp(chaseCamera.transform.position, transform.position - transform.rotation*new Vector3(0, 0, 10f), 2f*Time.deltaTime);
        chaseCamera.transform.LookAt(transform.position);
    }
}
