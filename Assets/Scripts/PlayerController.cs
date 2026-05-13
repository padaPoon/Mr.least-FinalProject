using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public enum PlayerID { Player1, Player2 }

    [Header("Player Setup")]
    public PlayerID playerID = PlayerID.Player1;

    [Header("Movement")]
    public float jumpForce = 10f;
    public float gravityStrength = 20f;        // ใช้แทน gravityMultiplier
    public float flipRotationSpeed = 1080f;
    public float flipJumpForce = 15f;          
    public float flipDelay = 0.2f;              

    [Header("References")]
    public Animator animator;
    public ParticleSystem fxDirt;
    public GameObject fxExplosionPrefab;
    public AudioClip crashSound;
    public AudioClip jumpSound;

    [Header("Ball Form")]
    public GameObject normalModel;
    public GameObject ballModel;
    public BoxCollider boxCollider;
    public SphereCollider sphereCollider;

    [Header("Health")]
    public int Maxhealth = 3;
    public int curentHealth;

    [Header("Immunity")]
    public bool isImmune = false;
    public float immunityDuration = 5f;
    public Color immuneColor = Color.yellow;
    [Header("Score")]
    public int score = 0;

    [Header("Speed Boost")]
    public float speedBoostMultiplier = 2f;

    public bool gameOver = false;

    private Rigidbody rb;
    private AudioSource audioSource;

    private bool isOnGround = true;
    private bool isUpsideDown = false;
    private bool isFlipping = false;
    private bool isBall = false;
    private float targetRotationZ = 0f;

    // ปุ่มของแต่ละคน
    private Key jumpKey;
    private Key flipKey;
    private Key ballKey;

    private Coroutine immunityCo;
    private Coroutine speedBoostCo;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        curentHealth = Maxhealth;
        SetupKeys();
    }

    void Start()
    {
        if (animator != null) animator.SetFloat("Speed_f", 1.0f);
        ApplyBallState();
    }

    void SetupKeys()
    {
        if (playerID == PlayerID.Player1)
        {
            jumpKey = Key.Space;
            flipKey = Key.Q;
            ballKey = Key.W;
        }
        else // Player2
        {
            jumpKey = Key.UpArrow;
            flipKey = Key.DownArrow;
            ballKey = Key.RightArrow;
        }
    }

    void Update()
    {
        if (gameOver) return;
        if (Keyboard.current == null) return;

        // Ball toggle
        if (Keyboard.current[ballKey].wasPressedThisFrame)
        {
            isBall = !isBall;
            ApplyBallState();
        }

        // Jump
        if (Keyboard.current[jumpKey].wasPressedThisFrame && isOnGround && !isFlipping)
        {
            Vector3 jumpDir = isUpsideDown ? Vector3.down : Vector3.up;
            rb.AddForce(jumpForce * jumpDir, ForceMode.Impulse);
            isOnGround = false;
            animator.SetTrigger("Jump_trig");
            fxDirt.Stop();
            audioSource.PlayOneShot(jumpSound);
        }

        // Flip
        if (Keyboard.current[flipKey].wasPressedThisFrame && isOnGround && !isFlipping)
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

    void FixedUpdate()
    {
        // gravity ของแต่ละคน (แยกกัน)
        if (gameOver) return;

        Vector3 gravityDir = isUpsideDown ? Vector3.up : Vector3.down;
        rb.AddForce(gravityDir * gravityStrength, ForceMode.Acceleration);
    }

    void ApplyBallState()
    {
        if (normalModel != null) normalModel.SetActive(!isBall);
        if (ballModel != null)   ballModel.SetActive(isBall);
        if (boxCollider != null)    boxCollider.enabled    = !isBall;
        if (sphereCollider != null) sphereCollider.enabled = isBall;
    }

    private IEnumerator JumpAndFlip()
    {
        isFlipping = true;

        Vector3 jumpDir = isUpsideDown ? Vector3.down : Vector3.up;
        rb.linearVelocity = Vector3.zero; 
        rb.AddForce(flipJumpForce * jumpDir, ForceMode.Impulse);

        isOnGround = false;
        animator.SetTrigger("Jump_trig");
        fxDirt.Stop();
        audioSource.PlayOneShot(jumpSound);

        yield return new WaitForSeconds(flipDelay);
        FlipGravity();
        isFlipping = false;
    }

    public void FlipGravity()
    {
        isUpsideDown = !isUpsideDown;
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
        else if (collision.gameObject.CompareTag("Wall"))
        {
            InstaKill();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {   
            if (isImmune) return;
            TakeDamage(1);
            Debug.Log(playerID + " hit obstacle! Health: " + curentHealth);
            animator.SetInteger("DeathType_int", 1);
            fxDirt.Stop();
            Instantiate(fxExplosionPrefab, transform.position, Quaternion.identity);
            audioSource.PlayOneShot(crashSound);
        }
    }

    public void InstaKill()
    {
        if (gameOver) return;

        curentHealth = 0;
        gameOver = true;

        if (animator != null)
        {
            animator.SetBool("Death_b", true);
            animator.SetInteger("DeathType_int", 1);
        }

        fxDirt.Stop();
        Instantiate(fxExplosionPrefab, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(crashSound);

        if (GameManager.Instance != null)
            GameManager.Instance.PlayerLost(playerID);
    }

    public void TakeDamage(int damage)
    {
        if (gameOver) return;

        curentHealth -= damage;
        if (curentHealth <= 0)
        {
            curentHealth = 0;
            Debug.Log(playerID + " Game Over!");
            gameOver = true;
            animator.SetBool("Death_b", true);

            if (GameManager.Instance != null)
                GameManager.Instance.PlayerLost(playerID);
        }
    }
    //Item Effects

    public void Heal(int amount)
    {
        if (gameOver) return;
        curentHealth = Mathf.Min(curentHealth + amount, Maxhealth);
        Debug.Log(playerID + " Healed! Health: " + curentHealth);
    }

    public void ActivateImmunity()
    {
        if (immunityCo != null) StopCoroutine(immunityCo);
        immunityCo = StartCoroutine(ImmortalityCoroutine(immunityDuration));
    }

    public void ActivateSpeedBoost(float duration)
    {
        if (speedBoostCo != null) StopCoroutine(speedBoostCo);
        speedBoostCo = StartCoroutine(SpeedBoostCoroutine(duration));
    }
    public void AddScore(int amount)
    {
        if (gameOver) return;
        score += amount;
        Debug.Log(playerID + " collected coin! Score: " + score);
    }   
    private IEnumerator ImmortalityCoroutine(float duration)
    {
        isImmune = true;
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

        foreach (Renderer r in renderers) r.enabled = true;
        isImmune = false;
    }

    private IEnumerator SpeedBoostCoroutine(float duration)
    {
        float originalMultiplier = MoveLeft.speedMultiplier;
        MoveLeft.speedMultiplier = originalMultiplier * speedBoostMultiplier;
        Debug.Log(playerID + " Speed Boost! Multiplier: " + MoveLeft.speedMultiplier);

        yield return new WaitForSeconds(duration);

        MoveLeft.speedMultiplier = originalMultiplier;
        Debug.Log(playerID + " Speed Boost ended.");
    }
}