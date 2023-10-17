using UnityEngine;

public class UpperLevelFish : Fish
{
    [Header("Player Interaction")]
    public float fleeDistance = 10.0f;
    public float fleeAngleThreshold = 60f;

    private float halfWidth;

    private GameObject player;

    void Start()
    {
        Initialize();
        player = GameObject.FindGameObjectWithTag("Player");

        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            halfWidth = rend.bounds.extents.x;
        }
        else
        {
            Debug.LogWarning("Renderer component not found, using default halfWidth value of 1.0f");
            halfWidth = 1.0f;
        }
    }

    public override void Move()
    {
        base.Move();

        if (player != null)
        {
            // Offset the position based on halfWidth and direction of movement
            Vector3 offsetPosition = transform.position + (transform.right * (moveForward ? 1 : -1) * halfWidth);

            // Calculate vector and distance to player from offset position
            Vector3 toPlayer = player.transform.position - offsetPosition;
            float distanceToPlayer = toPlayer.magnitude - halfWidth;  // Include halfWidth to take the size into account

            // Calculate the angle to the player
            float angleToPlayer = Vector3.Angle(transform.right * (moveForward ? 1 : -1), toPlayer);

            if (distanceToPlayer < fleeDistance && angleToPlayer < fleeAngleThreshold)
            {
                Flee();
            }
        }
    }

    void Flee()
    {
        //Debug.Log($"Fish {gameObject.name} is fleeing from the player.");
        moveForward = !moveForward;
    }
}
