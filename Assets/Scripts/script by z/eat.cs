using UnityEngine;
using UnityEngine.UI;

public class Eat : MonoBehaviour
{
    // 在Inspector中设置能量名称
    public string energyName = "能量";
    // 静态计数器，所有Eat对象共享
    public static int energyCount = 0;
    private static Text energyText;

    void Start()
    {
        if (energyText == null)
        {
            GameObject textObj = GameObject.Find("EatText");
            if (textObj != null)
            {
                energyText = textObj.GetComponent<Text>();
            }
        }
        EatManager.RegisterEat(this);
        UpdateUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            energyCount++;
            UpdateUI();
            // 不直接销毁，而是隐藏
            gameObject.SetActive(false);
            // 记录已被吃掉，方便重置
            EatManager.RegisterEaten(this);
        }
    }

    // 更新UI显示
    void UpdateUI()
    {
        if (energyText != null)
        {
            energyText.text = energyName + "能量积累：" + energyCount;
        }
    }

    // 新增：重置计数方法
    public static void ResetCount()
    {
        energyCount = 0;
        if (energyText != null)
        {
            energyText.text = "";
        }
    }
}
