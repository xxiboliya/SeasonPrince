using UnityEngine;

public class FavorManager : MonoBehaviour
{
    public static FavorManager Instance;
    public int character1Favor = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddFavor(int amount)
    {
        character1Favor += amount;
        Debug.Log("人物1好感度：" + character1Favor);
    }
} 