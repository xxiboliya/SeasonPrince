using System.Collections.Generic;
using UnityEngine;

public class FoodRespawnManager : MonoBehaviour
{
    private static List<RespawnableFood> foods = new List<RespawnableFood>();

    public static void Register(RespawnableFood food)
    {
        if (!foods.Contains(food))
            foods.Add(food);
    }

    public static void RespawnAll()
    {
        foreach (var food in foods)
        {
            if (food != null)
                food.Respawn();
        }
    }
}