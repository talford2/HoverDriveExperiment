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

    public float DistanceMultiplier = 1f;

    private void Awake()
    {
        speeder = GetComponentInChildren<Rigidbody>();

        frontLeft = gameObject.transform.position + Sphere1.localPosition;
        frontRight = gameObject.transform.position + Sphere2.localPosition;
        rearLeft = gameObject.transform.position + Sphere3.localPosition;
        rearRight = gameObject.transform.position + Sphere4.localPosition;
        centre = gameObject.transform.position + Sphere5.localPosition;

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
        var globalPosition = gameObject.transform.position;

        stabalise(frontLeft + globalPosition, MaxStabilityForce);
        stabalise(frontRight + globalPosition, MaxStabilityForce);
        stabalise(rearLeft + globalPosition, MaxStabilityForce);
        stabalise(rearRight + globalPosition, MaxStabilityForce);
        stabalise(centre + globalPosition, MaxMainForce);
    }

    private void Update()
    {
        
    }

    private void stabalise(Vector3 pos, float maxForce)
    {
        RaycastHit hit;
        var isHit = Physics.Raycast(pos, Vector3.down, out hit);
        if (isHit && hit.distance > 0)
        {
            var f = maxForce * (1f / hit.distance * DistanceMultiplier);
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
}