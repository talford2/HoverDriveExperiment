using UnityEngine;
using System.Collections;

public class ChaseCamera : MonoBehaviour
{
    public Transform ChaseObject;
    
    public float CameraHeightOffset = 3f;

    private Quaternion curRotation;

    private float curHeight;

    void Start()
    {

    }

    void Update()
    {
        var targetRotation = Quaternion.Euler(0f, ChaseObject.rotation.eulerAngles.y, 0f);

        var targetHeight = ChaseObject.position.y;

        curHeight = Mathf.Lerp(curHeight, targetHeight, Time.deltaTime * 10f);

        curRotation = Quaternion.Slerp(curRotation, targetRotation, 10f * Time.deltaTime);

        transform.position = curRotation * new Vector3(0, 0, -10f) + new Vector3(ChaseObject.position.x, curHeight + CameraHeightOffset, ChaseObject.position.z);
        transform.LookAt(ChaseObject.position + new Vector3(0, CameraHeightOffset, 0));
    }
}
