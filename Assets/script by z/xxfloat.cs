using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class xfloat : MonoBehaviour
{
    [Header("�ƶ�����")]
    public float moveSpeed = 5f;        // �ƶ��ٶ�
    public float moveRange = 5f;        // �ƶ���Χ

    private Vector3 startPosition;      // ��ʼλ��
    private float direction = 1f;       // �ƶ�����
    private Rigidbody2D rb;

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // ��ֹ��������
        rb.freezeRotation = true; // ��ֹ��ת
    }

    void FixedUpdate()
    {
        float distanceFromStart = transform.position.x - startPosition.x;
        if (Mathf.Abs(distanceFromStart) >= moveRange)
        {
            direction *= -1;
        }
        // ֻ��x���������ٶ�
        rb.velocity = new Vector2(moveSpeed * direction, 0);
    }
}
