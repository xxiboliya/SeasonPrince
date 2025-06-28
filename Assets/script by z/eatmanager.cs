using System.Collections.Generic;
using UnityEngine;

public class EatManager : MonoBehaviour
{
    private static List<Eat> allEats = new List<Eat>();
    private static List<Eat> eatenEats = new List<Eat>();

    public static void RegisterEat(Eat eat)
    {
        if (!allEats.Contains(eat))
            allEats.Add(eat);
    }

    public static void RegisterEaten(Eat eat)
    {
        if (!eatenEats.Contains(eat))
            eatenEats.Add(eat);
    }

    // 重新激活所有被吃掉的物体
    public static void ResetEats()
    {
        foreach (var eat in eatenEats)
        {
            if (eat != null)
                eat.gameObject.SetActive(true);
        }
        eatenEats.Clear();
    }
}
