using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb;
    public float speedForce;
    public float jumpForce;
    public bool isGround;
    public bool[] directions;
    public int left, right, up;
    public bool swap, fix, swapped, repaired;

    bool flipState;
    //public Transform groundCheck;
    //public float radius;
    //public LayerMask groundLayer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //Randomly creating a faulty controll button
        int index = Random.Range(0, directions.Length);
        for (int i = 0; i < directions.Length; i++)
        {
            if (i != index) directions[i] = true;
        }
        Debug.Log(index);
    }
    void FixedUpdate()
    {

        //Checking trigger for if they have clicked the swap option{Which will swap their current fault conttroller direction with complimentary one}
        TriggerCheck();

        float x = Input.GetAxis("Horizontal") * speedForce * Time.deltaTime;

        if (directions[left] && x < 0)
        {
            transform.Translate(Vector3.right * x);
            //rb.velocity = new Vector2(x, rb.velocity.y);
            FlipCondition(x);
        }
        if (directions[right] && x > 0)
        {
            transform.Translate(Vector3.right * x);
            //rb.velocity = new Vector2(x, rb.velocity.y);
            FlipCondition(x);
        }
        float y = Input.GetAxis("Vertical") * jumpForce * Time.deltaTime;

        //Checking if the player is on the ground so they can jump 

        //isGround = Physics2D.OverlapCircle(groundCheck.position, radius, groundLayer);
        if (isGround && directions[up])
        {
            //isGround = false;
            //transform.Translate(Vector3.up * y);
            rb.AddForce(Vector2.up * y, ForceMode2D.Impulse);

        }
    }

    // Update is called once per frame

    void FlipCondition(float input)  //flips player to create an illusion of player changing direction
    {
        if (input < 0 && !flipState)
        {
            flipState = true;
            Flip();
        }
        else if (input > 0 && flipState)
        {
            flipState = false;
            Flip();
        }

    }

    void Flip()
    {
        Vector3 flip = transform.localScale;
        //Transform flips 180 when their x scale is negative
        flip.x *= -1;
        transform.localScale = flip;
    }

    void OnTriggerStay2D(Collider2D other)
    {

        Debug.Log(other.name + " is Collier Activated");

        if (other.CompareTag("Swap")) swap = true;
        if (other.CompareTag("Fixed")) fix = true;
        if (other.CompareTag("Ground")) isGround = true;
        if (fix || swap)
        {
            Debug.Log("Object Entered");
        }
    }

    /*
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (CompareTag("Ground")) isGround = true;
    }
    */

    void TriggerCheck()
    {
        if (swap && !swapped)
        {
            swapped = true;
            directions[left] = false;
            directions[right] = true;
        }
        if (fix && !repaired)
        {
            repaired = true;
            directions[left] = true;
            directions[right] = true;
            directions[up] = true;
        }
    }


}