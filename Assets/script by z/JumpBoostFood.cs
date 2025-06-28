using UnityEngine;

public class JumpBoostFood : MonoBehaviour
{
    public float jumpPower = 15f; // z距离

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            }
            // 可选：吃掉后隐藏或销毁
            gameObject.SetActive(false);
        }
    }
}