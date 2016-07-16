using UnityEngine;
using System.Collections;

public class SimplePlatformController : MonoBehaviour {

    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = false;

    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public Transform groundCheck;

    private bool grounded = false;
    private Animator anim;
    private Rigidbody2D rb2d;

    // Use this for initialization
    void Awake() {

        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update() {

        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")); //Check against ground only.  groundCheck is a child of player, so it will represent the player's feet.  Linecast stops at that point and looks for ground.

        if (Input.GetButtonDown("Jump") && grounded) //If jumping and grounded.  No double jumping (yet!).
        {
            jump = true;
        }

    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");

        anim.SetFloat("Speed", Mathf.Abs(h)); //Speed is always positive, so use Mathf.Abs of the input.

        if (h * rb2d.velocity.x < maxSpeed) //if input * velocity < maxSpeed, apply additional force as directed by input.
        {
            rb2d.AddForce(Vector2.right * h * moveForce); //h = -1 to 1, if -1 Vector2.right becomes left * moveForce.
        }

        if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);

        if (h > 0 && !facingRight)
            Flip();
        else if (h < 0 && facingRight)
            Flip();

        if (jump)
        {
            anim.SetTrigger("Jump");
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
