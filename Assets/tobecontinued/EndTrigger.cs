using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class EndTrigger : MonoBehaviour {

	public Animator animator;
	
	public void TriggerEnd ()
	{
		// Animation de la box
		animator.SetBool("IsOpen", true);
	}

	private void OnTriggerEnter2D (Collider2D objectCol) {
		TriggerEnd();
	}

}
