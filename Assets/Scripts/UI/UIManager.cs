using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject chapterSelectPanel;
    public GameObject characterPanel;
    public GameObject achievementsPanel;
    public GameObject settingsPanel;
    public GameObject chapter2LevelSelectPanel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Start with only the main menu active
        ShowMainMenu();
    }

    private void HideAllPanels()
    {
        if(mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if(chapterSelectPanel != null) chapterSelectPanel.SetActive(false);
        if(characterPanel != null) characterPanel.SetActive(false);
        if(achievementsPanel != null) achievementsPanel.SetActive(false);
        if(settingsPanel != null) settingsPanel.SetActive(false);
        if(chapter2LevelSelectPanel != null) chapter2LevelSelectPanel.SetActive(false);
    }

    public void ShowMainMenu()
    {
        HideAllPanels();
        if(mainMenuPanel != null) mainMenuPanel.SetActive(true);

        // 重置所有主菜单按钮的状态
        var buttonEffects = mainMenuPanel.GetComponentsInChildren<MenuButtonEffect>(true);
        foreach (var effect in buttonEffects)
        {
            effect.ResetToDefault();
        }
    }

    public void ShowChapterSelect()
    {
        HideAllPanels();
        if(chapterSelectPanel != null) chapterSelectPanel.SetActive(true);
    }

    public void ShowCharacterProfile()
    {
        HideAllPanels();
        if(characterPanel != null) characterPanel.SetActive(true);
    }

    public void ShowAchievements()
    {
        HideAllPanels();
        if(achievementsPanel != null) achievementsPanel.SetActive(true);
    }

    public void ShowSettings()
    {
        HideAllPanels();
        if(settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void ShowChapter2LevelSelect()
    {
        HideAllPanels();
        if(chapter2LevelSelectPanel != null) chapter2LevelSelectPanel.SetActive(true);
    }
}
