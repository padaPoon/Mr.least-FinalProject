using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerGravityFlip3D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float forwardSpeed = 10f;
    [SerializeField] private float maxSpeed = 25f;
    [SerializeField] private float speedIncreaseRate = 0.1f; // เพิ่มความเร็วต่อวินาที
    [SerializeField] private float gravityStrength = 50f;
    [SerializeField] private float laneChangeSpeed = 10f;
    [SerializeField] private float laneDistance = 2.5f;      // ระยะห่างระหว่างเลน

    [Header("Flip Settings")]
    [SerializeField] private float flipRotationSpeed = 1080f;
    [SerializeField] private bool allowAirFlip = false;       // พลิกกลางอากาศได้ไหม

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.3f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;
    private bool isFlipped = false;
    private bool isGrounded = false;
    private float targetZRotation = 0f;
    private int currentLane = 0;     // -1, 0, 1 (ซ้าย, กลาง, ขวา)
    private float targetX = 0f;

    public bool IsFlipped => isFlipped;
    public float CurrentSpeed => forwardSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation; // กันตัวล้มจากการชน
    }

    private void Update()
    {
        // เช็กพื้น/เพดาน
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        HandleInput();
        SmoothRotation();

        // เพิ่มความเร็วเรื่อยๆ
        forwardSpeed = Mathf.Min(forwardSpeed + speedIncreaseRate * Time.deltaTime, maxSpeed);
    }

    private void FixedUpdate()
    {
        // กำหนดทิศ gravity ตามสถานะ
        float gravityDir = isFlipped ? 1f : -1f;

        Vector3 velocity = rb.linearVelocity;   // Unity 6+ | เวอร์ชันเก่าใช้ rb.velocity
        velocity.y += gravityStrength * gravityDir * Time.fixedDeltaTime;

        // เปลี่ยนเลน (แกน X) แบบ smooth
        float newX = Mathf.Lerp(rb.position.x, targetX, laneChangeSpeed * Time.fixedDeltaTime);
        velocity.x = (newX - rb.position.x) / Time.fixedDeltaTime;

        rb.linearVelocity = velocity;
    }

    private void HandleInput()
    {
        // พลิก gravity
        bool flipPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);
        if (flipPressed && (isGrounded || allowAirFlip))
        {
            FlipGravity();
        }

        // เปลี่ยนเลน ซ้าย/ขวา
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            ChangeLane(-1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            ChangeLane(1);
        }
    }

    private void ChangeLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, -1, 1);
        targetX = currentLane * laneDistance;
    }

    private void FlipGravity()
    {
        isFlipped = !isFlipped;
        targetZRotation = isFlipped ? 180f : 0f;
    }

    private void SmoothRotation()
    {
        // หมุนรอบแกน Z (เพราะวิ่งไปแกน Z, แกนซ้าย-ขวาคือ X, แกนตั้งคือ Y)
        Quaternion target = Quaternion.Euler(0f, 0f, targetZRotation);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, target, flipRotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    // เรียกจาก obstacle ตอนชน
    public void OnHitObstacle()
    {
        Debug.Log("Game Over!");
        // TODO: ใส่ระบบ game over ที่นี่
    }
}