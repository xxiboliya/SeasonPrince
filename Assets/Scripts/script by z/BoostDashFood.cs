using UnityEngine;

public class BoostDashFood : MonoBehaviour
{
    public float speedMultiplier = 2f; // x倍速度
    public float dashDistance = 5f;    // y距离

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                PlayerController2D player = other.GetComponent<PlayerController2D>();
                if (player != null)
                {
                    float direction = Mathf.Sign(rb.velocity.x);
                    if (direction == 0) direction = 1; // 默认向右
                    float dashSpeed = player.moveSpeed * speedMultiplier;
                    rb.velocity = new Vector2(dashSpeed * direction, rb.velocity.y);
                    // 可选：用协程或定时器实现“冲出y距离”后减速，这里简单实现
                }
            }
            // 可选：吃掉后隐藏或销毁
            gameObject.SetActive(false);
        }
    }
}