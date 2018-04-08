using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ballColliderDisabler : MonoBehaviour {

	// Use this for initialization
	private CircleCollider2D circleCol2D;
	private DialogueTrigger dt;

	void Start()
	{
		circleCol2D = GetComponent<CircleCollider2D>();
		dt = GetComponent<DialogueTrigger> ();
	}

	private void disabledColliderAndStartDialogue(){
		circleCol2D.enabled = false;
		dt.TriggerDialogue ("ball");
	}

	private void OnTriggerEnter2D (Collider2D objectCol) {
		if (objectCol.name == "ball") disabledColliderAndStartDialogue();
	}

}
