using UnityEngine;

public class BoatController : MonoBehaviour
{
    public GameObject sellButton; // Drag your UI Button here

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            sellButton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            sellButton.SetActive(false);
        }
    }
}
