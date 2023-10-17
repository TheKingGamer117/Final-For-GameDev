using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    private Vector3 direction;
    private Collider2D col;

    private void Start()
    {
        col = gameObject.GetComponent<BoxCollider2D>();
        if (col == null)
        {
            Debug.LogError("BoxCollider2D component is missing from the GameObject.");
        }
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       // Debug.Log("Projectile hit something");
        //Debug.Log("Collided with: " + collision.gameObject.name);

        // Try to get the Fish component from the parent if it's not found on the original GameObject.
        Fish fish = collision.GetComponent<Fish>() ?? collision.transform.parent.GetComponent<Fish>();

        if (fish != null)
        {
            //Debug.Log("Projectile hit a fish");
            fish.OnAttacked();
        }
        Destroy(gameObject);
    }

}
