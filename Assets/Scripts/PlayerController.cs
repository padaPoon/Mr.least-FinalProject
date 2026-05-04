using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f;
    public float gravityMultiplier = 1f;
    public bool gameOver = false;
    private Rigidbody rb;
    private InputAction jumpAction;
    public Animator animator;
    public ParticleSystem fxDirt;
    public GameObject fxExplosionPrefab;
    // 5.8 add audio source variable to play crash sound
    private AudioSource audioSource;

    public AudioClip crashSound;
    public AudioClip jumpSound; 

    private bool isOnGround = true;
    private bool isUpsideDown = false;

    public void FlipGravity()
    {
        isUpsideDown = !isUpsideDown;
        Physics.gravity = new Vector3(0f, -Physics.gravity.y, 0f);
        transform.Rotate(0f, 0f, 180f);
        isOnGround = false;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        jumpAction = InputSystem.actions.FindAction("Jump");

        // 5.8 get audio source component, if not exist, add one
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Physics.gravity *= gravityMultiplier;
        animator.SetFloat("Speed_f", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            return;
        }

        if (jumpAction.triggered && isOnGround)
        {
            Vector3 jumpDir = isUpsideDown ? Vector3.down : Vector3.up;
            rb.AddForce(jumpForce * jumpDir, ForceMode.Impulse);
            isOnGround = false;
            animator.SetTrigger("Jump_trig");
            fxDirt.Stop();
            audioSource.PlayOneShot(jumpSound);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            fxDirt.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            gameOver = true;
            animator.SetBool("Death_b", true);
            animator.SetInteger("DeathType_int", 1 );
            fxDirt.Stop();
            Instantiate(fxExplosionPrefab,transform.position,Quaternion.identity);
            audioSource.PlayOneShot(crashSound);
        }
    }
}
