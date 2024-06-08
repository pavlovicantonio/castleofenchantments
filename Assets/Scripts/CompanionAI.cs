using Pathfinding;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CompanionAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 100f; // Maximum distance for following the player
    public float pathUpdateSeconds = 0.5f;
    public float followDistance = 1;  // Desired distance to keep the companion from the player
    public float maxDistance = 20f;  // Maximum distance before teleporting back to the player

    [Header("Physics")]
    public float speed = 6f, jumpForce = 10f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.6f;
    public float jumpModifier = 0.2f;
    public float jumpCheckOffset = 2f;

    [Header("Custom Behavior")]
    public bool followEnabled = false; // Initially not following
    public bool jumpEnabled = true, isJumping, isInAir;
    public bool directionLookEnabled = true;

    [Header("Taming")]
    public float tamingDistance = 3f;
    public KeyCode tameKey = KeyCode.F;
    public GameObject tamePrompt; // Reference to the UI Text GameObject
    public bool isTamed = false; // Initially not tamed

    [SerializeField] Vector3 startOffset;

    private Path path;
    private int currentWaypoint = 0;
    [SerializeField] public RaycastHit2D isGrounded;
    Seeker seeker;
    Rigidbody2D rb;
    private bool isOnCoolDown;
    private Vector2 currentVelocity;

    private Collider2D companionCollider;
    private Collider2D playerCollider;
    private Animator animator;

    // New variable to track the previous state of the tame prompt
    private bool wasTamePromptActive = false;

    public void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        companionCollider = GetComponent<Collider2D>();
        playerCollider = target.GetComponent<Collider2D>();

        if (companionCollider != null && playerCollider != null)
        {
            Physics2D.IgnoreCollision(companionCollider, playerCollider, true);
        }

        isJumping = false;
        isInAir = false;
        isOnCoolDown = false;

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void FixedUpdate()
    {
        if (followEnabled && TargetInDistance())
        {
            PathFollow();
        }

        // Check if the companion is too far from the player and teleport if necessary
        if (isTamed && Vector2.Distance(transform.position, target.position) > maxDistance)
        {
            TeleportToPlayer();
        }

        // Taming logic
        if (!followEnabled && Vector2.Distance(transform.position, target.position) <= tamingDistance)
        {
            tamePrompt.SetActive(true);

            // Check if the tame prompt was previously inactive
            if (!wasTamePromptActive)
            {
                FindObjectOfType<AudioManager>().Play("PetGrowl"); // Play pet growl sound
                wasTamePromptActive = true; // Update the state
            }

            if (Input.GetKeyDown(tameKey))
            {
                isTamed = true; // Tame the companion
                followEnabled = true; // Follow is enabled
                tamePrompt.SetActive(false); // Prompt is not visible
                FindObjectOfType<AudioManager>().Play("PetTamed"); // Play pet tamed sound
                wasTamePromptActive = false; // Reset the state
            }
        }
        else
        {
            tamePrompt.SetActive(false);
            wasTamePromptActive = false; // Reset the state if the player moves away
        }
    }

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.position);

            // Check if the companion is within the follow distance
            if (distanceToTarget > followDistance)
            {
                Vector2 directionToTarget = (target.position - transform.position).normalized;
                Vector2 targetPosition = (Vector2)target.position - directionToTarget * followDistance;
                seeker.StartPath(rb.position, targetPosition, OnPathComplete);
            }
        }
    }

    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            animator.SetBool("isRunning", false);
            return;
        }

        // See if colliding with anything
        startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset, transform.position.z);
        isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f);

        // Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed;

        // Determine if the companion should be running
        bool isRunning = direction.sqrMagnitude > 0.1f;
        animator.SetBool("isRunning", isRunning);
        FindObjectOfType<AudioManager>().Play("PetRun"); // Play pet run sound

        rb.velocity = Vector2.SmoothDamp(rb.velocity, force, ref currentVelocity, 0.5f);
        rb.AddForce(force);

        // Jump
        if (jumpEnabled && isGrounded && !isInAir && !isOnCoolDown)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                if (isInAir) return;
                isJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                StartCoroutine(JumpCoolDown());
            }
        }

        if (isGrounded)
        {
            isJumping = false;
            isInAir = false;
        }
        else
        {
            isInAir = true;
        }

        // Movement
        rb.velocity = new Vector2(force.x, rb.velocity.y);

        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Direction Graphics Handling
        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    // Teleportation
    private void TeleportToPlayer()
    {
        transform.position = target.position;
        currentWaypoint = 0;
        rb.velocity = Vector2.zero;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    IEnumerator JumpCoolDown()
    {
        isOnCoolDown = true;
        yield return new WaitForSeconds(1f);
        isOnCoolDown = false;
    }
}
