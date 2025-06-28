using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathSceneReloader : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
} 