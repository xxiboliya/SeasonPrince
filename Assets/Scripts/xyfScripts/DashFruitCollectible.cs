using UnityEngine;

public class DashFruitCollectible : MonoBehaviour
{
    public float respawnTime = 2f;
    private SpriteRenderer sr;
    private Collider2D col;
    private float respawnTimer = 0f;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (!sr.enabled)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0f)
            {
                sr.enabled = true;
                col.enabled = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 重置冲刺
            var pc2d = other.GetComponent<PlayerController2D>();
            if (pc2d != null)
            {
                var canAirDashField = typeof(PlayerController2D).GetField("canAirDash", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (canAirDashField != null)
                    canAirDashField.SetValue(pc2d, true);
            }
            // 消失并计时重生
            sr.enabled = false;
            col.enabled = false;
            respawnTimer = respawnTime;
        }
    }
} 