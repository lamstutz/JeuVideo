using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public Text nameText;
	public Text dialogueText;
	public Text choix1Text;
	public Text choix2Text;
	public Animator animator;

	private currentLevel level;

	private Queue<string> sentences;
	private GameObject[] gos;
	private List<Dialogue> dialogues;
	private bool option;
	private int index;
	private int countT;
	private Boolean optionvisible; // Vrai ils sont cliquables sinon non
	private Boolean continuevisible; // Vrai ils sont cliquables sinon non

	private GameObject GOChoix1;
	private	GameObject GOChoix2;
	private GameObject GOContinue;
	private Vector3 PosChoix1;
	private Vector3 PosChoix2;
	private Vector3 PosContinue;

	private int selected; // 0 : aucun sélect, 1: sélect choix 1, 2: sélect choix 2.
	private string nomDialogueEnCours;
	
	private int	touch;
private Renderer ball;
	// Use this for initialization
	void Start () {
		sentences 		= new Queue<string>();
		index 			= 0;
		touch			= 0;

		level = GameObject.Find("/level").GetComponent<currentLevel>();;
		ball = GameObject.Find("ball").GetComponent<Renderer>();

		// Initialisation des variables boutons
		GOChoix1 = GameObject.Find("Choix1");
		GOChoix2 = GameObject.Find("Choix2");
		GOContinue = GameObject.Find("ContinueButton");
		PosChoix1 = GOChoix1.transform.position;
		PosChoix2 = GOChoix2.transform.position;
		PosContinue = GOContinue.transform.position;
	}

	void Update() {

		// Si touche gauche et option sont activés
		if((Input.GetAxis("Horizontal") < 0) && optionvisible){
			choix1Text.color = Color.black;
			choix2Text.color = Color.grey;
			//GOChoix1.GetComponent<Button>().Select();
			selected = 1;
		}

		if((Input.GetAxis("Horizontal") > 0) && optionvisible){
			//GOChoix2.GetComponent<Button>().Select();
			choix1Text.color = Color.grey;
			choix2Text.color = Color.black; // Noir
			selected = 2;
		}

		if(Input.GetKey("space") || (Input.GetKey(KeyCode.Return)) || Input.GetKey("a") || Input.GetKeyDown("joystick button 0")){
			touch++;
		}else{
			touch = 0;
		}

		// Touche a, entrée, et espace pour le bouton continue si visible
		if((Input.GetKey("space") || (Input.GetKey(KeyCode.Return)) || Input.GetKey("a") || Input.GetKeyDown("joystick button 0")) && continuevisible && touch == 1 && selected == 0){
			DisplayNextSentence();
		}

		//Touche a, entrée, et espace pour le bouton continue si visible
		if((Input.GetKey("space") || (Input.GetKey(KeyCode.Return)) || Input.GetKey("a") || Input.GetKeyDown("joystick button 0")) && selected > 0 && touch == 1){
			switch (selected)
			{
				case 1:
					choix1();
					break;
				case 2:
					choix2();
					break;
			}
		}

		// Quitter la boite de dialogue
		if(Input.GetKeyDown(KeyCode.Escape)){
			EndDialogue();
		}
	}

	
	void InitializatonNewDialogue(){

		index 			= 0;
		touch			= 0;
		selected		= 0;

		// Cache les boutons de choix
		cacherOption();

		// Affichage du bouton continue
		continuevisible = true;

		option 			= dialogues[index].option;	// Affichage ou non des options
		nameText.text   = dialogues[index].name;	// Nom de personnage
		countT 			= dialogues.Count;			// Nombre de dialogue	

		choix1Text.color = Color.grey;
		choix2Text.color = Color.grey;
	}
	public void StartDialogue (string nomGO)
	{
		// Initialisation des variables
		sentences 		= new Queue<string>();
		dialogues		= new List<Dialogue>();

		// Animation de la box
		animator.SetBool("IsOpen", true);

		// Choix du text dialogue
		choixDialogue(nomGO);

		InitializatonNewDialogue();

		ParcoursText();

		DisplayNextSentence();
		
	}
	void afficherOption () {
		// Les options sont affichées et cliquables
		optionvisible = true;

		// Cacher les deux boutons
		GOChoix1.transform.position = new Vector3(PosChoix1.x, PosChoix1.y, PosChoix1.z);
		GOChoix2.transform.position = new Vector3(PosChoix2.x, PosChoix2.y, PosChoix2.z);
		
		// Changement de texte des options
		choix1Text.text = dialogues[index].options[0];
		choix2Text.text = dialogues[index].options[1];

		// On cache le bouton continue
		continuevisible = false;
		GOContinue.transform.position = new Vector3(PosContinue.x - 10000, PosContinue.y - 10000, PosContinue.z);
	}

	void cacherOption () {

		// Indicateur de visibilité des options.
		optionvisible 	= false;

		// Cacher les deux boutons
		GOChoix1.transform.position = new Vector3(PosChoix1.x - 10000, PosChoix1.y - 10000, PosChoix1.z);
		GOChoix2.transform.position = new Vector3(PosChoix2.x - 10000, PosChoix2.y - 10000, PosChoix2.z);

	}

	void ParcoursText(){
		// Vide la queue des textes
		sentences.Clear();

		// Parcours des textes
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

					option 			= dialogues[index].option;	// Affichage ou non des options 
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
			SoundEffectsHelper.Instance.MakeLetterSound();
			dialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue()
	{
		animator.SetBool("IsOpen", false);
		if(nomDialogueEnCours == "ball"){
			level.level = 3;
		}
		if(nomDialogueEnCours == "girl" || nomDialogueEnCours == "girl_2"){
			level.level = 2;
			ball.enabled = true;
		}
	}

	public void choix1()
	{

		// Sélection du dialogue selon le parent
		switch (nomDialogueEnCours)
		{
			case "girl":
				EndDialogue();
				break;
			case "police_man":
				choixDialogue("police_man_1");
				break;
		}

		InitializatonNewDialogue();
		
		// Afficher le bouton "continue" si caché
		GOContinue.transform.position = new Vector3(PosContinue.x, PosContinue.y, PosContinue.z);	

		ParcoursText();

		DisplayNextSentence();
	}

	public void choix2()
	{
		// Sélection du dialogue selon le parent
		switch (nomDialogueEnCours)
		{
			case "girl":
				choixDialogue("girl_2");
				break;
			case "police_man":
				choixDialogue("police_man_2");
				break;
		}

		InitializatonNewDialogue();
		
		// Afficher le bouton "continue" si caché
		GOContinue.transform.position = new Vector3(PosContinue.x, PosContinue.y, PosContinue.z);	

		ParcoursText();

		DisplayNextSentence();
	}

	void choixDialogue(string nom){

		// On met de coté le nom du dialogue en cours
		nomDialogueEnCours = nom;

		if(dialogues.Count > 0){
			dialogues.Clear();
		}
		string[] sentences;
		string[] options;
		Dialogue unDialogue;

		// Selon le nom du GameObject, le dialogue sera différent
		switch (nom)
		{
			case "Nao":
				// Script de lancement
				sentences  = new String[] {"Où suis-je ?", "Il n'y a rien ici.", "Je devrais aller trouver des informations."};
				options  = new String[] {};
				unDialogue = new Dialogue("...", sentences, false, options);
				dialogues.Add(unDialogue);
				break;
			case "police_man":
				// Premier dialogue du policier
				// Interaction avec le policier
				sentences  = new String[] {"Hey, robot qu’est ce que tu fais là ?", "Tu n’as rien à faire seul ici."};
				options  = new String[] {};
				unDialogue = new Dialogue("Policier", sentences, false, options);
				dialogues.Add(unDialogue);
				// Réponse de Nao
				sentences  = new String[] {"Bonjour humain. Je m'appelle Nao.", "Je suis perdu. Où suis-je ?" };
				options  = new String[] {};
				unDialogue = new Dialogue("Nao", sentences, false, options);
				dialogues.Add(unDialogue);
				// Réponse du policier
				sentences  = new String[] {"Dans la forêt à proximité de la ville “404land”."};
				options  = new String[] {};
				unDialogue = new Dialogue("Policier", sentences, false, options);
				dialogues.Add(unDialogue);
				// Demande de Nao
				sentences  = new String[] {"( Que demander à cette personne  ? )"};
				options  = new String[] {"Une Direction", "Plus d'information"};
				unDialogue = new Dialogue("Nao", sentences, true, options);
				dialogues.Add(unDialogue);
				break;
			case "police_man_1":
				// Question de Nao
				sentences  = new String[] {"Quelle direction dois-je prendre ?"};
				options  = new String[] {};
				unDialogue = new Dialogue("Nao", sentences, false, options);
				dialogues.Add(unDialogue);
				// Réponse du policier
				sentences  = new String[] {"Suis le chemin de la forêt vers l’Est, tu atteindras la ville facilement.", "Tu ferais bien d’y aller, tu me sembles complètement désorienté."};
				options  = new String[] {};
				unDialogue = new Dialogue("Policier", sentences, false, options);
				dialogues.Add(unDialogue);
				break;
			case "police_man_2":
				// Question de Nao
				sentences  = new String[] {"Que fais-je ici ?"};
				options  = new String[] {};
				unDialogue = new Dialogue("Nao", sentences, false, options);
				dialogues.Add(unDialogue);
				// Réponse du policier
				sentences  = new String[] {"En te regardant, je pense que tu allais en direction de la décharge.", 
				"Tu as dû tombé d’un camion.", "Suis le chemin de la forêt vers l’Est, tu atteindras la ville facilement.",
				"Tu ferais bien d’y aller, tu me sembles complètement désorienté."};
				options  = new String[] {};
				unDialogue = new Dialogue("Policier", sentences, false, options);
				dialogues.Add(unDialogue);
				break;
			case "EntrerVille":
				// Dialogue Nao
				sentences  = new String[] {"Voila la ville. On pourra me donner des indications.", "Je ne devrais pas être là pourtant j'y suis."};
				options  = new String[] {};
				unDialogue = new Dialogue("Nao", sentences, false, options);
				dialogues.Add(unDialogue);
				break;
			case "girl":
				// Dialogue avec la petite fille
				sentences  = new String[] {"Snif snif...","J'ai perdu mon ballon...", "Je n'arrive pas à le retrouver.", "Personne ne peut m'aider ?"};
				options  = new String[] {"Partir", "Plus d'information"};
				unDialogue = new Dialogue("Petite fille", sentences, true, options);
				dialogues.Add(unDialogue);
				break;
			case "girl_2":
				// Question Nao à la petite fille
				sentences  = new String[] {"Bonjour petite humaine, comment t'appelles - tu ?"};
				options  = new String[] {};
				unDialogue = new Dialogue("Nao", sentences, false, options);
				dialogues.Add(unDialogue);
				// Réponse de la petite fille
				sentences  = new String[] {"Nina et toi ?"};
				options  = new String[] {};
				unDialogue = new Dialogue("Petite fille", sentences, false, options);
				dialogues.Add(unDialogue);
				// Réponse de Nao
				sentences  = new String[] {"Mon nom est Nao et j'aimerais d'aider.", "A quoi ressemble ton ballon ?"};
				options  = new String[] {};
				unDialogue = new Dialogue("Nao", sentences, false, options);
				dialogues.Add(unDialogue);
				// Réponse de Nina
				sentences  = new String[] {"Oh merci. Il est rond et tout rose !"};
				options  = new String[] {};
				unDialogue = new Dialogue("Nina", sentences, false, options);
				dialogues.Add(unDialogue);
				break;
			case "ball":
				// Dialogue avec la petite fille
				sentences  = new String[] {"Mon ballon ! Merci monsieur le robot !", "Vous êtes vraiment gentil."};
				options  = new String[] {};
				unDialogue = new Dialogue("Petite fille", sentences, false, options);
				dialogues.Add(unDialogue);
				break;
			default:
				break;
				
		}
	}
}
