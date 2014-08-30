using UnityEngine;

public class Car : MonoBehaviour
{

    public WheelCollider FrontLeftCollider;
    public WheelCollider FrontRightCollider;
    public WheelCollider RearLeftCollider;
    public WheelCollider RearRightCollider;

    public Transform WheelFrontLeft;
    public Transform WheelFrontRight;
    public Transform WheelRearLeft;
    public Transform WheelRearRight;


    private float steer;

    private float speed;
    private float maxSpeed = 100;
    private float acceleration = 100f;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame

    void Update()
    {
    }

    void FixedUpdate()
    {
        //Debug.Log("2");

        //void Update () {

        //steer = 0;
        if (Input.GetKey(KeyCode.A))
        {
            steer = Mathf.LerpAngle(steer, -30f, Time.deltaTime);
            //steer = -30;
        }
        if (Input.GetKey(KeyCode.D))
        {
            steer = Mathf.LerpAngle(steer, 30f, Time.deltaTime);
            //steer = 30;
        }

        
        if (Input.GetKey(KeyCode.S))
        {
            speed += acceleration * Time.deltaTime;

        }
        else if (Input.GetKey(KeyCode.W))
        {
            speed -= acceleration * Time.deltaTime;

        }
        speed = Mathf.Clamp(speed, maxSpeed * -1, maxSpeed);
        //else
        //{
        //    if (speed>0)
        //    {
        //        speed -= acceleration * Time.deltaTime;
        //    }
        //    if (speed<0)
        //    {
        //        speed += acceleration * Time.deltaTime;
        //    }
        //}

        //Debug.Log("Speed : " + speed);

        FrontLeftCollider.motorTorque = speed;
        FrontRightCollider.motorTorque = speed;

        FrontLeftCollider.steerAngle = steer;
        FrontRightCollider.steerAngle = steer;

        /*
        WheelFrontLeft.localEulerAngles = new Vector3(WheelFrontLeft.localEulerAngles.x, steer + 90f, (WheelFrontLeft.localEulerAngles.z));
        WheelFrontRight.localEulerAngles = new Vector3(WheelFrontRight.localEulerAngles.x, steer + 90f, (WheelFrontRight.localEulerAngles.z));
        */

        WheelFrontLeft.Rotate(0, FrontLeftCollider.rpm * 6 * Time.deltaTime, 0);
        WheelFrontRight.Rotate(0, FrontRightCollider.rpm * 6 * Time.deltaTime, steer);
        WheelRearLeft.Rotate(0, RearLeftCollider.rpm * 6 * Time.deltaTime, 0);
        WheelRearRight.Rotate(0, RearRightCollider.rpm * 6 * Time.deltaTime, 0);

        //WheelFrontLeft.localEulerAngles = new Vector3(WheelFrontLeft.localEulerAngles.x, steer, WheelFrontLeft.localEulerAngles.z);
        //WheelFrontRight.localEulerAngles = new Vector3(WheelFrontRight.localEulerAngles.x, steer, WheelFrontRight.localEulerAngles.z);
    }
}
