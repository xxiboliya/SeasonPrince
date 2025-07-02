using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Died: Hit DeathZone or Spike");
            // SceneManager.LoadScene("DeathScene");
            GameManager.Instance.PlayerDied();
        }
    }
} 