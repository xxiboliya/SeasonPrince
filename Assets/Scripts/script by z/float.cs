using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float amplitude = 0.5f;   // �������ȣ������ƶ��ľ��룩
    public float frequency = 1f;     // ����Ƶ�ʣ��ٶȣ�

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // ��¼��ʼλ��
    }

    void Update()
    {
        // �����µ�Y����
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        // Ӧ���µ�λ��
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}