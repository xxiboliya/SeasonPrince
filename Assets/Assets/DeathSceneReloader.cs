using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathSceneReloader : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            var player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                var pc = player.GetComponent<PlayerController2D>();
                if (pc != null)
                    pc.ResetDeathFlag();
            }
            SceneManager.LoadScene("SampleScene");
        }
    }
} 