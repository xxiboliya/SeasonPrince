using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    public void OnStartGame()
    {
        SceneManager.LoadScene("MapSelectScene");
        FoodRespawnManager.RespawnAll();
    }
}