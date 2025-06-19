using UnityEngine;

public class IcePath : MonoBehaviour
{
    [Tooltip("加速度倍数")] public float accelMultiplier = 2f;
    [Tooltip("速度上限倍数")] public float maxSpeedMultiplier = 2f;
    [Tooltip("反向减速速率系数（0.5为减半）")] public float reverseDecelMultiplier = 0.5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            var pc2d = collision.collider.GetComponent<PlayerController2D>();
            if (pc2d != null)
            {
                pc2d.moveSpeed *= maxSpeedMultiplier;
                pc2d.maxMoveSpeed *= maxSpeedMultiplier;
                pc2d.accelRate *= accelMultiplier;
                // 反向减速速率通过public变量暴露给PlayerController2D
                var reverseDecelField = typeof(PlayerController2D).GetField("reverseDecelMultiplier", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (reverseDecelField != null)
                    reverseDecelField.SetValue(pc2d, reverseDecelMultiplier);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            var pc2d = collision.collider.GetComponent<PlayerController2D>();
            if (pc2d != null)
            {
                pc2d.moveSpeed /= maxSpeedMultiplier;
                pc2d.maxMoveSpeed /= maxSpeedMultiplier;
                pc2d.accelRate /= accelMultiplier;
                var reverseDecelField = typeof(PlayerController2D).GetField("reverseDecelMultiplier", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (reverseDecelField != null)
                    reverseDecelField.SetValue(pc2d, 1f);
            }
        }
    }
} 