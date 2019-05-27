using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    Flapper enemy class used to define the behaviour of the flapper enemy
    - By John Wang
*/
public class Flapper : Enemy
{

    public float groundedRadius = 0.9f;
    public LayerMask groundMask = 1;
    public float flapStrength;
    public float flapAltitude = 0.5f;  //GDD asks us to make sure it doesnt go below the lowest platform


    // Update is called once per frame
    void Update()
    {

        //collision check
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, groundedRadius, groundMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                if (colliders[i].isTrigger || colliders[i].tag == "BackWall")   //make sure backwall doesnt reset jump
                    continue;

                if (colliders[i].transform.position.y < this.transform.position.y && isVulnerable)
                {
                    Flap();
                }

                break;
            }

        }



        //checks if a flap is required
        if (this.transform.position.y < flapAltitude && isVulnerable) { Flap(); }


    }


    void Flap()
    {
        //neturalizes y velocity before flap
        rb.velocity = new Vector2(rb.velocity.x, 0.0f);
        //simulates a flap
        rb.AddForce(new Vector2(0, flapStrength));      
    }
}
