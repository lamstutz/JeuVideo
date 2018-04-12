using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barriere : MonoBehaviour {

	private float start = 0.0f;
	private float closed = 0.8f;
	private float opened = -7.0f;
	private float duration = 2.0f;
	private float elapsedTime = 0.0f;
	private float currentPos;

	public currentLevel level;
	public int openLevel = 3; 

	public Transform elemTransform;
	// Use this for initialization
	void Start () {
		//elemTransform = GetComponent<Transform>();
		currentPos = closed;
	}
	
	// Update is called once per frame
	void Update () {
		float y = EaseOut(start, currentPos,
			elapsedTime, duration);
		elapsedTime += Time.deltaTime;
		Vector3 target = new Vector3 (elemTransform.position.x, y,1.0f);
		elemTransform.position = Vector2.MoveTowards(elemTransform.position,target, 0.1f);
	}

	private float EaseOut(float start, float distance, float elapsedTime, float duration) {
		//elapsedTime = (elapsedTime > duration) ? 1.0f : elapsedTime / duration;
		//elapsedTime--;
		//return (distance * (elapsedTime * elapsedTime * elapsedTime + 1.0f) + start) * 1.0f;
		elapsedTime = (elapsedTime > duration) ? 2.0f : elapsedTime / (duration / 2);
		if (elapsedTime < 1) return distance / 2 * elapsedTime * elapsedTime * elapsedTime * elapsedTime + start;
		elapsedTime -= 2;
		return -distance / 2 * (elapsedTime * elapsedTime * elapsedTime * elapsedTime - 2) + start;
	}

	public void open(){
		if (level.level >= openLevel) {
			currentPos = opened;
			elapsedTime = 0.0f;
		}
	}

	public void close(){
		currentPos = closed;
		elapsedTime = 0.0f;
	}

	private void OnTriggerEnter2D (Collider2D objectCol) {
		if (objectCol.name != "barriere" && objectCol.name != "barriereEnd" ) open();
	}
	private void OnTriggerExit2D (Collider2D objectCol) {
		if (objectCol.name != "barriere" && objectCol.name != "barriereEnd" ) close();
	}



}
