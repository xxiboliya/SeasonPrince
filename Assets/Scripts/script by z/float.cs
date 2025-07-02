using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float amplitude = 0.5f;   // 浮动幅度（上下移动的距离）
    public float frequency = 1f;     // 浮动频率（速度）

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // 记录初始位置
    }

    void Update()
    {
        // 计算新的Y坐标
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        // 应用新的位置
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}