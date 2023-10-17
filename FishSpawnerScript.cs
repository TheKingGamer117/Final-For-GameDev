using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject[] upperLevelFishPrefabs;
    public GameObject[] middleLevelFishPrefabs;
    public GameObject[] lowerLevelFishPrefabs;

    public Transform[] upperLevelSpawnPoints;
    public Transform[] middleLevelSpawnPoints;
    public Transform[] lowerLevelSpawnPoints;

    [SerializeField]
    public float yOffset = 10f;  // Y-offset for each tier of fish

    void Start()
    {
        SpawnFish(upperLevelFishPrefabs, upperLevelSpawnPoints, 0);
        SpawnFish(middleLevelFishPrefabs, middleLevelSpawnPoints, -yOffset);
        SpawnFish(lowerLevelFishPrefabs, lowerLevelSpawnPoints, -2 * yOffset);
    }

    void SpawnFish(GameObject[] fishPrefabs, Transform[] spawnPoints, float yLevelOffset)
    {
        float typeOffset = 0.0f;

        // Count downwards through the array
        for (int i = fishPrefabs.Length - 1; i >= 0; i--)
        {
            GameObject fishPrefab = fishPrefabs[i];
            float adjustedYLevelOffset = yLevelOffset + typeOffset;

            foreach (Transform spawnPoint in spawnPoints)
            {
                Vector3 spawnPositionBase = new Vector3(spawnPoint.position.x, spawnPoint.position.y + adjustedYLevelOffset, spawnPoint.position.z);
                int packSize = fishPrefab.GetComponent<Fish>().packSize;

                for (int j = 0; j < packSize; j++)
                {
                    Vector3 spawnPosition = spawnPositionBase;
                    GameObject fishInstance = Instantiate(fishPrefab, spawnPosition, Quaternion.identity);
                    Fish fishScript = fishInstance.GetComponent<Fish>();
                    fishScript.Initialize();
                }
            }

            typeOffset += 6.0f;
        }
    }
}
