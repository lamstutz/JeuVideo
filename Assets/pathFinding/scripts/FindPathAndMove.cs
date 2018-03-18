using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;


public class FindPathAndMove : MonoBehaviour {


	private Transform objectToMove;
	private Transform targetObject;
	
	private TilemapCollider2D mapCollider;
	private Tilemap tileMap;

	private Vector3? moveTo;
	private bool fin; 
	private Polyline currentPath;
	private Vector2 currentTarget;
	private Vector2 targetV2;



	private bool started = false;
	private bool ended = false;
	private string callback = "";
	private string objectToMoveName = "";

	void Start() { 
		print("START OnClickFindPathAndMove");
		currentPath = GameObject.Find("/pathfinding/polyPath").GetComponent<Polyline>();
		var CollisionTilemap = GameObject.Find("/background/collisionTilemap");
		mapCollider = CollisionTilemap.GetComponent<TilemapCollider2D>();
		tileMap = CollisionTilemap.GetComponent<Tilemap>();
	}

	void Update() {
		if(started && !ended){
			targetV2 = new Vector2(targetObject.position.x,targetObject.position.y);

			//pathfinding 
			if (currentTarget == null || Vector2.Distance(targetV2, currentTarget) > 2.5f) {
				fin = false;
				Vector3 origin = tileMap.WorldToCell(objectToMove.position);
				Vector3 target =  tileMap.WorldToCell(targetObject.position);
				
				var findedPath = AStar
					.FindPath(origin, target, mapCollider);

				if(findedPath != null){
					currentPath.nodes = findedPath.Select(pos => (Vector3) pos)
					.ToList<Vector3>();
				}
				currentTarget = new Vector2(target.x, target.y);

					
				moveTo = null;
			}

			//arrivé au noeud courant
			if (!fin && moveTo.HasValue && Vector2.Distance(moveTo.Value, objectToMove.position) < 0.1f) {
				moveTo = null;
			}

			//prise en compte du prochain noeud
			if (currentPath.nodes.Count > 0) {
				if (!moveTo.HasValue) {
					moveTo = currentPath.nodes[0];
					currentPath.nodes.RemoveAt(0);
				}
			}

			//deplacement
			if (moveTo.HasValue) {
				this.objectToMove.position = Vector2.MoveTowards(this.objectToMove.position, moveTo.Value, 0.05f);
			}

			//arrivé à la cible
			if(!fin && currentPath.nodes.Count == 0 && (Vector2.Distance(targetV2, currentTarget) <= 2.5f ) && (Vector2.Distance(targetV2, objectToMove.position) <= 2.5f) ){
				fin = true;
				ended = true;
				print("FINISH PATHFINDING");
				callCallback();

			}
		}
	}

   


	public void startPath(string str){
		print("START PATHFINDING");
		string[] toMoveTarget = str.Split('|');
		objectToMoveName = toMoveTarget[0];
		objectToMove = GameObject.Find(objectToMoveName).GetComponent<Transform>();
		targetObject = GameObject.Find(toMoveTarget[1]).GetComponent<Transform>();
		if(toMoveTarget.Length > 2){
			callback = toMoveTarget[2];
		}else{
			callback = "";
		}
		started = true;
		ended = false;
	}

	private void callCallback(){
		if(callback != null && callback == "dialogue"){
			print("Call Callback for :");
			print(objectToMoveName);
			GameObject.Find(objectToMoveName).GetComponent<DialogueTrigger>().TriggerDialogue(objectToMoveName);
		}
	}


}