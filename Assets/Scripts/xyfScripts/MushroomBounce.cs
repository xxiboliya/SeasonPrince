using UnityEngine;

public class MushroomBounce : MonoBehaviour
{
    [Tooltip("玩家被弹飞的竖直速度")]
    public float bounceVelocity = 16f;
    [Tooltip("判定玩家来自正上方的最大夹角（度）")]
    public float maxBounceAngle = 45f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        // 判定玩家是否在蘑菇顶端
        Transform player = other.transform;
        Vector2 contactDir = (player.position - transform.position).normalized;
        float angle = Vector2.Angle(contactDir, transform.up);
        if (angle < maxBounceAngle)
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = new Vector2(0f, bounceVelocity);
                var pc2d = rb.GetComponent<PlayerController2D>();
                if (pc2d != null)
                {
                    var isGroundedField = typeof(PlayerController2D).GetField("isGrounded", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (isGroundedField != null)
                        isGroundedField.SetValue(pc2d, false);
                }
            }
        }
    }
} 