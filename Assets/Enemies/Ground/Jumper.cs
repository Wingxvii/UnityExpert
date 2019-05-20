using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    Jumper enemy class used to define the behaviour of the jumper enemy
    - By John Wang
*/


public class Jumper : Enemy
{
    public bool isGrounded;
    public float groundedRadius = 0.5f;
    public LayerMask groundMask = 1;
    public float jumpPower = 1.0f;
    public float groundColliderOffset = -0.2f;

    // Update is called once per frame
    void Update()
    {
        //ground collision check
        Collider2D[] colliders = Physics2D.OverlapCircleAll( new Vector3(this.transform.position.x, this.transform.position.y + groundColliderOffset), groundedRadius, groundMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                if (colliders[i].isTrigger || colliders[i].tag == "BackWall")   //make sure backwall doesnt reset jump
                    continue;

                isGrounded = true;
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        //trigger jump
        if (isGrounded && isVulnerable) {
            Jump();
            isGrounded = false;
            //Debug.Log("Jumped");
        }
    }


    internal void Jump() {
        //neturalizes y velocity before jump
        rb.velocity = new Vector2(rb.velocity.x, 0.0f);

        //adds a up impulse force to simulate jump
        rb.AddForce(new Vector2(0, jumpPower));
    }
}
