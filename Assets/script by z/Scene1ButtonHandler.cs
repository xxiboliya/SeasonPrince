using UnityEngine;
using UnityEngine.SceneManagement;
public static class SceneActivatorData
{
    public static int activateIndex = -1;
}
public class Scene1ButtonHandler : MonoBehaviour
{
    public string targetSceneName = "SampleScene2"; // 填你的场景2名字

    // 按钮点击时调用，index为要激活的组件编号
    public void SwitchSceneAndSetIndex(int index)
    {
        SceneActivatorData.activateIndex = index;
        SceneManager.LoadScene(targetSceneName);
    }
}