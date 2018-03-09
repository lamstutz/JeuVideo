using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using pathfinding;
using UnityEngine;

public class OnClickFindPathAndMove : MonoBehaviour {

	public Grider grid;
	public Transform objectToMove;
	public Transform targetObject;
	public Polyline currentPath;
	public Collider2D pathCollider;

	public Vector3? moveTo;

	private Vector2 currentTarget;
	private Vector2 targetV2;
	private bool fin; 

	void Start() { }

	void Update() {
		targetV2 = new Vector2(targetObject.position.x,targetObject.position.y);
		if (
			grid != null && 
			(currentTarget == null || (Vector2.Distance(targetV2, currentTarget) > 2.0f ))
		) {
			fin = false;
			print("A");
			print(targetV2);
			print(currentTarget);
			Vector3 origin = objectToMove.position;
			var v3 = targetObject.position;
			v3.z = 10.0f;
			Vector3 target =  v3;

			//print(target);
			// var v3 = Input.mousePosition;
			// v3.z = 10.0f;
			// Vector3 targetCell = Camera.main.ScreenToWorldPoint(v3);

			
			var findedPath = AStar
				.FindPath(grid.WorldToCell(origin), grid.WorldToCell(target), Collide,targetV2,currentTarget);

			if(findedPath != null){
				currentPath.nodes = findedPath.Select(pos => (Vector3) grid.CellToWorld(pos))
				.ToList<Vector3>();
			}
			currentTarget = new Vector2(target.x, target.y);

				
			moveTo = null;
		}
		if (!fin && moveTo.HasValue && Vector2.Distance(moveTo.Value, objectToMove.position) < 0.1f) {
			moveTo = null;
		}
		if (currentPath.nodes.Count > 0) {
			if (!moveTo.HasValue) {
				moveTo = currentPath.nodes[0];
				currentPath.nodes.RemoveAt(0);
			}
		}
		if (moveTo.HasValue) {
			this.objectToMove.position = Vector2.MoveTowards(this.objectToMove.position, moveTo.Value, 0.05f);
		}

		if(!fin && currentPath.nodes.Count == 0 && (currentTarget != null || (Vector2.Distance(targetV2, currentTarget) <= 2.0f ) )){
			fin = true;
		}
	}

    private bool Collide(Vector2Int cellPos)
    {
        if (pathCollider == null) return false;
			return pathCollider.OverlapPoint(grid.CellToWorld(cellPos));
    }


}