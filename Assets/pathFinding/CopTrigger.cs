using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CopTrigger : MonoBehaviour {

	public UnityEvent actionOnTrigger;

	private void OnTriggerEnter2D (Collider2D other) {
		if (actionOnTrigger != null) actionOnTrigger.Invoke ();
	}
}