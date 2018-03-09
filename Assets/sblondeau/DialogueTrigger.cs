using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public Dialogue[] dialogues;

	public void TriggerDialogue ()
	{
		FindObjectOfType<DialogueManager>().StartDialogue(dialogues);
	}

}
