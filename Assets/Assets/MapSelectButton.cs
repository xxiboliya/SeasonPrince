using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelectButton : MonoBehaviour
{
    public void OnSelectMap(int index)
    {
        PlayerPrefs.SetInt("ActiveIndex", index);
        SceneManager.LoadScene("SampleScene");
    }
}