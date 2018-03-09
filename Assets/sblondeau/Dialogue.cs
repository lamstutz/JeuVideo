using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {

	public string name;
	[TextArea(3, 10)]
	public string[] sentences;
	public bool option;

	public Dialogue(string name, string[] sentences, bool option){
		this.name = name;
		this.sentences = sentences;
		this.option = option;
	}

}
