using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRobot : MonoBehaviour
{
	public float acceleration = 8f; // unit per second, per second

	private AnimationCourse ac;
	private Rigidbody2D rigidBody2D;

	void Start()
	{
		// Récupère une référence au script AnimationCourse attaché au même GameObject
		ac = GetComponent<AnimationCourse>();
		rigidBody2D = GetComponent<Rigidbody2D>();

	}


	void FixedUpdate()
	{
		forceByKey();
	}


	private void forceByKey()
	{
		Vector2 force = Vector2.zero;


		if (Input.GetKey (KeyCode.LeftArrow)) 
		{
			force.x -= acceleration;
		}
		if (Input.GetKey (KeyCode.RightArrow)) 
		{
			force.x += acceleration;
		}
		if (Input.GetKey (KeyCode.UpArrow)) 
		{
			force.y += acceleration;
		}
		if (Input.GetKey (KeyCode.DownArrow)) 
		{
			force.y -= acceleration;
		}

		// Debug.Log ("force " + force);

		// apply force
		rigidBody2D.AddForce (force);

		ac.SetAnimationFromSpeed(Input.GetAxis("Horizontal") + 0.001f * force.magnitude);

	}
}


