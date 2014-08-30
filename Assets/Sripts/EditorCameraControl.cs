using UnityEngine;

public class EditorCameraControl : MonoBehaviour
{
    // Camera Control Properties
    public float ZoomSensitivity;
    public float XSensitivity;
    public float ZSensitivity;
    public bool IsEnabled;

    // Camera Variables
    private bool isRotateMode;
    private Vector3 mouseOffset;

    private float cameraYaw;
    private float cameraYawOffset;
    private float cameraPitch;
    private float cameraPitchOffset;
    private float cameraDistance;

    private Quaternion rotationOffset;
    private Vector3 positionOffset;

    private void Start()
    {
        rotationOffset = Camera.main.transform.rotation;
        positionOffset = Camera.main.transform.position;
    }

    private void Update()
    {
        if (IsEnabled)
        {
            if (Input.GetMouseButton(1))
            {
                if (!isRotateMode)
                {
                    isRotateMode = true;
                    mouseOffset = Input.mousePosition;

                    cameraYawOffset = cameraYaw;
                    cameraPitchOffset = cameraPitch;
                }
                if (isRotateMode)
                {
                    var delta = Input.mousePosition - mouseOffset;
                    cameraYaw = cameraYawOffset + 360f*delta.x/Screen.width;
                    cameraPitch = cameraPitchOffset + 360f*delta.y/Screen.height;
                }
            }
            else
            {
                isRotateMode = false;
            }

            var angle = Quaternion.Euler(rotationOffset.eulerAngles.x - cameraPitch, rotationOffset.eulerAngles.y + cameraYaw, rotationOffset.eulerAngles.z);

            cameraDistance -= Input.GetAxis("Mouse ScrollWheel")*ZoomSensitivity;
            positionOffset += angle*new Vector3(1f, 0, 0)*Input.GetAxis("Horizontal")*XSensitivity*Time.deltaTime;
            positionOffset += angle*new Vector3(0, 0, 1f)*Input.GetAxis("Vertical")*ZSensitivity*Time.deltaTime;

            positionOffset.y = Mathf.Clamp(positionOffset.y, 0.2f, 100f);

            Camera.main.transform.position = positionOffset + angle*new Vector3(0, 0, -1f)*cameraDistance;
            Camera.main.transform.rotation = angle;
        }
    }
}