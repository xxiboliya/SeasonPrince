using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float crouchSpeed = 2.5f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.08f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isCrouching;
    private float moveInput;

    // --- Season Abilities ---
    [Header("Season Abilities")]
    public bool enableDoubleJump = true; // Spring
    public bool enableDash = true;       // Summer
    public bool enableGlide = true;      // Autumn
    public bool enableSlide = true;      // Winter

    // Double Jump
    private bool canDoubleJump;

    // Dash
    public float dashSpeed = 12f;
    public float dashDuration = 0.2f;
    private bool isDashing = false;
    private float dashTimeLeft;
    private bool canAirDash = true;
    private float dashCooldownTimer = 0f; // 冲刺CD计时器
    public float dashCooldown = 0.2f; // 冲刺CD

    // Glide
    public float glideGravityScale = 0.3f;
    private bool isGliding = false;
    private float originalGravityScale;
    private float glideTargetVx = 0f; // 漂浮时的目标速度

    // Slide
    public float slideSpeed = 8f;
    public float slideDuration = 0.4f;
    private bool isSliding = false;
    private float slideTimeLeft;

    // New variables for short press movement
    [Header("Short Press Movement")]
    public float shortPressMoveDuration = 0.15f;
    public float shortPressMoveSpeed = 7f;
    private float shortPressMoveTimer = 0f;
    private float moveDirection = 0f;

    // Slide/加速参数
    private float targetMoveDir = 0f;
    private bool isAccelerating = false;
    private float baseMoveSpeed = 5f;
    public float maxMoveSpeed = 15f;
    public float accelRate = 10f;
    public float decelRate = 50f;

    // Wall Slide
    [Header("Wall Slide")]
    public float wallSlideSpeed = 1.5f;
    private bool isTouchingWall = false;
    private bool isWallSliding = false;
    public Transform leftWallCheck;
    public Transform rightWallCheck;
    public float wallCheckDistance = 0.2f;

    // Wall Jump
    [Header("Wall Jump")]
    public float wallJumpForce = 12f;
    public float wallJumpXForce = 8f;
    private bool isWallJumping = false;
    private float wallJumpTime = 0.4f;
    private float wallJumpTimer = 0f;

    private bool justWallJumped = false;

    public bool isOnVine = false;

    private bool canMove = true;

    [Header("Effects")]
    public GameObject doubleJumpEffectPrefab; // 二段跳特效Prefab
    public GameObject dashEffectPrefab; // 冲刺特效Prefab
    public GameObject glideEffectPrefab; // 漂浮特效Prefab
    private GameObject currentGlideEffect; // 当前漂浮特效实例

    private bool wasDashing = false; // 记录上帧冲刺状态
    private bool prevIsGliding = false; // 记录上帧漂浮状态

    private Animator animator;

    void Awake()
    {
        if (groundCheck == null)
        {
            groundCheck = transform.Find("groundCheck");
            if (groundCheck == null)
                Debug.LogError("groundCheck not found as child of Player!");
        }
        if (leftWallCheck == null)
        {
            leftWallCheck = transform.Find("leftWallCheck");
            if (leftWallCheck == null)
            {
                GameObject lwc = new GameObject("leftWallCheck");
                lwc.transform.parent = transform;
                lwc.transform.localPosition = new Vector3(-0.3f, 0, 0);
                leftWallCheck = lwc.transform;
            }
        }
        if (rightWallCheck == null)
        {
            rightWallCheck = transform.Find("rightWallCheck");
            if (rightWallCheck == null)
            {
                GameObject rwc = new GameObject("rightWallCheck");
                rwc.transform.parent = transform;
                rwc.transform.localPosition = new Vector3(0.3f, 0, 0);
                rightWallCheck = rwc.transform;
            }
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 4f;
        originalGravityScale = rb.gravityScale;
        baseMoveSpeed = moveSpeed;
        maxMoveSpeed = 15f;
        accelRate = 10f;
        animator = GetComponent<Animator>();
        // 进入游戏时关闭MagicArea02_gold_Instance的粒子播放
        Transform magicArea = transform.Find("MagicArea02_gold_Instance");
        if (magicArea != null)
        {
            var ps = magicArea.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
    }

    void OnEnable()
    {
        DialogueUI.OnDialogueActiveChanged += SetCanMove;
    }
    void OnDisable()
    {
        DialogueUI.OnDialogueActiveChanged -= SetCanMove;
    }
    void SetCanMove(bool isActive)
    {
        if (isActive)
        {
            // 先完全静止
            if (rb != null)
                rb.velocity = Vector2.zero;
            canMove = false;
        }
        else
        {
            canMove = true;
        }
    }

    void Update()
    {
        if (!canMove)
        {
            rb.velocity = Vector2.zero;
            return; // 禁止一切输入
        }
        float rawInput = Input.GetAxisRaw("Horizontal");
        moveInput = rawInput;
        isCrouching = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        // 在Update里主动检测地面，保证加速逻辑稳定
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Wall Jump 输入检测
        if (isWallSliding && Input.GetButtonDown("Jump"))
        {
            isWallJumping = true;
            wallJumpTimer = wallJumpTime;
            justWallJumped = true;
            int wallDir = 0;
            int wallLayerMask = LayerMask.GetMask("Wall");
            if (Physics2D.OverlapCircle(leftWallCheck.position, wallCheckDistance, wallLayerMask))
                wallDir = 1;
            else if (Physics2D.OverlapCircle(rightWallCheck.position, wallCheckDistance, wallLayerMask))
                wallDir = -1;
            rb.velocity = new Vector2(wallDir * wallJumpXForce, wallJumpForce);
        }

        // --- Double Jump (Spring) & Glide (Autumn) 优化 ---
        if (enableDoubleJump || enableGlide)
        {
            if (isGrounded)
                canDoubleJump = true;
            if (Input.GetButtonDown("Jump"))
            {
                if (isGrounded && !isCrouching)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                }
                else if (canDoubleJump && !isCrouching)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    canDoubleJump = false;
                    // 只在空中二段跳时释放特效
                    if (!isGrounded && doubleJumpEffectPrefab != null)
                    {
                        Instantiate(doubleJumpEffectPrefab, transform.position, Quaternion.identity);
                    }
                }
            }
            // 只有在下落、未二段跳后，且长按空格时才滑翔
            if (enableGlide && !isGrounded && rb.velocity.y < 0 && !canDoubleJump && Input.GetKey(KeyCode.Space))
            {
                isGliding = true;
            }
            else
            {
                isGliding = false;
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }

        // --- Dash (Summer) ---
        if (enableDash)
        {
            // 落地重置空中冲刺
            if (isGrounded)
                canAirDash = true;
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && !isSliding)
            {
                if (isGrounded || (!isGrounded && canAirDash))
                {
                    // 判断冲刺方向
                    if (moveInput != 0 && Mathf.Sign(moveInput) != Mathf.Sign(targetMoveDir) && targetMoveDir != 0)
                    {
                        // 反向冲刺，速度重置为该方向基础速度
                        moveSpeed = baseMoveSpeed;
                        targetMoveDir = moveInput;
                    }
                    // 正向冲刺不改变速度
                    isDashing = true;
                    dashTimeLeft = dashDuration;
                    if (!isGrounded)
                        canAirDash = false;
                }
            }
        }

        // --- Slide/加速 (Winter) ---
        if (enableSlide)
        {
            if (isGrounded && Input.GetKey(KeyCode.LeftControl))
            {
                isAccelerating = true;
                if (moveInput != 0)
                    targetMoveDir = moveInput;
                if (Mathf.Sign(moveInput) == Mathf.Sign(targetMoveDir) && moveInput != 0)
                {
                    baseMoveSpeed = Mathf.Min(baseMoveSpeed + accelRate * Time.deltaTime, maxMoveSpeed);
                }
                else if (moveInput != 0 && Mathf.Sign(moveInput) != Mathf.Sign(targetMoveDir))
                {
                    baseMoveSpeed = Mathf.Max(baseMoveSpeed - decelRate * Time.deltaTime, 0f);
                    if (baseMoveSpeed == 0f)
                        targetMoveDir = moveInput;
                }
                else if (moveInput == 0)
                {
                    if (baseMoveSpeed > moveSpeed)
                    {
                        baseMoveSpeed = Mathf.Max(baseMoveSpeed - decelRate * Time.deltaTime, moveSpeed);
                        if (baseMoveSpeed == moveSpeed)
                        {
                            targetMoveDir = 0f;
                            isAccelerating = false;
                        }
                    }
                    else
                    {
                        baseMoveSpeed = moveSpeed;
                        targetMoveDir = 0f;
                        isAccelerating = false;
                    }
                }
            }
            else if (!isGrounded) // 空中加速/减速逻辑
            {
                if (moveInput != 0)
                {
                    if (Mathf.Sign(moveInput) == Mathf.Sign(targetMoveDir))
                    {
                        // 空中同向，速度不变
                    }
                    else if (Mathf.Sign(moveInput) != Mathf.Sign(targetMoveDir))
                    {
                        baseMoveSpeed = Mathf.Max(baseMoveSpeed - decelRate * Time.deltaTime, 0f);
                        if (baseMoveSpeed == 0f)
                            targetMoveDir = moveInput;
                    }
                }
                // 空中松开A/D，速度和方向保持不变
            }
            else if (isGrounded && !Input.GetKey(KeyCode.LeftControl))
            {
                if (moveInput != 0)
                {
                    baseMoveSpeed = Mathf.Max(baseMoveSpeed - decelRate * Time.deltaTime, moveSpeed);
                    if (baseMoveSpeed == moveSpeed)
                    {
                        targetMoveDir = moveInput;
                        isAccelerating = false;
                    }
                }
                else
                {
                    if (baseMoveSpeed > moveSpeed)
                    {
                        baseMoveSpeed = Mathf.Max(baseMoveSpeed - decelRate * Time.deltaTime, moveSpeed);
                        if (baseMoveSpeed == moveSpeed)
                        {
                            targetMoveDir = 0f;
                            isAccelerating = false;
                        }
                    }
                    else
                    {
                        baseMoveSpeed = moveSpeed;
                        targetMoveDir = 0f;
                        isAccelerating = false;
                    }
                }
            }
        }
        else
        {
            targetMoveDir = moveInput;
            baseMoveSpeed = moveSpeed;
        }

        // New logic for short press movement
        if (isGrounded && !isDashing && !isSliding)
        {
             if (moveInput != 0 && shortPressMoveTimer <= 0)
             {
                moveDirection = moveInput;
                shortPressMoveTimer = shortPressMoveDuration;
             }
        }

        // 检测冲刺开始，绑定特效播放到isDashing状态变化
        if (isDashing && !wasDashing)
        {
            PlayDashEffect();
        }
        wasDashing = isDashing;

        // 冲刺重置逻辑：只有竖直速度为0时重置canAirDash
        if (Mathf.Abs(rb.velocity.y) < 0.01f)
            canAirDash = true;

        // 直接控制MagicArea02_gold_Instance特效播放（仅在状态变化时触发）
        Transform magicArea = transform.Find("MagicArea02_gold_Instance");
        if (magicArea != null)
        {
            var ps = magicArea.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                if (isGliding && !prevIsGliding)
                {
                    ps.Clear();
                    ps.Play(); // 只在刚进入漂浮时Restart
                }
                else if (!isGliding && prevIsGliding)
                {
                    ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
            }
        }
        prevIsGliding = isGliding;
    }

    void FixedUpdate()
    {
        if (justWallJumped)
        {
            justWallJumped = false;
            return;
        }
        // Use LayerMask for ground check for efficiency and correctness.
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // wall slide 只检测Wall Layer
        isWallSliding = false;
        if (!isGrounded && Mathf.Abs(moveInput) > 0.01f)
        {
            int wallLayerMask = LayerMask.GetMask("Wall");
            bool leftWall = Physics2D.OverlapCircle(leftWallCheck.position, wallCheckDistance, wallLayerMask);
            bool rightWall = Physics2D.OverlapCircle(rightWallCheck.position, wallCheckDistance, wallLayerMask);
            isWallSliding = (leftWall && moveInput < 0) || (rightWall && moveInput > 0);
        }

        // --- Dash 触发 ---
        dashCooldownTimer -= Time.deltaTime;
        if (enableDash && Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0f && (isGrounded || (canAirDash && !isDashing)))
        {
            isDashing = true;
            dashTimeLeft = dashDuration;
            if (!isGrounded) canAirDash = false;
            dashCooldownTimer = dashCooldown;
        }
        // --- Dash 物理 ---
        if (isDashing)
        {
            rb.velocity = new Vector2(moveInput * dashSpeed, 0);
            dashTimeLeft -= Time.fixedDeltaTime;
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
            }
            return;
        }

        // --- Slide ---
        if (isSliding)
        {
            rb.velocity = new Vector2(moveInput * slideSpeed, rb.velocity.y);
            slideTimeLeft -= Time.fixedDeltaTime;
            if (slideTimeLeft <= 0)
            {
                isSliding = false;
            }
            return;
        }

        // --- Glide ---
        if (isGliding)
        {
            rb.gravityScale = glideGravityScale;
        }
        else
        {
            rb.gravityScale = originalGravityScale;
        }

        // --- Wall Slide ---
        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }
        else if (shortPressMoveTimer > 0)
        {
             // Short press movement logic
             float speed = isCrouching ? crouchSpeed : shortPressMoveSpeed;
             rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
             shortPressMoveTimer -= Time.fixedDeltaTime;
             if (shortPressMoveTimer <= 0)
             {
                rb.velocity = new Vector2(0, rb.velocity.y); // Stop after move
             }
        }
        else if (!isGrounded)
        {
            float airSpeed = baseMoveSpeed;
            float vx = rb.velocity.x;
            if (isGliding)
            {
                float input = moveInput;
                float speed = moveSpeed;
                float turnSmooth = 8f; // 缓冲系数

                // 判断是否同向
                if (Mathf.Abs(input) > 0.01f)
                {
                    if (Mathf.Sign(input) == Mathf.Sign(vx) || Mathf.Abs(vx) < 0.05f)
                    {
                        // 同向或已接近0，目标速度为input*speed
                        glideTargetVx = input * speed;
                    }
                    else
                    {
                        // 反向，目标速度先为0
                        glideTargetVx = 0f;
                    }
                }
                else
                {
                    // 无输入，缓冲到0
                    glideTargetVx = 0f;
                }

                float newVx = Mathf.Lerp(vx, glideTargetVx, turnSmooth * Time.fixedDeltaTime);
                rb.velocity = new Vector2(newVx, rb.velocity.y);
            }
            else
            {
                // 空中移动：只能通过反方向键减速，不能直接反向
                if (Mathf.Abs(moveInput) > 0.01f && !isWallJumping)
                {
                    if (Mathf.Sign(moveInput) == Mathf.Sign(vx) || Mathf.Abs(vx) < 0.01f)
                    {
                        // 同向或已停止，维持当前速度
                        rb.velocity = new Vector2(moveInput * airSpeed, rb.velocity.y);
                    }
                    else
                    {
                        // 反向，减速到0，不能直接反向
                        float decel = accelRate * 1.5f * Time.fixedDeltaTime;
                        float newVx = Mathf.MoveTowards(vx, 0, decel);
                        rb.velocity = new Vector2(newVx, rb.velocity.y);
                    }
                }
                // 没有输入时，保持原速度
            }
        }
        else
        {
            // --- Normal Move ---
            float vx = moveInput * moveSpeed;
            if (float.IsNaN(vx) || float.IsInfinity(vx)) vx = 0f;
            rb.velocity = new Vector2(vx, rb.velocity.y);
        }

        // Wall Jump后短暂禁止水平输入
        if (isWallJumping)
        {
            wallJumpTimer -= Time.fixedDeltaTime;
            if (wallJumpTimer <= 0f)
            {
                isWallJumping = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("trap"))
        {
            Debug.Log("Player Died: Collided with a trap.");
            if (animator != null)
            {
                animator.SetTrigger("isHurt");
            }
            GameManager.Instance.PlayerDied();
            EatManager.ResetEats();
            Eat.ResetCount();
        }
    }

    // 冲刺特效播放函数
    private void PlayDashEffect()
    {
        if (dashEffectPrefab != null && moveInput != 0)
        {
            float offsetX = moveInput > 0 ? -0.7f : 0.7f;
            Vector3 effectPos = transform.position + new Vector3(offsetX, 0.1f, 0);
            Quaternion effectRot = moveInput > 0 ? Quaternion.Euler(0, -90, 0) : Quaternion.Euler(0, 90, 0);
            var effect = Instantiate(dashEffectPrefab, effectPos, effectRot, transform);
            effect.SetActive(true);
            var ps = effect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
                float delay = ps.main.duration + ps.main.startLifetime.constantMax;
                StartCoroutine(ClearParticlesAfter(effect, ps, delay));
            }
        }
    }

    private System.Collections.IEnumerator ClearParticlesAfter(GameObject effectObj, ParticleSystem ps, float delay)
    {
        yield return new WaitForSeconds(delay + 0.1f); // 残留0.1秒
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        Destroy(effectObj);
    }
}