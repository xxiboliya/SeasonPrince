using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public GameObject panel;
    public Text dialogueText;
    public Button optionA;
    public Button optionB;

    void Start()
    {
        panel.SetActive(false);
    }

    public void Show(string text, System.Action onA, System.Action onB)
    {
        panel.SetActive(true);
        dialogueText.text = text;
        optionA.onClick.RemoveAllListeners();
        optionB.onClick.RemoveAllListeners();
        optionA.onClick.AddListener(() => { onA(); panel.SetActive(false); });
        optionB.onClick.AddListener(() => { onB(); panel.SetActive(false); });
    }
} 