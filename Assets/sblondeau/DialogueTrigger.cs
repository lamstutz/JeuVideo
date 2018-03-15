using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public List<Dialogue> dialogues;
	public string nomGO;

	public void TriggerDialogue ()
	{
		FindObjectOfType<DialogueManager>().StartDialogue(dialogues, nomGO);
	}

}
