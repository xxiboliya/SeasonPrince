using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueUI : MonoBehaviour
{
    public GameObject panel;
    public Text dialogueText;
    public Button optionA;
    public Button optionB;
    public float typeSpeed = 0.05f; // 新增：打字机速度，可在Inspector调节
    public float buttonDelay = 1f;  // 新增：按钮延迟出现时间

    private System.Action onAAction;
    private System.Action onBAction;
    private Coroutine typeCoroutine;

    public delegate void DialogueEvent(bool isActive);
    public static event DialogueEvent OnDialogueActiveChanged;

    void Start()
    {
        panel.SetActive(false);
    }

    public void Show(string text, System.Action onA, System.Action onB)
    {
        panel.SetActive(true);
        dialogueText.text = "";
        optionA.gameObject.SetActive(false);
        optionB.gameObject.SetActive(false);

        onAAction = onA;
        onBAction = onB;

        // 禁止玩家移动
        OnDialogueActiveChanged?.Invoke(true);

        if (typeCoroutine != null)
            StopCoroutine(typeCoroutine);
        typeCoroutine = StartCoroutine(TypeTextAndShowButtons(text));
    }

    IEnumerator TypeTextAndShowButtons(string text)
    {
        // 打字机效果
        for (int i = 0; i <= text.Length; i++)
        {
            dialogueText.text = text.Substring(0, i);
            yield return new WaitForSeconds(typeSpeed); // 使用可调节速度
        }
        // 等待按钮延迟时间
        yield return new WaitForSeconds(buttonDelay);

        optionA.gameObject.SetActive(true);
        optionB.gameObject.SetActive(true);

        optionA.onClick.RemoveAllListeners();
        optionB.onClick.RemoveAllListeners();
        optionA.onClick.AddListener(() => { onAAction(); Close(); });
        optionB.onClick.AddListener(() => { onBAction(); Close(); });
    }

    void Close()
    {
        panel.SetActive(false);
        // 恢复玩家移动
        OnDialogueActiveChanged?.Invoke(false);
    }
} 