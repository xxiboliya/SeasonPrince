
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OpenChapterSelect()
    {
        Debug.Log("OpenChapterSelect button clicked!");
        if (UIManager.Instance != null) UIManager.Instance.ShowChapterSelect();
    }

    public void OpenCharacterProfile()
    {
        Debug.Log("OpenCharacterProfile button clicked!");
        if (UIManager.Instance != null) UIManager.Instance.ShowCharacterProfile();
    }

    public void OpenAchievements()
    {
        Debug.Log("OpenAchievements button clicked!");
        if (UIManager.Instance != null) UIManager.Instance.ShowAchievements();
    }

    public void OpenSettings()
    {
        Debug.Log("OpenSettings button clicked!");
        if (UIManager.Instance != null) UIManager.Instance.ShowSettings();
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame button clicked!");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
