using UnityEngine;

public class MenuButtonController : MonoBehaviour
{
    public GameObject[] panels; // 0��Ϊ���0��1-5Ϊ���1-5

    /// <summary>
    /// ��ť���ʱ���ã�����indexΪ1-5
    /// </summary>
    public void OnButtonClick(int index)
    {
        // �ر����0
        if (panels.Length > 0 && panels[0] != null)
            panels[0].SetActive(false);

        // ��ȫ���ر�1-5
        for (int i = 1; i < panels.Length; i++)
        {
            if (panels[i] != null)
                panels[i].SetActive(false);
        }

        // �����Ӧ�����
        if (index > 0 && index < panels.Length && panels[index] != null)
            panels[index].SetActive(true);
    }
}