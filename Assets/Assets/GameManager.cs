using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject player;
    public Transform spawnPoint;
    public GameObject deathUIPanel;
    public Button restartButton;

    private Rigidbody2D playerRb;
    private PlayerController2D playerController;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: if you have multiple scenes and want the manager to persist.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {MusicManager.Instance.PlayMusic1();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
            playerController = player.GetComponent<PlayerController2D>();
        }

        // Initially, the death panel should be hidden.
        if(deathUIPanel != null)
        {
            deathUIPanel.SetActive(false);
        }

        // Add a listener to the restart button.
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(Restart);
        }
    }

    public void PlayerDied()
    {MusicManager.Instance.PlayMusic2();
        // Show the death UI panel
        if(deathUIPanel != null)
        {
            deathUIPanel.SetActive(true);
        }
        
        // Freeze the player instead of pausing the game
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero;
            playerRb.isKinematic = true;
        }
    }

    public void Restart()
    {MusicManager.Instance.PlayMusic1();
        // Hide the death UI panel
        if(deathUIPanel != null)
        {
            deathUIPanel.SetActive(false);
        }

        // Resume player controls and physics
        if (player != null && spawnPoint != null)
        {
            // Re-enable player controls
            if (playerController != null)
            {
                playerController.enabled = true;
            }

            // Move player to spawn point and reset physics state
            player.transform.position = spawnPoint.position;
            if (playerRb != null)
            {
                playerRb.isKinematic = false;
                playerRb.velocity = Vector2.zero;
            }
        }
    }
} 