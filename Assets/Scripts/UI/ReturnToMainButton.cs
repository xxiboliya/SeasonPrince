
using UnityEngine;

public class ReturnToMainButton : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        Debug.Log("Return to Main Menu button clicked!");
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowMainMenu();
        }
        else
        {
            Debug.LogError("UIManager.Instance is not found in the scene! Make sure a UIManager object with the script exists.");
        }
    }
}
