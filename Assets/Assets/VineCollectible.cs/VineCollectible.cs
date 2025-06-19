using UnityEngine;

public class VineCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController2D player = other.GetComponent<PlayerController2D>();
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            VineInteractable vine = GetComponent<VineInteractable>();
            if (player != null && rb != null && vine != null)
                vine.AttachPlayer(other.transform, rb);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            VineInteractable vine = GetComponent<VineInteractable>();
            if (vine != null)
                vine.DetachPlayer();
        }
    }
}