using UnityEngine;
using System.Collections;

public class PlatformFall : MonoBehaviour {

    public float fallDelay = 3f;
    public Transform groundCheck;


    private Rigidbody2D rb2d;

    /*
     * 1. Detect when the player is touching it.
     * 2. Wait /fallDelay/ seconds.
     * 3. Enable gravity.
     * 
     */
	// Use this for initialization
	void Awake () {
        //Or, getComponent<CircleCollider2D>() here from public Transform (or is it gameObject?) hero and check with .isTouching
        rb2d = GetComponent<Rigidbody2D>();
    }

    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Invoke("GravityDelay", fallDelay);

            // OR: Invoke (GravityDelay, fallDelay);
            // No need for a coroutine, yield/return line

            //My version:
            //StartCoroutine(GravityDelay(fallDelay));
        }
    }

    void GravityDelay()
    {
        rb2d.isKinematic = false;
    }

    /*  My version:
        IEnumerator GravityDelay (float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        rb2d.isKinematic = false;
    }
    */
    void Update () {
	   
	}
}
