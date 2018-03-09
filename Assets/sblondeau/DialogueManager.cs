using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public Text nameText;
	public Text dialogueText;

	public Animator animator;

	private Queue<string> sentences;

	private GameObject[] gos;

	private Vector3 otherPosn;

	// Use this for initialization
	void Start () {
		sentences 	= new Queue<string>();
	}

	public void StartDialogue (Dialogue dialogue)
	{

		animator.SetBool("IsOpen", true);
		nameText.text = dialogue.name;

		gos 		  = GameObject.FindGameObjectsWithTag("choix");
		if(gos.Length > 0){
			otherPosn = gos[0].transform.position;

			foreach (GameObject go in gos)
			{
				otherPosn = go.transform.position;
				go.transform.position = new Vector3(otherPosn.x-10000, otherPosn.y-10000, otherPosn.z);
			}
		}
	
		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
		if (sentences.Count == 0)
		{
			// S'il y a des options, affichage de ces derniers
			if(gos.Length > 0){

				// Affichage des options
				foreach (GameObject go in gos)
				{
					otherPosn = go.transform.position;
					go.transform.position = new Vector3(otherPosn.x + 10000, otherPosn.y + 10000, otherPosn.z);
				}

				// On cache le bouton continue
				gos = GameObject.FindGameObjectsWithTag("continue");
				otherPosn = gos[0].transform.position;
				gos[0].transform.position = new Vector3(otherPosn.x - 10000, otherPosn.y - 10000, otherPosn.z);

	
			}else{
				// Sinon fin de dialogue
				EndDialogue();
				return;
			}

		}else{
			string sentence = sentences.Dequeue();
			StopAllCoroutines();
			StartCoroutine(TypeSentence(sentence));
		}		
	}

	IEnumerator TypeSentence (string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue()
	{
		animator.SetBool("IsOpen", false);
	}

	public void choixAttendre()
	{
		// A modifier
		EndDialogue();
		return;
	}

	public void choixPartir()
	{
		// A modifier
		EndDialogue();
		return;
	}

}
