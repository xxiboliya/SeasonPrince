using UnityEngine;

public class VineInteractable : MonoBehaviour
{
    [Header("藤蔓物理参数")]
    [Tooltip("摆荡加速度，影响左右摆动的速度")] public float swingAccel = 2000f;
    [Tooltip("摆荡阻尼（惯性），越接近1越顺滑")] public float swingDamp = 0.98f;
    [Tooltip("回正速度，影响藤蔓自动回到垂直的速度")] public float swingRestore = 200f;
    [Tooltip("最大摆荡角度")] public float maxSwingAngle = 90f;
    [Tooltip("碰到极限角度时的反弹系数")] public float swingBounce = 0.85f;
    [Tooltip("离开藤蔓时的速度放大")] public float swingExitBoost = 1.5f;
    [Tooltip("离开藤蔓时的水平速度放大")] public float swingExitHorizontalBoost = 2.0f;
    [Tooltip("离开藤蔓时的垂直速度放大")] public float swingExitVerticalBoost = 0.3f;
    [Tooltip("离开后再次吸附的延迟")] public float vineReattachDelay = 0.2f;
    [Header("藤蔓长度限制")]
    [Tooltip("玩家在藤蔓上的最短距离（顶部）")]
    public float minSwingLength = 1.0f;
    [Tooltip("玩家在藤蔓上的最大距离（底部）")]
    public float maxSwingLength = 3.0f;
    [Header("藤蔓攀爬参数")]
    [Tooltip("玩家在藤蔓上攀爬的速度")]
    public float climbSpeed = 2.0f;
    [Tooltip("离开藤蔓时的最小向上速度（保证抛物线弹射）")]
    public float minExitUpwardSpeed = 6f;

    private float swingAngle = 0f;
    private float swingSpeed = 0f;
    private float swingLength = 3f;
    private Transform attachedPlayer = null;
    private Rigidbody2D playerRb = null;
    private float lastDetachTime = -10f;
    private Vector2 cachedEntryVelocity = Vector2.zero;

    private void Awake()
    {
        var box = GetComponent<BoxCollider2D>();
        if (box != null)
        {
            float top = box.offset.y + box.size.y / 2f;
            float bottom = box.offset.y - box.size.y / 2f;
            minSwingLength = Mathf.Abs(top);
            maxSwingLength = Mathf.Abs(bottom);
            if (minSwingLength > maxSwingLength)
            {
                float tmp = minSwingLength;
                minSwingLength = maxSwingLength;
                maxSwingLength = tmp;
            }
        }
    }

    void Update()
    {
        if (attachedPlayer != null)
        {
            float input = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(input) > 0.01f)
            {
                swingSpeed += input * swingAccel * Time.deltaTime;
            }
            else
            {
                float restoreForce = -Mathf.Sin(swingAngle * Mathf.Deg2Rad) * swingRestore;
                swingSpeed += restoreForce * Time.deltaTime;
            }
            swingSpeed *= swingDamp;
            swingAngle += swingSpeed * Time.deltaTime;
            if (Mathf.Abs(swingAngle) > maxSwingAngle)
            {
                swingAngle = Mathf.Sign(swingAngle) * maxSwingAngle;
                swingSpeed = -swingSpeed * swingBounce;
            }

            // 支持WS上下移动
            float vInput = Input.GetAxisRaw("Vertical");
            if (Mathf.Abs(vInput) > 0.01f)
            {
                swingLength -= vInput * Time.deltaTime * climbSpeed;
                swingLength = Mathf.Clamp(swingLength, minSwingLength, maxSwingLength);
            }

            transform.localRotation = Quaternion.Euler(0, 0, swingAngle);
            attachedPlayer.localPosition = new Vector3(0, -swingLength, 0);
            if (Input.GetButtonDown("Jump"))
            {
                DetachPlayer();
            }
        }
    }

    public void AttachPlayer(Transform player, Rigidbody2D rb)
    {
        if (attachedPlayer != null) return;
        if (Time.time - lastDetachTime < vineReattachDelay) return;
        attachedPlayer = player;
        playerRb = rb;
        cachedEntryVelocity = rb.velocity;
        player.SetParent(transform);
        swingLength = Mathf.Abs(player.localPosition.y);
        swingAngle = 0f;
        Vector2 tangent = new Vector2(1, 0);
        if (player.localPosition.x < 0) {
            tangent = new Vector2(-1, 0);
        }
        swingSpeed = Vector2.Dot(playerRb.velocity, tangent) / swingLength;
        playerRb.velocity = Vector2.zero;
        player.localPosition = new Vector3(0, -swingLength, 0);
        playerRb.isKinematic = true;

        // 自动重置冲刺次数
        var pc2d = player.GetComponent<PlayerController2D>();
        if (pc2d != null)
        {
            var canAirDashField = typeof(PlayerController2D).GetField("canAirDash", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (canAirDashField != null)
            {
                canAirDashField.SetValue(pc2d, true);
            }
            // 自动重置二段跳
            var canDoubleJumpField = typeof(PlayerController2D).GetField("canDoubleJump", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (canDoubleJumpField != null)
            {
                canDoubleJumpField.SetValue(pc2d, true);
            }
            // 强制设为非落地，保证二段跳判定丝滑
            var isGroundedField = typeof(PlayerController2D).GetField("isGrounded", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (isGroundedField != null)
            {
                isGroundedField.SetValue(pc2d, false);
            }
            // 标记藤蔓状态
            pc2d.isOnVine = true;
        }
    }

    public void DetachPlayer()
    {
        if (attachedPlayer == null) return;
        lastDetachTime = Time.time;
        attachedPlayer.SetParent(null);
        attachedPlayer.localRotation = Quaternion.identity;
        float rad = swingAngle * Mathf.Deg2Rad;
        Vector2 tangent = new Vector2(Mathf.Cos(rad - Mathf.PI / 2), Mathf.Sin(rad - Mathf.PI / 2));
        Vector2 endVelocity = swingSpeed * swingLength * tangent;
        Vector2 vineVelocity = endVelocity * swingExitBoost + Vector2.up * swingExitVerticalBoost;
        if (vineVelocity.y < minExitUpwardSpeed)
            vineVelocity.y = minExitUpwardSpeed;
        playerRb.isKinematic = false;
        playerRb.velocity = vineVelocity;
        // 解除藤蔓状态
        var pc2d = attachedPlayer.GetComponent<PlayerController2D>();
        if (pc2d != null) {
            pc2d.isOnVine = false;
            // 藤蔓脱离后给予1次二段跳机会
            if (pc2d.enableDoubleJump)
            {
                var canDoubleJumpField = typeof(PlayerController2D).GetField("canDoubleJump", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (canDoubleJumpField != null)
                    canDoubleJumpField.SetValue(pc2d, true);
            }
            // 藤蔓脱离后自动进入1s滑翔
            var glideGraceTimerField = typeof(PlayerController2D).GetField("glideGraceTimer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var glideGraceDurationField = typeof(PlayerController2D).GetField("glideGraceDuration", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (glideGraceTimerField != null && glideGraceDurationField != null)
            {
                float duration = (float)glideGraceDurationField.GetValue(pc2d);
                glideGraceTimerField.SetValue(pc2d, duration);
            }
        }
        attachedPlayer = null;
        playerRb = null;
        transform.localRotation = Quaternion.identity;
    }

    // 自动吸附玩家
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                AttachPlayer(other.transform, rb);
            }
        }
    }
} 