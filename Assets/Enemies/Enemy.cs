﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    Enemy class based on instructor written version
    
    - Original by Dustin Carroll
    - Version 2 by John Wang
*/
public class Enemy : MonoBehaviour
{
    public int maxHealth = 1;

    internal bool isVulnerable;
    protected int health;
    internal Rigidbody2D rb;
    internal CircleCollider2D circleColl;
    internal SpriteRenderer sr;
    internal Color originalColor;
    internal Collider2D backWallCollider;

    //this queue is used to signal function calls to the child
    internal Queue<int> functionQueue;


    void Awake()
    {

        functionQueue = new Queue<int>();

        rb = this.GetComponent<Rigidbody2D>();
        circleColl = this.GetComponent<CircleCollider2D>();
        sr = this.GetComponent<SpriteRenderer>();
        originalColor = sr.color;

        Setup();    
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == GameplayConstants.TAG_KillZone)
        {
            GoToSleep();
        }
        else if (col.tag == GameplayConstants.TAG_WakeField)
        {
            WakeUp();
        }
    }

    internal virtual void Setup()
    {
        // Any additional commands at Awaken.
    }

    internal virtual void WakeUp()
    {
        isVulnerable = true;


    }

    internal virtual void GoToSleep()
    {
        this.gameObject.SetActive(false);
    }

    public virtual void Spawn(Vector3 position)
    {
        //sends a signal to indicate spawn was called
        functionQueue.Enqueue(1);

        isVulnerable = false;
        health = maxHealth;
        circleColl.isTrigger = false;
        this.transform.position = position;
        SetScaleByHealth(maxHealth);
        sr.color = originalColor;
        rb.velocity = Vector2.zero;

        backWallCollider = GameObject.FindGameObjectWithTag("BackWall").GetComponent<Collider2D>();
        //ignores enemy collisions
        Physics2D.IgnoreCollision(circleColl, backWallCollider);

    }

    internal virtual void SetScaleByHealth(int currentHealth)
    {
        float healthScalar = GameplayConstants.ENEMY_SCALE * Mathf.Pow(GameplayConstants.HEALTH_SIZE_SCALAR, currentHealth - 1);
        this.transform.localScale =  healthScalar * Vector3.one;
    }
    
    public virtual int Squash()
    {
        health -= 1;
        if (health < 1)
        {
            Die();
            return 1;
        }
        else
        {
            SetScaleByHealth(health);
            StartCoroutine("Invulnerable");
            return 0;
        }
    }

    internal IEnumerator Invulnerable()
    {
        isVulnerable = false;
        yield return new WaitForSeconds(0.5f);
        isVulnerable = true;
    }

    public virtual void Die()
    {
        sr.color = Color.black;
        isVulnerable = false;
        rb.gravityScale = 3;
        rb.velocity = 10f * Vector2.up;
        StartCoroutine("DelayedDeath");
        Debug.Log("Died");
    }

    internal IEnumerator DelayedDeath()
    {
        // Give the player a chance to jump off.
        yield return new WaitForSeconds(0.5f);
        circleColl.isTrigger = true;
    }
}
