using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class MissileController : MonoBehaviour
{
    private Rigidbody rb;
    public float moveSpeed;
    public Transform target;
    public float rotateSpeed = 15f;
    public float fieldOfView = 45f;
    public GameObject explosionPrefab;
    private Rigidbody targetRigidbody;
    public LayerMask raycastMask;
    private Vector3 castPosition;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetRigidbody = target.GetComponent<Rigidbody>();
    }

    // Normalize the angle to be within -180 to 180
    float NormalizeAngle(float angle)
    {
        return Mathf.Repeat(angle + 180f, 360f) - 270f;
    }

    void Update()
    {
        // Calculate direction to target
        Vector3 directionToTarget = (target.position + Time.deltaTime * (Vector3)targetRigidbody.linearVelocity) - transform.position;
        float targetAngle = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;
        float angleDiff = Vector3.SignedAngle(transform.forward, directionToTarget, Vector3.up);

        bool isTargetInFOV = Mathf.Abs(angleDiff) < fieldOfView;

        
        bool isTargetFound = false;
        if(Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit)){
            Debug.DrawLine(transform.position, hit.point, Color.blue); 
            castPosition = hit.point;
            if(hit.collider.transform.GetComponentInParent<TankController>()){
                isTargetFound = true;
            }
        }
        if (isTargetInFOV && isTargetFound)
        {
            float step = rotateSpeed * Time.deltaTime;
            float angleToRotate = Mathf.MoveTowardsAngle(transform.eulerAngles.y, transform.eulerAngles.y + angleDiff, step);
            transform.eulerAngles = transform.up * angleToRotate;
        }
        rb.linearVelocity = transform.forward * moveSpeed;
        
        
        #region Debug
        Debug.DrawRay(transform.position, Vector3.Normalize(directionToTarget) * 2, Color.green);
        Debug.DrawRay(transform.position, Vector3.Normalize(transform.up) * 2, Color.green);
        #endregion
    }
    private void OnCollisionEnter(Collision collision)
    {
        //GameObject explosion = Instantiate(explosionPrefab);
        //explosion.transform.position = transform.position;
        Destroy(gameObject);
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(castPosition, 0.5f);
    }
}
