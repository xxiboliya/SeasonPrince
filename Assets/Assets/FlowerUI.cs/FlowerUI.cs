using UnityEngine;
using UnityEngine.UI;

public class FlowerUI : MonoBehaviour
{
    public int flowerCount = 0;
    public Text flowerText;
    private static FlowerUI _instance;
    public static FlowerUI Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateFlowerUI();
    }

    public void AddFlower()
    {
        flowerCount++;
        UpdateFlowerUI();
    }

    private void UpdateFlowerUI()
    {
        if (flowerText != null)
        {
            flowerText.text = flowerCount.ToString();
        }
    }
} 