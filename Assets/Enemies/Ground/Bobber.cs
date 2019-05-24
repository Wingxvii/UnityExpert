using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Bobber enemy class used to define the behaviour of the bobber enemy
    - By John Wang
*/

public class Bobber : Enemy
{

    public float groundedRadius = 0.9f;
    public LayerMask groundMask = 1;

    public float range = 4.0f;
    public float speed = 0.2f;

    private float targetUp;
    private float targetDown;
    private int goingDir = 1;


    // Update is called once per frame
    void Update()
    {

        if (functionQueue.Count > 0)
        {
            //queue listener for spawn calls
            switch (functionQueue.Dequeue())
            {
                case 1: //this handles spawn instances
                    DetermineTargets();                             //redetermines range targets
                    rb.velocity = new Vector2(rb.velocity.x, 0.0f); //relock velocity
                    rb.gravityScale = 0;                            //turns off gravity
                    break;

                default:
                    break;
            }
        }


        //check if direction needs to be reversed
        if (this.transform.position.y < targetDown) {
           goingDir = 1;
       }
       if (this.transform.position.y > targetUp)
       {
           goingDir = -1;
       }

       //collision check
       Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, groundedRadius, groundMask);
       for (int i = 0; i < colliders.Length; i++)
       {
           if (colliders[i].gameObject != gameObject)
           {
               if (colliders[i].isTrigger || colliders[i].tag == "BackWall")   //make sure backwall doesnt reset jump
                   continue;

               if (colliders[i].transform.position.y > this.transform.position.y)
               {
                   goingDir = -1;
               }
               else if (colliders[i].transform.position.y < this.transform.position.y)
               {
                   goingDir = 1;
               }

               break;
            }

        }


    }

    //determines targets for bobbing range
    private void DetermineTargets() {
        //determine targets
        targetUp = this.transform.position.y + (range / 2.0f);
        targetDown = this.transform.position.y - (range / 2.0f);
    }

    private void FixedUpdate()
    {
        //moves itself
        if (isVulnerable)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + (speed * (float)goingDir), this.transform.position.z);
        }

    }

}
