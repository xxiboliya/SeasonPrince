using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class xfloat : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 5f;        // 移动速度
    public float moveRange = 5f;        // 移动范围

    private Vector3 startPosition;      // 起始位置
    private float direction = 1f;       // 移动方向
    private Rigidbody2D rb;

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // 防止物体下落
        rb.freezeRotation = true; // 防止旋转
    }

    void FixedUpdate()
    {
        float distanceFromStart = transform.position.x - startPosition.x;
        if (Mathf.Abs(distanceFromStart) >= moveRange)
        {
            direction *= -1;
        }
        // 只在x轴上设置速度
        rb.velocity = new Vector2(moveSpeed * direction, 0);
    }
}
