using UnityEngine;
using UnityEngine.InputSystem;

public class TankController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotationSpeed = 100f;
    private Vector2 lastInput;
    private Rigidbody rb;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public bool player2;
    private Animator anim;
    private Vector2 moveDirection;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Get player input for movement
        //Vector2 moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveDirection.Normalize();  // Normalize to prevent diagonal speed boost
        if (player2)
        {
            moveDirection = -moveDirection;
        }

        if (moveDirection != Vector2.zero)
        {
            lastInput = moveDirection;
        }

        // Handle movement in the direction of the input
        Vector3 moveVec = new Vector3(moveDirection.x, 0, moveDirection.y) * moveSpeed;

        // Move the tank using Rigidbody velocity
        MoveTank(moveVec);

        // Handle rotation
        RotateTank();
    }
    public void Move(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }
    public void Fire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            bullet.transform.parent = null;
            anim.Play("shoot");
        }
    }

    private void MoveTank(Vector3 moveVec)
    {
        // Set the rigidbody velocity for movement in the X and Z directions
        rb.linearVelocity = new Vector3(moveVec.x, rb.linearVelocity.y, moveVec.z);
    }

    private void RotateTank()
    {
        // Calculate the target rotation based on the input direction
        float targetRotation = Mathf.Atan2(lastInput.x, lastInput.y) * Mathf.Rad2Deg;

        // Rotate the tank smoothly using Rigidbody.MoveRotation
        Quaternion targetRotationQuat = Quaternion.Euler(0, targetRotation, 0);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotationQuat, Time.deltaTime * rotationSpeed));
    }
}
