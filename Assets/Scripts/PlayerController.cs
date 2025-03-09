using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // 移动速度
    public float jumpForce = 13f; // 跳跃力度
    private Rigidbody2D rb;
    private bool isGrounded; // 是否在地面上
    private Transform groundCheck; // 地面检测点
    public LayerMask groundLayer; // 地面层
    public float checkRadius = 1.0f; // 检测半径
    private int jumpCount = 0; // 跳跃计数
    public int maxJumpCount = 2; // 最大跳跃次数

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // 获取Rigidbody2D组件
        rb = GetComponent<Rigidbody2D>();

        // 获取Collider2D组件
        Collider2D collider = GetComponent<Collider2D>();

        // 获取Animator组件
        animator = GetComponent<Animator>();

        // 获取SpriteRenderer组件
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 动态创建地面检测点
        groundCheck = new GameObject("GroundCheck").transform;
        groundCheck.SetParent(transform); // 设置为角色的子对象

        // 设置位置为物体的下边界
        if (collider != null)
        {
            groundCheck.localPosition = new Vector3(0, -collider.bounds.extents.y, 0);
        }
        else
        {
            groundCheck.localPosition = new Vector3(0, -1f, 0); // 默认位置
        }

        groundCheck.gameObject.hideFlags = HideFlags.HideInHierarchy; // 隐藏在Hierarchy中
    }

    private void Update()
    {
        // 获取水平输入（前进/后退）
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // 检测是否在地面上
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // 重置跳跃计数
        if (isGrounded && rb.velocity.y <= 0)
        {
            jumpCount = 0;
        }

        // 按下跳跃键并确保跳跃次数未达到最大值
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
        }

        // 切换动画状态
        bool isMoving = horizontalInput != 0;
        animator.SetBool("run", isMoving); // 触发跑步动画
        animator.SetBool("idle", !isMoving); // 触发待机动画
        animator.SetBool("jump",!isGrounded); // 触发跳跃动画

        // 根据方向翻转角色
        if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
        }

        animator.SetFloat("ySpeed", rb.velocity.y); // 设置Y轴速度参数
    }

    private void OnDrawGizmos()
    {
        // 在Scene视图中绘制地面检测点的范围（调试用）
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}