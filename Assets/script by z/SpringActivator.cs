using UnityEngine;

public class ObjectActivator : MonoBehaviour
{
    void Start()
    {
        string objName = PlayerPrefs.GetString("ActiveObject", "");
        if (!string.IsNullOrEmpty(objName))
        {
            // 先全部隐藏
            GameObject obj1 = GameObject.Find("1");
            GameObject obj2 = GameObject.Find("2");
            if (obj1 != null) obj1.SetActive(false);
            if (obj2 != null) obj2.SetActive(false);

            // 激活目标
            GameObject obj = GameObject.Find(objName);
            if (obj != null)
            {
                obj.SetActive(true);
            }
            else
            {
                Debug.LogWarning("未找到对象: " + objName);
            }
        }
        else
        {
            Debug.LogWarning("未设置ActiveObject参数");
        }
    }
}