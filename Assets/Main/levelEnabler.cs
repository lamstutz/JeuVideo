using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelEnabler : MonoBehaviour {

	private BoxCollider2D collide;
	//private currentLevel level;
	// Use this for initialization
	void Start () {
		collide = GetComponent<BoxCollider2D>();
		//level = GetComponent<currentLevel>();
		collide.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void active(){
		collide.enabled = true;
	}

	private void OnTriggerEnter2D (Collider2D objectCol) {
		//if (level.level > 1) {
			if (objectCol.name != "barriere" && objectCol.name != "barriereEnd") {
				active ();
			}
		//}
	}
}
