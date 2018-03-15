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
	//private Dialogue[] dialogues;

	private List<Dialogue> dialogues;
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

	public void StartDialogue (string nomGO)
	{
		// Initialisation des variables
		sentences 		= new Queue<string>();
		dialogues		= new List<Dialogue>();
		index 			= 0;
		continuevisible = false;
		touch			= 0;

		// Choix du dialorue
		choixDialogue(nomGO);

		// Animation de la box
		animator.SetBool("IsOpen", true);

		// Cache les boutons de choix
		cacherOption();

		option 			= dialogues[index].option;	// Affichage ou non 
		nameText.text   = dialogues[index].name;	// Nom de personnage
		countT 			= dialogues.Count;			// Nombre de dialogue		
		
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
				Debug.Log("countT " + countT);
				Debug.Log("index " + index);
				// Sinon fin de dialogue
				if(countT == index +1){
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

	public void choix1()
	{
		// Sélection du dialogue
		choixDialogue("Policie_1");
		
		return;
	}

	public void choix2()
	{
		// Choix du dialogue
		choixDialogue("Policie_1");
		return;
	}

	void choixDialogue(string nom){

		if(dialogues.Count > 0){
			dialogues.Clear();
		}
		string[] sentences;
		Dialogue unDialogue;

		// Selon le nom du GameObject, le dialogue sera différent
		switch (nom)
		{
			case "police_man":
				// Premier dialogue du policier
				// Interaction avec le policier
				sentences  = new String[] {"Hey, robot qu’est ce que tu fais là ?", "Tu n’as rien à faire seul ici.", };
				unDialogue = new Dialogue("Policier", sentences, false);
				dialogues.Add(unDialogue);
				// Réponse de Nao
				sentences  = new String[] {"Je ne sais pas, Où suis-je ?" };
				unDialogue = new Dialogue("Nao", sentences, false);
				dialogues.Add(unDialogue);
				// Réponse du policier
				sentences  = new String[] {"Dans la forêt à proximité de la ville “404land”."};
				unDialogue = new Dialogue("Policier", sentences, false);
				dialogues.Add(unDialogue);
				// Demande de Nao
				sentences  = new String[] {"( Que demander à cette personne  ? )"};
				unDialogue = new Dialogue("Nao", sentences, true);
				dialogues.Add(unDialogue);
				break;
			case "Policie_1":
				// Question de Nao
				sentences  = new String[] {"Quel direction dois-je prendre ?"};
				unDialogue = new Dialogue("Nao", sentences, false);
				dialogues.Add(unDialogue);
				// Réponse du policier
				sentences  = new String[] {"Suis le chemin de la forêt vers l’Est, tu atteindre la ville facilement.", "Tu ferais bien d’y aller tu me semble complètement désorienté."};
				unDialogue = new Dialogue("Policier", sentences, false);
				dialogues.Add(unDialogue);
				break;
			case "Policie_2":
				// Question de Nao
				sentences  = new String[] {"Quel direction dois-je prendre ?"};
				unDialogue = new Dialogue("Nao", sentences, false);
				dialogues.Add(unDialogue);
				// Réponse du policier
				sentences  = new String[] {"En te regardant, je pense que tu allais en direction de la décharge.", 
				"Tu as du tombé d’un camion.", "Suis le chemin de la forêt vers l’Est, tu atteindre la ville facilement.",
				"Tu ferais bien d’y aller tu me semble complètement désorienté."};
				unDialogue = new Dialogue("Policier", sentences, false);
				dialogues.Add(unDialogue);
				break;
			case "Nao":
				// Script de lancement
				sentences  = new String[] {"Où suis-je ?", "Il n'y a personne ici.", "Je devrais aller trouver des informations."};
				unDialogue = new Dialogue("...", sentences, false);
				dialogues.Add(unDialogue);
				break;
		}
	}
}
