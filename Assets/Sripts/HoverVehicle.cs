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

    #endregion

    public float MaxStabilityForce = 1f;

    public float MaxMainForce = 1f;

    private void Awake()
    {
        speeder = GetComponentInChildren<Rigidbody>();

        frontLeft = gameObject.transform.position + new Vector3(2, 0, 2);
        frontRight = gameObject.transform.position + new Vector3(-2, 0, 2);
        rearLeft = gameObject.transform.position + new Vector3(2, 0, -2);
        rearRight = gameObject.transform.position + new Vector3(-2, 0, -2);
        centre = gameObject.transform.position + Vector3.zero;

        frontLeft = new Vector3(2, 0, 2);
        frontRight = new Vector3(-2, 0, 2);
        rearLeft = new Vector3(2, 0, -2);
        rearRight = new Vector3(-2, 0, -2);
        centre = Vector3.zero;
    }

    public Transform Sphere1;
    public Transform Sphere2;
    public Transform Sphere3;
    public Transform Sphere4;
    public Transform Sphere5;

    private void FixedUpdate()
    {
        positionSpheres();

        var globalPosition = gameObject.transform.position;

        stabalise(frontLeft + globalPosition, MaxStabilityForce);
        stabalise(frontRight + globalPosition, MaxStabilityForce);
        stabalise(rearLeft + globalPosition, MaxStabilityForce);
        stabalise(rearRight + globalPosition, MaxStabilityForce);
        stabalise(centre + globalPosition, MaxMainForce);
    }

    private void stabalise(Vector3 pos, float maxForce)
    {
        RaycastHit hit;
        var isHit = Physics.Raycast(pos, Vector3.down, out hit);
        if (isHit && hit.distance > 0)
        {
            var f = maxForce * (1f / hit.distance);
            if (f > maxForce)
            {
                f = maxForce;
            }
            speeder.AddForceAtPosition(Vector3.up * f, pos);
        }
        else
        {
            Debug.Log("no force!");
        }
    }

    private void positionSpheres()
    {
        Sphere1.localPosition = frontLeft;
        Sphere2.localPosition = frontRight;
        Sphere3.localPosition = rearLeft;
        Sphere4.localPosition = rearRight;
        Sphere5.localPosition = centre;
    }
}