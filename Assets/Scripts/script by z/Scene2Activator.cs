using UnityEngine;

public class Scene2Activator : MonoBehaviour
{
    public GameObject[] componentsToActivate;

    void Start()
    {
        int index = SceneActivatorData.activateIndex;
        for (int i = 0; i < componentsToActivate.Length; i++)
        {
            if (i == index && componentsToActivate[i] != null)
                componentsToActivate[i].SetActive(true);
            else if (componentsToActivate[i] != null)
                componentsToActivate[i].SetActive(false);
        }
    }
}