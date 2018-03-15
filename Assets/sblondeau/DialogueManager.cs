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
		continuevisible = true;
		touch			= 0;
	}

	void Update() {

		// Si touche gauche et option activé
		if((Input.GetAxis("Horizontal") < 0) && optionvisible){
			GameObject choix1 = GameObject.Find("Choix1");
			GameObject choix2 = GameObject.Find("Choix2");
			choix1.GetComponent<Button>().Select();
		}

		if((Input.GetAxis("Horizontal") > 0) && optionvisible){
			GameObject choix1 = GameObject.Find("Choix1");
			GameObject choix2 = GameObject.Find("Choix2");
			choix2.GetComponent<Button>().Select();
		}

		if(Input.GetKey("space") || (Input.GetKey(KeyCode.Return)) || Input.GetKey("a") || Input.GetKeyDown("joystick button 0")){
			touch++;
		}else{
			touch = 0;
		}

		// Touche a entré et space pour le bouton continue si visible
		if((Input.GetKey("space") || (Input.GetKey(KeyCode.Return)) || Input.GetKey("a") || Input.GetKeyDown("joystick button 0")) && continuevisible && touch == 1){
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
	public void StartDialogue (string nomGO)
	{
		// Initialisation des variables
		sentences 		= new Queue<string>();
		dialogues		= new List<Dialogue>();
		index 			= 0;
		touch			= 0;

		// Choix du dialorue
		choixDialogue(nomGO);

		// Animation de la box
		animator.SetBool("IsOpen", true);

		// Cache les boutons de choix
		cacherOption();

		if(!continuevisible){
			gos = GameObject.FindGameObjectsWithTag("continue");
			initPosn = gos[0].transform.position;
			gos[0].transform.position = new Vector3(initPosn.x + 10000, initPosn.y + 10000, initPosn.z);
		}

		option 			= dialogues[index].option;	// Affichage ou non 
		nameText.text   = dialogues[index].name;	// Nom de personnage
		countT 			= dialogues.Count;			// Nombre de dialogue		

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
			if(dialogues[index].option){
				afficherOption();
			}else{
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

	public void choix1()
	{

		// Sélection du dialogue
		choixDialogue("police_man_1");

		// Cache les boutons de choix
		cacherOption();

		// Afficher le continue si cacher
		continuevisible = true;
		gos = GameObject.FindGameObjectsWithTag("continue");
		initPosn = gos[0].transform.position;
		gos[0].transform.position = new Vector3(initPosn.x + 10000, initPosn.y + 10000, initPosn.z);

		index 			= 0;
		option 			= dialogues[index].option;	// Affichage ou non 
		nameText.text   = dialogues[index].name;	// Nom de personnage
		countT 			= dialogues.Count;			// Nombre de dialogue		

		ParcoursText();

		DisplayNextSentence();
	}

	public void choix2()
	{
		// Choix du dialogue
		choixDialogue("police_man_2");

		// Cache les boutons de choix
		cacherOption();

		// Afficher le continue si cacher
		continuevisible = true;
		gos = GameObject.FindGameObjectsWithTag("continue");
		initPosn = gos[0].transform.position;
		gos[0].transform.position = new Vector3(initPosn.x + 10000, initPosn.y + 10000, initPosn.z);

		index 			= 0;
		option 			= dialogues[index].option;	// Affichage ou non 
		nameText.text   = dialogues[index].name;	// Nom de personnage
		countT 			= dialogues.Count;			// Nombre de dialogue		

		ParcoursText();

		DisplayNextSentence();
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
			case "Nao":
				// Script de lancement
				sentences  = new String[] {"Où suis-je ?", "Il n'y a rien ici.", "Je devrais aller trouver des informations."};
				unDialogue = new Dialogue("...", sentences, false);
				dialogues.Add(unDialogue);
				break;
			case "police_man":
				// Premier dialogue du policier
				// Interaction avec le policier
				sentences  = new String[] {"Hey, robot qu’est ce que tu fais là ?", "Tu n’as rien à faire seul ici.", };
				unDialogue = new Dialogue("Policier", sentences, false);
				dialogues.Add(unDialogue);
				// Réponse de Nao
				sentences  = new String[] {"Bonjour humain. Je m'appelle Nao.", "Je suis perdu. Où suis-je ?" };
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
			case "police_man_1":
				// Question de Nao
				sentences  = new String[] {"Quel direction dois-je prendre ?"};
				unDialogue = new Dialogue("Nao", sentences, false);
				dialogues.Add(unDialogue);
				// Réponse du policier
				sentences  = new String[] {"Suis le chemin de la forêt vers l’Est, tu atteindras la ville facilement.", "Tu ferais bien d’y aller tu me semble complètement désorienté."};
				unDialogue = new Dialogue("Policier", sentences, false);
				dialogues.Add(unDialogue);
				break;
			case "police_man_2":
				// Question de Nao
				sentences  = new String[] {"Que fais-je ici ?"};
				unDialogue = new Dialogue("Nao", sentences, false);
				dialogues.Add(unDialogue);
				// Réponse du policier
				sentences  = new String[] {"En te regardant, je pense que tu allais en direction de la décharge.", 
				"Tu as du tombé d’un camion.", "Suis le chemin de la forêt vers l’Est, tu atteindras la ville facilement.",
				"Tu ferais bien d’y aller tu me semble complètement désorienté."};
				unDialogue = new Dialogue("Policier", sentences, false);
				dialogues.Add(unDialogue);
				break;
			
			case "Girl":
				// Dialogue avec la petite fille
				sentences  = new String[] {"Snif sninf...","J'ai perdu mon ballon..."};
				unDialogue = new Dialogue("...", sentences, false);
				dialogues.Add(unDialogue);
				break;
			case "EntrerVille":
				// Dialogue avec la petite fille
				sentences  = new String[] {"Voila la ville. On pourra me donner des indications.", "Je ne devrais pas être là pourtant j'y suis"};
				unDialogue = new Dialogue("...", sentences, false);
				dialogues.Add(unDialogue);
				break;
				
		}
	}
}
