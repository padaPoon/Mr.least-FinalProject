using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f;
    public float gravityMultiplier = 1f;
    public float flipRotationSpeed = 1080f;
    public float flipJumpForce = 15f;          
    public float flipDelay = 0.2f;              
    public Animator animator;
    public ParticleSystem fxDirt;
    public GameObject fxExplosionPrefab;
    public AudioClip crashSound;
    public AudioClip jumpSound;

    [Header("Ball Form")]
    public GameObject normalModel;        // your normal character mesh
    public GameObject ballModel;          // your ball mesh
    public BoxCollider boxCollider;       // assign in inspector
    public SphereCollider sphereCollider; // assign in inspector

    public bool gameOver = false;

    private Rigidbody rb;
    private AudioSource audioSource;

    private bool isOnGround = true;
    private bool isUpsideDown = false;
    private bool isFlipping = false;
    private bool isBall = false;
    private float targetRotationZ = 0f;
    private Vector3 originalGravity;


    public int Maxhealth = 3;
    public int curentHealth;


    public bool isImmune = false;
    public float immunityDuration = 5f;
    public Color immuneColor = Color.yellow;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        curentHealth = Maxhealth;
    }

    void Start()
    {
        Physics.gravity *= gravityMultiplier;
        originalGravity = Physics.gravity;
        animator.SetFloat("Speed_f", 1.0f);
        ApplyBallState();
    }

    void Update()
    {
        if (gameOver) return;
        if (Keyboard.current == null) return;

        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            isBall = !isBall;
            ApplyBallState();
        }

        //Space
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isOnGround && !isFlipping)
        {
            Vector3 jumpDir = isUpsideDown ? Vector3.down : Vector3.up;
            rb.AddForce(jumpForce * jumpDir, ForceMode.Impulse);
            isOnGround = false;
            animator.SetTrigger("Jump_trig");
            fxDirt.Stop();
            audioSource.PlayOneShot(jumpSound);
        }

        //กระโดดก่อนพลิก
        if (Keyboard.current.qKey.wasPressedThisFrame && isOnGround && !isFlipping)
        {
            StartCoroutine(JumpAndFlip());
        }

        // หมุนตัวละครแบบ smooth
        Quaternion target = Quaternion.Euler(
            transform.eulerAngles.x,
            transform.eulerAngles.y,
            targetRotationZ);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, target, flipRotationSpeed * Time.deltaTime);
    }

    void ApplyBallState()
    {
        // Swap visuals
        if (normalModel != null) normalModel.SetActive(!isBall);
        if (ballModel != null)   ballModel.SetActive(isBall);

        // Swap colliders
        if (boxCollider != null)    boxCollider.enabled    = !isBall;
        if (sphereCollider != null) sphereCollider.enabled = isBall;
    }

    private IEnumerator JumpAndFlip()
    {
        isFlipping = true;

        //กระโดดขึ้นก่อน
        Vector3 jumpDir = isUpsideDown ? Vector3.down : Vector3.up;
        rb.linearVelocity = Vector3.zero; 
        rb.AddForce(flipJumpForce * jumpDir, ForceMode.Impulse);

        isOnGround = false;
        animator.SetTrigger("Jump_trig");
        fxDirt.Stop();
        audioSource.PlayOneShot(jumpSound);

        //ลอยขึ้นพอประมาณ
        yield return new WaitForSeconds(flipDelay);
        FlipGravity();
        isFlipping = false;
    }

    public void FlipGravity()
    {
        isUpsideDown = !isUpsideDown;
        Physics.gravity = isUpsideDown ? -originalGravity : originalGravity;
        targetRotationZ = isUpsideDown ? 180f : 0f;
        isOnGround = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") ||
            collision.gameObject.CompareTag("Ceiling"))
        {
            isOnGround = true;
            fxDirt.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {   
            if(isImmune) return;
            TakeDamage(1);
            Debug.Log("Collided with obstacle! Current Health: " + curentHealth);
            animator.SetInteger("DeathType_int", 1);
            fxDirt.Stop();
            Instantiate(fxExplosionPrefab, transform.position, Quaternion.identity);
            audioSource.PlayOneShot(crashSound);
        }



    }


    public void TakeDamage(int damage)
    {
        if (gameOver) return;

        curentHealth -= damage;
        if (curentHealth <= 0)
        {
            curentHealth = 0;
            Debug.Log("Game Over!");
            gameOver = true;
            animator.SetBool("Death_b", true);

            
        }
    }

    public void ActivateImmunity()
    {
        if (isImmune) StopCoroutine(nameof(ImmortalityCoroutine)); // reset ถ้าเก็บซ้ำ
        StartCoroutine(ImmortalityCoroutine(immunityDuration));
    }

    private IEnumerator ImmortalityCoroutine(float duration)
    {
        isImmune = true;

        // กระพริบตัวละครตอน immune
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        float elapsed = 0f;
        bool visible = true;

    while (elapsed < duration)
    {
        visible = !visible;
        foreach (Renderer r in renderers) r.enabled = visible;
        yield return new WaitForSeconds(0.15f);
        elapsed += 0.15f;
    }

        // คืนค่าปกติ
        foreach (Renderer r in renderers) r.enabled = true;
        isImmune = false;
    }
}