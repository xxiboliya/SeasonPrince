using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelectButton : MonoBehaviour
{
    public void OnSelectMap(string objectName)
    {
        PlayerPrefs.SetString("ActiveObject", objectName);
        SceneManager.LoadScene("SampleScene");
    }
}