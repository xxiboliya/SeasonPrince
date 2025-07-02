using UnityEngine;

public class RespawnableFood : MonoBehaviour
{
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
        FoodRespawnManager.Register(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }

    // 供管理器调用
    public void Respawn()
    {
        transform.position = originalPosition;
        gameObject.SetActive(true);
    }
}