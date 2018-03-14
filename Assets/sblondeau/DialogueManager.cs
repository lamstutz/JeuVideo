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

	private Vector3 initPosn;
	private Dialogue[] dialogues;

	private bool option;
	private int index;
	private int countT;

	// Use this for initialization
	void Start () {
		sentences 	= new Queue<string>();
		index 		= 0;
	}

	void cacherOption () {
		gos = GameObject.FindGameObjectsWithTag("choix");
		otherPosn = gos[0].transform.position;

		foreach (GameObject go in gos)
		{
			otherPosn = go.transform.position;
			// SBO: avec cette méthode, si "cacherOption" est appelé plusieurs fois il devient difficile de les retrouver
			// autre méthode : mémoriser la position initiale avant de déplacer
			// puis remettre cette position pour réapparaitre (plutôt que refaire l'opération inverse)
			go.transform.position = new Vector3(otherPosn.x-10000, otherPosn.y-10000, otherPosn.z);
		}
	}

	void afficherOption () {
		gos = GameObject.FindGameObjectsWithTag("choix");
		
		otherPosn = gos[0].transform.position;

		foreach (GameObject go in gos)
		{
			otherPosn = go.transform.position;
			go.transform.position = new Vector3(otherPosn.x+10000, otherPosn.y+10000, otherPosn.z);
		}

		// On cache le bouton continue
		gos = GameObject.FindGameObjectsWithTag("continue");
		initPosn = gos[0].transform.position;
		gos[0].transform.position = new Vector3(initPosn.x - 10000, initPosn.y - 10000, initPosn.z);
	}

	public void StartDialogue (Dialogue[] dialoguesV)
	{
		
		// Garde les dialogues pour la suite
		dialogues = dialoguesV;

		// Animation de la box
		// SBO: l'animator pour faire glisser une boite de dialogue est un peu overkill mais pourquoi pas, ça fonctionne.
		animator.SetBool("IsOpen", true);
		// Cache les boutons de choix
		cacherOption();

		option 			= dialogues[index].option;	// Affichage ou non 
		nameText.text   = dialogues[index].name;	// Nom de personnage
		countT 			= dialogues.Length;			// Nombre de dialogue		
		
		ParcoursText();

		DisplayNextSentence();
		
	}

	void ParcoursText(){
		// Vide la queue des textes
		sentences.Clear();

		// Parcourt des textes
		foreach (string sentence in dialogues[index].sentences)
		{
			sentences.Enqueue(sentence);
		}
	}

	public void DisplayNextSentence ()
	{
		if (sentences.Count == 0)
		{
			// S'il y a des options, affichage de ces derniers
			if(option){
				afficherOption();
			}else{
				// Sinon fin de dialogue
				if(countT <= index -1){
					EndDialogue();
					return;
				}else{
					// Passage au dialogue suivant
					index += 1;

					option 			= dialogues[index].option;	// Affichage ou non 
					nameText.text   = dialogues[index].name;	// Nom de personnage

					ParcoursText();
					DisplayNextSentence();
				}
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
			//SoundEffectsHelper.Instance.MakeLetterSound();
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
