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

        Physics.Raycast(FrontLeftSphere.position, down, out frontLeftHit);
        Physics.Raycast(FrontRightSphere.position, down, out frontRightHit);
        Physics.Raycast(LeftSphere.position, down, out rearLeftHit);
        Physics.Raycast(RightSphere.position, down, out rearRightHit);
        Physics.Raycast(centre, down, out centreHit);




        var angleHelperForce = 0.1f;

        var frontLeftLift = -Physics.gravity.y * angleHelperForce * (hoverHeight - frontLeftHit.distance * 2);
        var frontRightLift = -Physics.gravity.y * angleHelperForce * (hoverHeight - frontRightHit.distance * 2);
        var leftLift = -Physics.gravity.y * angleHelperForce * (hoverHeight - rearLeftHit.distance * 2);
        var rightLift = -Physics.gravity.y * angleHelperForce * (hoverHeight - rearRightHit.distance * 2);

        var centreLift = -Physics.gravity.y * (hoverHeight - centreHit.distance);


        var boost = 150f;

        GetComponentInChildren<Rigidbody>().AddForceAtPosition(up * frontLeftLift * boost, FrontLeftSphere.position);
        GetComponentInChildren<Rigidbody>().AddForceAtPosition(up * frontRightLift * boost, FrontRightSphere.position);
        GetComponentInChildren<Rigidbody>().AddForceAtPosition(up * leftLift * boost, LeftSphere.position);
        GetComponentInChildren<Rigidbody>().AddForceAtPosition(up * rightLift * boost, RightSphere.position);

        //GetComponentInChildren<Rigidbody>().AddForce(up * centreLift);

        GetComponentInChildren<Rigidbody>().AddRelativeForce(GetComponentInChildren<Rigidbody>().transform.up * centreLift * boost);


        //GetComponentInChildren<Rigidbody>().AddForceAtPosition(up * centreLift, centre);
    }

    void Update()
    {
    }
}
