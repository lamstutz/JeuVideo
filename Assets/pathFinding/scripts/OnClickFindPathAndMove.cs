using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using pathfinding;
using UnityEngine;

public class OnClickFindPathAndMove : MonoBehaviour {


	private Transform objectToMove;
	private Transform targetObject;
	public Collider2D pathCollider;

	private Vector3? moveTo;
	private bool fin; 
	private Polyline currentPath;
	private Grider grid;
	private Vector2 currentTarget;
	private Vector2 targetV2;
	private bool started = false;
	private bool ended = false;
	private string callback = "";
	private string objectToMoveName = "";

	void Start() { 
		print("START PATHFINDING");
		grid = GameObject.Find("/trackerMove/grid").GetComponent<Grider>();
		currentPath = GameObject.Find("/trackerMove/polyPath").GetComponent<Polyline>();
	}

	void Update() {
		if(started && !ended){
			targetV2 = new Vector2(targetObject.position.x,targetObject.position.y);

			//pathfinding 
			if (grid != null && (currentTarget == null || (Vector2.Distance(targetV2, currentTarget) > 2.0f ))) {
				fin = false;
				Vector3 origin = objectToMove.position;
				var v3 = targetObject.position;
				v3.z = 10.0f;
				Vector3 target =  v3;
				
				var findedPath = AStar
					.FindPath(grid.WorldToCell(origin), grid.WorldToCell(target), Collide,targetV2,currentTarget);

				if(findedPath != null){
					currentPath.nodes = findedPath.Select(pos => (Vector3) grid.CellToWorld(pos))
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
			if(!fin && currentPath.nodes.Count == 0 && (Vector2.Distance(targetV2, currentTarget) <= 2.0f ) ){
				fin = true;
				ended = true;
				print("FINISH PATHFINDING");

				if(callback != null && callback == "dialogue"){
					GameObject.Find(objectToMoveName).GetComponent<DialogueTrigger>().TriggerDialogue(objectToMoveName);
				}

			}
		}
	}

    private bool Collide(Vector2Int cellPos)
    {
        if (pathCollider == null) return false;
			return pathCollider.OverlapPoint(grid.CellToWorld(cellPos));
    }


	public void startPath(string str){
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


}