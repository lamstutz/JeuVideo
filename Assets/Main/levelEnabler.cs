using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelEnabler : MonoBehaviour {

	private BoxCollider2D collide;
	// Use this for initialization
	void Start () {
		collide = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
