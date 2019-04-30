using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{

	private Rigidbody2D playerbody;

	public float accSpeed = 1.0F;
	public float jumpStrength = 1.0F;
	public float maxSpeed = 100.0F;

	public bool jumpAvaliable = false;

    void Start()
    {
		playerbody = this.GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
    {
		//jump
		if (Input.GetKeyDown(KeyCode.Space) && jumpAvaliable)
		{
			playerbody.AddForce(new Vector2(0.0F, jumpStrength));
			jumpAvaliable = false;
		}

	}

	//used for movement
	private void FixedUpdate()
	{

		//movement horizontal
		if (Input.GetKey(KeyCode.A)) {
			//double speed if on ground
			if (jumpAvaliable) {
				playerbody.AddForce(new Vector2(-accSpeed, 0.0F));
			}
			playerbody.AddForce(new Vector2(-accSpeed, 0.0F));
		}
		if (Input.GetKey(KeyCode.D))
		{
			//double speed if on ground
			if (jumpAvaliable)
			{
				playerbody.AddForce(new Vector2(accSpeed, 0.0F));
			}
			playerbody.AddForce(new Vector2(accSpeed, 0.0F));
		}

		//cap speed
		if (playerbody.velocity.x > maxSpeed) {
			playerbody.velocity = new Vector3(maxSpeed, playerbody.velocity.y);
		}
		if (playerbody.velocity.x < -maxSpeed)
		{
			playerbody.velocity = new Vector3(-maxSpeed, playerbody.velocity.y);
		}

	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Ground") {
			jumpAvaliable = true;
		};
	}
}

