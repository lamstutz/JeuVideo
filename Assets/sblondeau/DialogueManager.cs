using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public Text nameText;
	public Text dialogueText;

	public Animator animator;

	public GUIStyle buttonBorderStyle;

	private Queue<string> sentences;
	private GameObject[] gos;
	private Vector3 otherPosn;
	private Vector3 initPosn;
	private Dialogue[] dialogues;
	private bool option;
	private int index;
	private int countT;
	private Boolean optionvisible; // Vrai ils sont cliquable sinon non
	private Boolean continuevisible; // Vrai ils sont cliquable sinon non
	private int	touch;

	// Use this for initialization
	void Start () {
		sentences 		= new Queue<string>();
		index 			= 0;
		continuevisible = false;
		touch			= 0;
	}

	void cacherOption () {

		// Indicateur de visibilité des options.
		optionvisible 	= false;
		gos 		= GameObject.FindGameObjectsWithTag("choix");
		otherPosn 	= gos[0].transform.position;

		foreach (GameObject go in gos)
		{
			otherPosn = go.transform.position;
			go.transform.position = new Vector3(otherPosn.x-10000, otherPosn.y-10000, otherPosn.z);
		}
	}

	void Update() {

		// Si touche gauche et option activé
		if(Input.GetKey("left") && optionvisible){
			GameObject choix1 = GameObject.Find("Choix1");
			GameObject choix2 = GameObject.Find("Choix2");
		}

		if(Input.GetKey("right") && optionvisible){
			GameObject choix1 = GameObject.Find("Choix1");
			GameObject choix2 = GameObject.Find("Choix2");
		}

		if(Input.GetKey("space") || (Input.GetKey(KeyCode.Return)) || Input.GetKey("a")){
			touch++;
		}else{
			touch = 0;
		}

		// Touche a entré et space pour le bouton continue si visible
		if((Input.GetKey("space") || (Input.GetKey(KeyCode.Return)) || Input.GetKey("a")) && continuevisible && touch == 1){
			DisplayNextSentence();
		}
	}

	void afficherOption () {
		// Les options sont affichés et cliquable
		optionvisible = true;
		gos = GameObject.FindGameObjectsWithTag("choix");
		otherPosn = gos[0].transform.position;

		foreach (GameObject go in gos)
		{
			otherPosn = go.transform.position;
			go.transform.position = new Vector3(otherPosn.x+10000, otherPosn.y+10000, otherPosn.z);
		}

		// On cache le bouton continue
		continuevisible = false;
		gos = GameObject.FindGameObjectsWithTag("continue");
		initPosn = gos[0].transform.position;
		gos[0].transform.position = new Vector3(initPosn.x - 10000, initPosn.y - 10000, initPosn.z);
	}

	public void StartDialogue (Dialogue[] dialoguesV, string nomGO)
	{
		// Garde les dialogues pour la suite
		dialogues = dialoguesV;

		// Animation de la box
		animator.SetBool("IsOpen", true);

		// Cache les boutons de choix
		cacherOption();

		option 			= dialogues[index].option;	// Affichage ou non 
		nameText.text   = dialogues[index].name;	// Nom de personnage
		countT 			= dialogues.Length;			// Nombre de dialogue		
		
		continuevisible = true;

		ParcoursText();

		DisplayNextSentence();
		
	}

	void ParcoursText(){
		Debug.Log("ParcoursText");
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
			Debug.Log("sentences dequeue");
			string sentence = sentences.Dequeue();
			StopAllCoroutines();
			StartCoroutine(TypeSentence(sentence));
		}	
	}

	IEnumerator TypeSentence (string sentence)
	{
		Debug.Log("TypeSentece");
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

	void choixDialogue(string nom){
		// Selon le nom du GameObject, le dialogue sera différent
		switch (nom)
		{
			case "Policie":
				// Premier dialogue du policier
				break;
			case "Policie_1":
				// Réponse au choix 1 par le policier
				break;
			case "Policie_2":
				// Réponse au choix 2 par le policier
				break;
			default:
				// Script de lancement
				break;
		}
	}
}
