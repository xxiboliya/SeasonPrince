using UnityEngine;

public class MenuButtonController : MonoBehaviour
{
    public GameObject[] panels; // 0号为组件0，1-5为组件1-5

    /// <summary>
    /// 按钮点击时调用，参数index为1-5
    /// </summary>
    public void OnButtonClick(int index)
    {
        // 关闭组件0
        if (panels.Length > 0 && panels[0] != null)
            panels[0].SetActive(false);

        // 先全部关闭1-5
        for (int i = 1; i < panels.Length; i++)
        {
            if (panels[i] != null)
                panels[i].SetActive(false);
        }

        // 激活对应的组件
        if (index > 0 && index < panels.Length && panels[index] != null)
            panels[index].SetActive(true);
    }
}