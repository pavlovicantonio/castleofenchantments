using UnityEngine;

public class Enemy4 : MonoBehaviour
{
    public float jumpForce = 5f;
    public float jumpInterval = 2f;
    private Rigidbody2D rb;
    private float jumpTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpTimer = jumpInterval;
    }

    void Update()
    {

        jumpTimer -= Time.deltaTime;
        if (jumpTimer <= 0f)
        {
            JumpInPlace();
            jumpTimer = jumpInterval;
        }
    }

    void JumpInPlace()
    {

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
