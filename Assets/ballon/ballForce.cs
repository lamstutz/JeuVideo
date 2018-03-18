using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (GameObject))]
public class ballForce : MonoBehaviour {

	private Rigidbody2D ball;
	// Use this for initialization
	void Start () {
		ball = GetComponent<Rigidbody2D>();
		ball.AddForce(new Vector2(80,10));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
