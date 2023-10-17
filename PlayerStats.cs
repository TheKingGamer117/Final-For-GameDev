using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI oxygenText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI goldText;

    [Header("Player Stats")]
    public float oxygen = 100f;
    public float health = 100f;
    private float maxY = 0f;
    public float food = 100f;
    public float water = 100f;
    public int gold = 0;

    [Header("Inventory & Equipment")]
    //public Inventory playerInventory;

    public GameObject treasurePrefab; // Assign the Treasure prefab in the inspector
    private Vector3 treasureSpawnLocation; // Location where the treasure was destroyed

    private void Start()
    {
        goldText.text = $"Gold: {gold}";
    }

    private void Update()
    {
        if (transform.position.y < maxY)
        {
            oxygen -= Time.deltaTime;
            if (oxygen < 0)
            {
                oxygen = 0;
                health -= Time.deltaTime;
            }
        }
        else
        {
            oxygen += Time.deltaTime;
            if (oxygen > 100)
            {
                oxygen = 100;
            }
        }

        if (health <= 0)
        {
            health = 0;
            // Uncomment to switch to MainMenuScene
            // SceneManager.LoadScene("MainMenuScene");
        }

        oxygenText.text = $"Oxygen: {Mathf.Floor(oxygen)}";
        healthText.text = $"Health: {Mathf.Floor(health)}";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Treasure")
        {
            AddGold(50);
            treasureSpawnLocation = collision.transform.position;
            Destroy(collision.gameObject);
            Invoke("RespawnTreasure", 5f); // Invoke RespawnTreasure method after 5 seconds
        }
    }

    private void RespawnTreasure()
    {
        Instantiate(treasurePrefab, treasureSpawnLocation, Quaternion.identity);
    }

    public void AddGold(int amount)
    {
        gold += amount;
        goldText.text = $"Gold: {gold}";
    }
}
