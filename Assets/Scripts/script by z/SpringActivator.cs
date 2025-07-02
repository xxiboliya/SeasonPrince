using UnityEngine;

public class ObjectActivator : MonoBehaviour
{
    void Start()
    {
        string objName = PlayerPrefs.GetString("ActiveObject", "");
        // 先全部隐藏
        string[] allNames = { "1", "2", "3" };
        foreach (var name in allNames)
        {
            GameObject obj = GameObject.Find(name);
            if (obj != null) obj.SetActive(false);
        }
        // 激活目标
        if (!string.IsNullOrEmpty(objName))
        {
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
            // 没有设置则默认激活全部
            foreach (var name in allNames)
            {
                GameObject obj = GameObject.Find(name);
                if (obj != null) obj.SetActive(true);
            }
        }
    }
}