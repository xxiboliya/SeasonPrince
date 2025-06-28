using UnityEngine;
using TMPro;

public class FlowerCollectible : MonoBehaviour
{
    private static int flowerCount = 0;
    private static FlowerUI flowerUI;

    private void Start()
    {
        if (flowerUI == null)
        {
            flowerUI = FindObjectOfType<FlowerUI>();
        }
        // 初始化时刷新一次UI
        flowerUI?.SetFlowerCount(flowerCount);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            flowerCount++;
            if (flowerUI == null)
            {
                flowerUI = FindObjectOfType<FlowerUI>();
            }
            flowerUI?.SetFlowerCount(flowerCount);
            Destroy(gameObject);
        }
    }
}

public class FlowerCounter : MonoBehaviour
{
    public static FlowerCounter Instance { get; private set; }
    public int flowerCount = 0;
    public TMP_Text flowerText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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