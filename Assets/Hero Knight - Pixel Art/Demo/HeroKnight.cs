using UnityEngine;

public class HeroKnight : MonoBehaviour
{
    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] float m_rollForce = 6.0f;
    [SerializeField] bool m_noBlood = false;
    [SerializeField] GameObject m_slideDust;
    [SerializeField] private AudioClip heroJumpClip;
    [SerializeField] private AudioClip heroRunClip;
    [SerializeField] private AudioClip heroLandClip;
    [SerializeField] private AudioClip heroWallSlideClip;
    [SerializeField] private AudioClip heroDeathClip;
    [SerializeField] private AudioClip heroHurtClip;
    [SerializeField] private AudioClip heroSwordSwingClip;
    [SerializeField] private AudioClip heroRollClip;
    [SerializeField] private AudioClip heroShieldBashClip;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_HeroKnight m_groundSensor;
    private Sensor_HeroKnight m_wallSensorR1;
    private Sensor_HeroKnight m_wallSensorR2;
    private Sensor_HeroKnight m_wallSensorL1;
    private Sensor_HeroKnight m_wallSensorL2;
    private bool m_isWallSliding = false;
    private bool isWallSlidingSoundPlaying = false;
    private bool m_grounded = false;
    private bool m_rolling = false;
    private int m_facingDirection = 1;
    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    private float m_delayToIdle = 0.0f;
    private float m_rollDuration = 8.0f / 14.0f;
    private float m_rollCurrentTime;

    private bool isMoving = false;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
    }

    void Update()
    {
        m_timeSinceAttack += Time.deltaTime;

        if (m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        if (m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
            SFXManager.instance.PlaySFXClip(heroLandClip, transform, 1f);    // Play land sound

        }

        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        float inputX = Input.GetAxis("Horizontal");

        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        if (!m_rolling)
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // Check wall slide
        bool wasWallSliding = m_isWallSliding;
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);

        if (m_isWallSliding && !isWallSlidingSoundPlaying)
        {
            SFXManager.instance.PlaySFXClip(heroWallSlideClip, transform, 1f); // Play wall slide sound
            isWallSlidingSoundPlaying = true;
        }
        else if (!m_isWallSliding && isWallSlidingSoundPlaying)
        {
            SFXManager.instance.PlaySFXClip(heroWallSlideClip, transform, 1f); // Stop wall slide sound
            isWallSlidingSoundPlaying = false;
        }

        if (Input.GetKeyDown("e") && !m_rolling)
        {
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
            SFXManager.instance.PlaySFXClip(heroDeathClip, transform, 1f); //Play death sound

        }
        else if (Input.GetKeyDown("q") && !m_rolling)
        {
            m_animator.SetTrigger("Hurt");
            SFXManager.instance.PlaySFXClip(heroHurtClip, transform, 1f); // Play Hurt sound
        }

        else if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
            {
                m_currentAttack++;

                if (m_currentAttack > 3)
                    m_currentAttack = 1;

                if (m_timeSinceAttack > 1.0f)
                    m_currentAttack = 1;

                m_animator.SetTrigger("Attack" + m_currentAttack);
                m_timeSinceAttack = 0.0f;
                SFXManager.instance.PlaySFXClip(heroSwordSwingClip, transform, 1f); // Play SwingSword01 sound

            Debug.Log("Playing sound: SwingSword01"); // Debug info
        }
        else if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
            SFXManager.instance.PlaySFXClip(heroShieldBashClip, transform, 1f); // Play ShieldDraw sound
        }
        else if (Input.GetMouseButtonUp(1))
            m_animator.SetBool("IdleBlock", false);
        else if (Input.GetKeyDown(KeyCode.S) && !m_rolling && !m_isWallSliding)
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
            SFXManager.instance.PlaySFXClip(heroRollClip, transform, 1f); // Play Roll sound

        }
        else if ((Input.GetKeyDown("space") || Input.GetKeyDown(KeyCode.W)) && m_grounded && !m_rolling)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);

            SFXManager.instance.PlaySFXClip(heroJumpClip, transform, 1f);    // Play jump sound

        }
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
            if (!isMoving)
            {
                isMoving = true;
                SFXManager.instance.PlaySFXClip(heroRunClip, transform, 1f); // Play run sound

            }
        }
        else
        {
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
            {
                m_animator.SetInteger("AnimState", 0);
                if (isMoving)
                {
                    isMoving = false;
                    SFXManager.instance.PlaySFXClip(heroRunClip, transform, 1f);   // Stop run sound

                }
            }
        }
    }

    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }
}
