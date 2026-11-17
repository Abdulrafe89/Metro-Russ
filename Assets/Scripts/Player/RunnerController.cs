using System.Collections;
using UnityEngine;

public class RunnerController : MonoBehaviour
{
    public static RunnerController Instance { get; private set; }

    public float LaneWidth = 3.5f;
    public float JumpStrength = 6.5f;
    public float SlideDuration = 0.8f;
    public float GroundY = 1.1f;

    private Rigidbody rb;
    private float targetX;
    private int laneIndex = 1;
    private bool isGrounded = true;
    private bool isSliding = false;
    private float slideTimer;
    private float magnetTimer;
    private float hoverboardTimer;
    private float jetpackTimer;
    private float shieldTimer;

    private CapsuleCollider capsuleCollider;
    private Vector3 originalCenter;
    private float originalHeight;

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        originalCenter = capsuleCollider.center;
        originalHeight = capsuleCollider.height;
        targetX = transform.position.x;
    }

    private void Update()
    {
        if (rb == null)
        {
            return;
        }

        if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameManager.GameState.Running)
        {
            return;
        }

        HandleInput();

        float currentX = Mathf.Lerp(transform.position.x, targetX, Time.deltaTime * 12f);
        transform.position = new Vector3(currentX, transform.position.y, transform.position.z);

        if (magnetTimer > 0f)
        {
            magnetTimer -= Time.deltaTime;
        }

        if (hoverboardTimer > 0f)
        {
            hoverboardTimer -= Time.deltaTime;
            if (transform.position.y < 2.2f)
            {
                transform.position = new Vector3(transform.position.x, 2.2f, transform.position.z);
            }
        }

        if (jetpackTimer > 0f)
        {
            jetpackTimer -= Time.deltaTime;
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Max(rb.velocity.y, 2.8f), rb.velocity.z);
        }

        if (shieldTimer > 0f)
        {
            shieldTimer -= Time.deltaTime;
        }

        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0f)
            {
                isSliding = false;
                capsuleCollider.height = originalHeight;
                capsuleCollider.center = originalCenter;
            }
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftControl))
        {
            Slide();
        }
    }

    public void MoveLeft()
    {
        laneIndex = Mathf.Max(0, laneIndex - 1);
        targetX = laneIndex * LaneWidth - LaneWidth;
    }

    public void MoveRight()
    {
        laneIndex = Mathf.Min(2, laneIndex + 1);
        targetX = laneIndex * LaneWidth - LaneWidth;
    }

    public void Jump()
    {
        if (!isGrounded || isSliding)
        {
            return;
        }

        isGrounded = false;
        rb.velocity = new Vector3(rb.velocity.x, JumpStrength, rb.velocity.z);
    }

    public void Slide()
    {
        if (!isGrounded || isSliding)
        {
            return;
        }

        isSliding = true;
        slideTimer = SlideDuration;
        capsuleCollider.height = originalHeight * 0.6f;
        capsuleCollider.center = new Vector3(originalCenter.x, 0.3f, originalCenter.z);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.name.Contains("Track"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.name.Contains("Train") || collision.gameObject.name.Contains("Barrier"))
        {
            if (shieldTimer > 0f)
            {
                shieldTimer = 0f;
                Destroy(collision.gameObject);
                return;
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.EndGame();
            }
        }
    }

    public void ApplyPowerUp(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.Magnet:
                magnetTimer = 8f;
                break;
            case PowerUpType.Hoverboard:
                hoverboardTimer = 7f;
                break;
            case PowerUpType.Jetpack:
                jetpackTimer = 6f;
                break;
            case PowerUpType.Shield:
                shieldTimer = 6f;
                break;
        }
    }

    public void ResetState()
    {
        laneIndex = 1;
        targetX = 0f;
        isGrounded = true;
        shieldTimer = 0f;
        magnetTimer = 0f;
        hoverboardTimer = 0f;
        jetpackTimer = 0f;
        slideTimer = 0f;
        isSliding = false;
        if (capsuleCollider != null)
        {
            capsuleCollider.height = originalHeight;
            capsuleCollider.center = originalCenter;
        }
    }
}

public enum PowerUpType
{
    Magnet,
    Hoverboard,
    Jetpack,
    Shield
}
