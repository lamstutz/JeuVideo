using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {


	public void TriggerDialogue (string nom)
	{
		FindObjectOfType<DialogueManager>().StartDialogue(nom);
	}

}
