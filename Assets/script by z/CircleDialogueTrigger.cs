using UnityEngine;

public class CircleDialogueTrigger : MonoBehaviour
{
    public DialogueUI dialogueUI;
    public string dialogue = "你好，选择你的选项：";
    public int favorA = 5;
    public int favorB = 2;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueUI.Show(
                dialogue,
                () => FavorManager.Instance.AddFavor(favorA),
                () => FavorManager.Instance.AddFavor(favorB)
            );
        }
    }
} 