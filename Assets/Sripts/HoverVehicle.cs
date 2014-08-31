using UnityEngine;

public class HoverVehicle : MonoBehaviour
{
    public Transform FrontLeftSphere;
    public Transform FrontRightSphere;

    public Transform LeftSphere;
    public Transform RightSphere;

    public Transform CentreSphere;

    public Transform DirectionCube;

    //public float StabaliseForce;
    public float AntiGravityForce;

    [Range(0, 1)] public float StabliseAmount = 0.5f;

    [Range(0.001f, 10)] public float DistanceEffect = 2.0f;

    public float hoverHeight = 5f;

    private Camera chaseCamera;

    private float forwardCoeff = 50000f;
    private float forwardPower;

    private float strafeCoeff = 50000f;
    private float strafePower;

    private float turnCoeff = 20000f;
    private float turnPower;

    private void Awake()
    {
        chaseCamera = Camera.main;
    }

    private void FixedUpdate()
    {
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
            ? -Physics.gravity.y*(hoverHeight - frontLeftHit.distance*DistanceEffect)
            : 0f;
        var frontRightLift = isFrontRightFound
            ? -Physics.gravity.y*(hoverHeight - frontRightHit.distance*DistanceEffect)
            : 0f;
        var leftLift = isBackLeftFound
            ? -Physics.gravity.y*(hoverHeight - rearLeftHit.distance*DistanceEffect)
            : 0f;
        var rightLift = isBackRightFound
            ? -Physics.gravity.y*(hoverHeight - rearRightHit.distance*DistanceEffect)
            : 0f;

        var centreLift = -Physics.gravity.y*(hoverHeight - centreHit.distance*DistanceEffect);

        // Debug.Log(isCentreFound);


        var stabaliseForce = AntiGravityForce/1f*StabliseAmount;

        GetComponentInChildren<Rigidbody>().AddForceAtPosition(up*frontLeftLift*stabaliseForce, FrontLeftSphere.position);
        GetComponentInChildren<Rigidbody>().AddForceAtPosition(up*frontRightLift*stabaliseForce, FrontRightSphere.position);
        GetComponentInChildren<Rigidbody>().AddForceAtPosition(up*leftLift*stabaliseForce, LeftSphere.position);
        GetComponentInChildren<Rigidbody>().AddForceAtPosition(up*rightLift*stabaliseForce, RightSphere.position);

        GetComponentInChildren<Rigidbody>().AddRelativeForce(up*centreLift*AntiGravityForce*(1f - StabliseAmount));

        // Control Hovercraft
        GetComponentInChildren<Rigidbody>().AddRelativeForce(Vector3.forward*forwardCoeff*forwardPower);
        GetComponentInChildren<Rigidbody>().AddRelativeForce(Vector3.right*strafeCoeff*strafePower);
        GetComponentInChildren<Rigidbody>().AddRelativeTorque(0, turnCoeff*turnPower, 0);
    }

    private void Update()
    {
        forwardPower = Input.GetAxis("Vertical");
        strafePower = Input.GetAxis("Horizontal");
        turnPower = Input.GetAxis("Mouse X");

        DirectionCube.position = transform.position + transform.rotation*new Vector3(0, 0, 20f);

        Debug.Log(forwardPower);
        chaseCamera.transform.position = Vector3.Slerp(chaseCamera.transform.position, transform.position + transform.rotation*new Vector3(0, 2.5f, -5f), 2f*Time.deltaTime);
        chaseCamera.transform.LookAt(transform.position);
    }
}