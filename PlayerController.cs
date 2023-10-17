using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public Rigidbody2D rigidbody2D; // Rigidbody component

    [SerializeField]
    private float moveSpeed = 5f; // Regular movement speed

    [SerializeField]
    private float doubleClickTime = 0.2f; // Time window for double click, editable in Inspector

    [SerializeField]
    private float dashDistance = 5f; // Distance to dash on double click, editable in Inspector

    private float lastClickTimeW = 0f;
    private float lastClickTimeA = 0f;
    private float lastClickTimeS = 0f;
    private float lastClickTimeD = 0f;

    public GameObject projectilePrefab;  // Drag your Projectile prefab here in the inspector
    private Camera mainCamera;


    [SerializeField]
    private float maxY = 1;  // Set this to the y-coordinate of the water's surface

    private SpriteRenderer spriteRenderer; // SpriteRenderer component

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main; // Initialize main camera
    }

    void Update()
    {
        // Regular movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        rigidbody2D.velocity = movement * moveSpeed;

        // Get mouse position in world coordinates and calculate direction
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

        // Make the player's "right" vector point toward the mouse cursor
        transform.right = direction;

        // Flip sprite based on the mouse position relative to the player
        Vector3 localScale = transform.localScale;
        if (mousePosition.x < transform.position.x)
        {
            // If the mouse is to the left of the player
            localScale.y = -1f;
        }
        else
        {
            // If the mouse is to the right of the player
            localScale.y = 1f;
        }
        transform.localScale = localScale;

        // Check for double click to dash
        CheckForDoubleClick(KeyCode.W, Vector2.up);
        CheckForDoubleClick(KeyCode.A, Vector2.left);
        CheckForDoubleClick(KeyCode.S, Vector2.down);
        CheckForDoubleClick(KeyCode.D, Vector2.right);

        // Prevent going above water surface
        if (transform.position.y >= maxY)
        {
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
            if (rigidbody2D.velocity.y > 0)
            {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
            }
        }

        // Shoot on left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Treasure")
        {
            // Assuming you have a currency variable in PlayerStats
            PlayerStats stats = GetComponent<PlayerStats>();
            if (stats)
            {
                stats.AddGold(50);
            }
            Destroy(collision.gameObject);
        }
    }

    void Shoot()
    {
        //Debug.Log("Shoot method called");  // Debug log

        // Offset the projectile position slightly in the direction the player is facing
        Vector3 spawnPosition = transform.position + (transform.right * 1f);

        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        if (projectile == null)
        {
            Debug.LogError("Projectile could not be instantiated.");
            return;
        }
        else
        {
            //Debug.Log("Projectile instantiated at position: " + projectile.transform.position);
        }

        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.SetDirection(transform.right);
        }
        else
        {
            Debug.LogError("Projectile script not found on the instantiated object.");
        }

        AudioManagerMainScene.instance.PlayShootSFX();
    }

    private void CheckForDoubleClick(KeyCode key, Vector2 direction)
    {
        float lastClickTime = 0;

        // Get the appropriate last click time based on the key
        switch (key)
        {
            case KeyCode.W: lastClickTime = lastClickTimeW; break;
            case KeyCode.A: lastClickTime = lastClickTimeA; break;
            case KeyCode.S: lastClickTime = lastClickTimeS; break;
            case KeyCode.D: lastClickTime = lastClickTimeD; break;
        }

        // Check for double click
        if (Input.GetKeyDown(key))
        {
            if (Time.time - lastClickTime < doubleClickTime)
            {
                // Perform dash
                rigidbody2D.position += direction * dashDistance;
            }

            // Update last click time
            lastClickTime = Time.time;
        }

        // Update the appropriate last click time based on the key
        switch (key)
        {
            case KeyCode.W: lastClickTimeW = lastClickTime; break;
            case KeyCode.A: lastClickTimeA = lastClickTime; break;
            case KeyCode.S: lastClickTimeS = lastClickTime; break;
            case KeyCode.D: lastClickTimeD = lastClickTime; break;
        }
    }
}
