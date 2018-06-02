using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]

	public bool jump = false;				// Condition for whether the player should jump.
    private float canJumpAfter; // Timestamp when jump delay is over
    public float jumpCoolDownInSeconds = 0.1f; // Time how long one has to wait before jumping again


	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.


    public Transform groundCheck;			// A position marking the center of circle where to check if the player is grounded.
    public bool grounded = false;           // Whether or not the player is grounded.
    public float groundCheckRadius = 0.4f;   // Radius of ground check circle
    public LayerMask groundLayer;

    public Transform waterCheck;           // A position marking the center of circle where to check if the player is in water.
    public bool watered = false;           // Whether or not the player is in water (deep enough to swim).
    public float waterCheckRadius = 0.1f;   // Radius of water check circle
    public LayerMask waterLayer;

	private Animator anim;					// Reference to the player's animator component.
    private Rigidbody2D rigi;


	void Awake()
	{
		// Setting up references.
        anim = GetComponent<Animator>();
        rigi = GetComponent<Rigidbody2D>();
        canJumpAfter = Time.time;
	}


	void Update()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        watered = Physics2D.OverlapCircle(waterCheck.position, waterCheckRadius, waterLayer);

        float now = Time.time;
        float vert = Input.GetAxis("Vertical");
        // If the jump button is pressed and the player is grounded then the player should jump.
        jump = false;
        if (vert > 0 && grounded && now > canJumpAfter && !watered)
        {
            jump = true;
            canJumpAfter = now + jumpCoolDownInSeconds;
        }
	}


	void FixedUpdate ()
	{
        // Cache the horizontal input.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

		// The Speed animator parameter is set to the absolute value of the horizontal input.
		anim.SetFloat("Speed", Mathf.Abs(h));

        //print(grounded);
		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
        if (h * rigi.velocity.x < maxSpeed)
			// ... add a force to the player.
			rigi.AddForce(Vector2.right * h * moveForce);

		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(rigi.velocity.x) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
            rigi.velocity = new Vector2(Mathf.Sign(rigi.velocity.x) * maxSpeed, rigi.velocity.y);

		// If the input is moving the player right and the player is facing left...
		if(h > 0 && !facingRight)
			// ... flip the player.
			Flip();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(h < 0 && facingRight)
			// ... flip the player.
			Flip();

		// If the player should jump...
		if(jump)
		{
			// Set the Jump animator trigger parameter.
			anim.SetTrigger("Jump");

			// Play a random jump audio clip.
			//int i = Random.Range(0, jumpClips.Length);
			//AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);

			// Add a vertical force to the player.
            rigi.AddForce(new Vector2(0f, jumpForce));
            //rigi.velocity = new Vector2(rigi.velocity.x, jumpForce*0.1f);

			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			jump = false;
		}
        else if (watered){
            rigi.AddForce(Vector2.up * v * moveForce);
            if(Mathf.Abs(rigi.velocity.y) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
            rigi.velocity = new Vector2(rigi.velocity.x, Mathf.Sign(rigi.velocity.y) * maxSpeed);
        }
	}
	
	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

}
