using UnityEngine;

public class UIPanelSwitcher : MonoBehaviour
{
    public GameObject[] panels;

    public void ShowPanel(string panelName)
    {
        foreach (var panel in panels)
        {
            panel.SetActive(panel.name == panelName);
        }
    }
} 