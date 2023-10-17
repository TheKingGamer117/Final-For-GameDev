using UnityEngine;

public class Fish : MonoBehaviour
{
    [Header("Attributes")]
    public int minHealth;
    public int maxHealth;
    public float minSpeed;
    public float maxSpeed;
    public int minPackSize;
    public int maxPackSize;

    [Header("Components")]
    public Transform bodyTransform;  // Assign this in the inspector
    public AnimationClip animationClip;

    [Header("Movement")]
    public float boundary = 1000f; // distance at which fish will turn around
    protected bool moveForward = true; // whether fish is currently moving forward

    [Header("Dead Prefab")]
    public GameObject deadFishPrefab;

    public int health;
    public float speed;
    public int packSize;

    private Animator animator;  // Animator component

    public void Initialize()
    {
        this.health = Random.Range(minHealth, maxHealth + 1);
        this.speed = Random.Range(minSpeed, maxSpeed);
        this.packSize = Random.Range(minPackSize, maxPackSize + 1);

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on this GameObject.");
        }
        else
        {
            if (animationClip != null)
            {
                animator.Play(animationClip.name);  // Play the animation clip for this specific fish
            }
            else
            {
                Debug.LogWarning("Animation clip is not set for this fish.");
            }
        }

    }

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        Move();
    }

    public virtual void Move()
    {
        float moveDirection = moveForward ? 1 : -1;

        if (bodyTransform != null)
        {
            Vector3 bodyScale = bodyTransform.localScale;

            if ((moveForward && bodyScale.x < 0) || (!moveForward && bodyScale.x > 0))
            {
                // Flip the scales
                bodyScale.x *= -1;

                bodyTransform.localScale = bodyScale;
            }
        }

        // Using Vector3.right to move along the X-axis
        Vector3 moveVector = Vector3.right * moveDirection * speed * Time.deltaTime;
        transform.localPosition += moveVector;

        //Debug.Log($"Fish {gameObject.name} is trying to move. Position: {transform.localPosition}, Speed: {speed}");

        // Check for boundary conditions along the X-axis
        if (moveForward && transform.localPosition.x > boundary)
        {
            moveForward = false;
            //Debug.Log($"Fish {gameObject.name} hit the forward boundary.");
        }
        else if (!moveForward && transform.localPosition.x < -boundary)
        {
            moveForward = true;
            //Debug.Log($"Fish {gameObject.name} hit the backward boundary.");
        }
    }

    public virtual void OnAttacked()
    {
        // Reduce health
        health -= 25;

        // Check for death
        if (health <= 0)
        {
            if (deadFishPrefab != null)
            {
                // Instantiate the dead fish prefab and get its GameObject reference
                GameObject deadFish = Instantiate(deadFishPrefab, transform.position, transform.rotation);

                // Add the DeadFishBehavior script to the dead fish GameObject
                deadFish.AddComponent<DeadFishBehavior>();
            }
            else
            {
                Debug.LogWarning($"Dead fish prefab is not set for {gameObject.name}");
            }

            // Destroy the original fish GameObject
            Destroy(gameObject);
        }
    }

}
