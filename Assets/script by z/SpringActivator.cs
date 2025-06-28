using UnityEngine;

public class MapObjectActivator : MonoBehaviour
{
    public GameObject[] objects; // 0号无用，1-3号分别对应1、2、3

    void Start()
    {
        int index = PlayerPrefs.GetInt("ActiveIndex", 0);
        Debug.Log("激活index: " + index);
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] != null)
                objects[i].SetActive(i == index);
        }
        MusicManager.Instance.PlayMusic1();
    }
}