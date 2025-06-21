using UnityEngine;
using UnityEngine.UI;

public class FlowerCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FlowerUI.Instance.AddFlower();
            Destroy(gameObject);
        }
    }
}

public class FlowerCounter : MonoBehaviour
{
    public static FlowerCounter Instance { get; private set; }
    public int flowerCount = 0;
    public Text flowerText;

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