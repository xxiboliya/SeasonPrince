using UnityEngine;
using TMPro;

public class eat : MonoBehaviour
{
    // 静态变量用于全局计数
    private static int totalCount = 0;
    // 静态UI引用
    private static TMP_Text countText;

    // 在场景开始时查找UI
    private void Start()
    {
        if (countText == null)
        {
            // 这里用你UI组件的名字
            GameObject uiObj = GameObject.Find("EatCountText");
            if (uiObj != null)
                countText = uiObj.GetComponent<TMP_Text>();
        }
        UpdateUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            totalCount++;
            UpdateUI();
            Destroy(gameObject);
        }
    }

    private void UpdateUI()
    {
        if (countText != null)
        {
            countText.text = "吃掉数量: " + totalCount;
        }
    }
}