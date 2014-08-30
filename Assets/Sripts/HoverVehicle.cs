using UnityEngine;

public class HoverVehicle : MonoBehaviour
{
    public Transform FrontLeftSphere;
    public Transform FrontRightSphere;

    public Transform LeftSphere;
    public Transform RightSphere;

    public Transform CentreSphere;

    void Start()
    {
    }

    private void FixedUpdate()
    {
        var hoverHeight = 5f;

        var up = Vector3.up;// new Vector3(0, 1f, 0);

        var down = Vector3.down;// new Vector3(0, -1, 0);

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

        var angleHelperForce = 0.1f;

        var frontLeftLift = isFrontLeftFound
            ? -Physics.gravity.y*angleHelperForce*(hoverHeight - frontLeftHit.distance*2)
            : 0f;
        var frontRightLift = isFrontRightFound
            ? -Physics.gravity.y*angleHelperForce*(hoverHeight - frontRightHit.distance*2)
            : 0f;
        var leftLift = isBackLeftFound
            ? -Physics.gravity.y*angleHelperForce*(hoverHeight - rearLeftHit.distance*2)
            : 0f;
        var rightLift = isBackRightFound
            ? -Physics.gravity.y*angleHelperForce*(hoverHeight - rearRightHit.distance*2)
            : 0f;

        var centreLift = -Physics.gravity.y*(hoverHeight - centreHit.distance);

        // Debug.Log(isCentreFound);

        var boost = 150f;

        GetComponentInChildren<Rigidbody>().AddForceAtPosition(up * frontLeftLift * boost, FrontLeftSphere.position);
        GetComponentInChildren<Rigidbody>().AddForceAtPosition(up * frontRightLift * boost, FrontRightSphere.position);
        GetComponentInChildren<Rigidbody>().AddForceAtPosition(up * leftLift * boost, LeftSphere.position);
        GetComponentInChildren<Rigidbody>().AddForceAtPosition(up * rightLift * boost, RightSphere.position);

        GetComponentInChildren<Rigidbody>().AddRelativeForce(up * centreLift * boost);
    }

    void Update()
    {
    }
}
