using UnityEngine;
using System.Collections;

public class PlatformMove : MonoBehaviour
{

    public float moveAccel = 0.35f;
    public float topSpeed = 2f;
    public float distRange = 5f;
    public float brakingMultiplier = 2f;

    private bool justFlipped = false;
    private Rigidbody2D rb2d;
    private float movingRight = 1f;
    private float startingX;
    /*
	 * 1. Check velocity of rb2d against topSpeed.
	 * 2. If topSpeed >= velocity, addforce Vector2.right * movingRight * moveAccel.  If result > topSpeed, set to topSpeed.
	 *    		      <= velocity, do not add additional force (UNLESS sign of movingRight is opposite velocity). (can be done by skipping code and doing if for the inverse case)
	 * 				(Multiply by movingRight for direction.)
	 * 				(See SimplePlatformController script ln 44-50.)
	 * 3. If Mathf.Abs(position.x - startingX) >= distRange, set movingRight to -1f.
	 * 
	 * 
	*/

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        startingX = rb2d.position.x;
        Debug.Log(justFlipped);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //If recently reversed direction and not in the range again yet, don't check for direction switching.

        // Is the platform outside of the range?  Flips repeatedly if not back in range within one update.  Need a way

        //rb2d.AddForce (Vector2.right * movingRight * moveAccel);
        /*	if (rb2d.velocity > topSpeed)
            rb2d.velocity = topSpeed;		// Unnecessary.  We are reversing direction, and the rest of the code should keep topSpeed in check, so it will (almost) never be faster.
                                            // Even if it is, it will be slowing down anyway.
    */

        // decideDirection();
        StartCoroutine("checkFlip");
        // Debug.Log("Reached.  Flip? : " + justFlipped);
        if (Mathf.Abs(rb2d.velocity.x) < topSpeed)  //if under topSpeed, add force.
            if (Mathf.Sign(movingRight) == Mathf.Sign(rb2d.velocity.x))  //if adding force to same direction currently moving, add normally.  Else add with a braking multiplier to slow quicker.
                rb2d.AddForce(Vector2.right * movingRight * moveAccel);
            else
                rb2d.AddForce(Vector2.right * movingRight * moveAccel * brakingMultiplier);
        else if (Mathf.Sign(rb2d.velocity.x) == Mathf.Sign(movingRight))//Need a case for if hits topSpeed and also is told to flip so it doesn't go in reverse at topSpeed immediately
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * topSpeed, 0f);
        else
            rb2d.AddForce(Vector2.right * movingRight * moveAccel * brakingMultiplier);
        //Debug.Log("Pos: " + rb2d.position.x + "  Flip: " + justFlipped);
        //Debug.Log ("Pos: " + rb2d.position.x + "  Start: " + startingX + "  Range: " + distRange + "  MveRight: " + movingRight); //Doesn't reverse.  Look into that.
        //	Debug.Log (rb2d.velocity.x);
        Debug.Log("Is " + Mathf.Abs(rb2d.velocity.x) + " < " + topSpeed + "?  " + (Mathf.Abs(rb2d.velocity.x) < topSpeed));
    }
    //if justFlipped is positive, don't run the checkFlip coroutine (do all this in another method that is called by FixedUpdate).  Instead, check if Mathf.Abs(rb2d.position.x - startingX) < distRange.  If so, justFlipped = false. 



    IEnumerator checkFlip()
    {
        if (!justFlipped)
        {
            if (Mathf.Abs(rb2d.position.x - startingX) > distRange)
            {
                movingRight *= -1f;
                justFlipped = true;
                yield return null;
            }
        }
        else
            if (Mathf.Abs(rb2d.position.x - startingX) < distRange)
            justFlipped = false;

        yield return null;
    }
}

