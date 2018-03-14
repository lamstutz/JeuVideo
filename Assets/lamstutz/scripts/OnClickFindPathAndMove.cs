using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using pathfinding;
using UnityEngine;

//using CielaSpike;
using System.Threading;


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





	Thread ChildThread = null;
    EventWaitHandle ChildThreadWait = new EventWaitHandle(true, EventResetMode.ManualReset);
    EventWaitHandle MainThreadWait = new EventWaitHandle(true, EventResetMode.ManualReset);





	void Start() { 
		print("START PATHFINDING");
		grid = GameObject.Find("/trackerMove/grid").GetComponent<Grider>();
		currentPath = GameObject.Find("/trackerMove/polyPath").GetComponent<Polyline>();
	}

	void Update() {
		if(started && !ended){
			pather();
		}
	}

    private bool Collide(Vector2Int cellPos)
    {
        if (pathCollider == null) return false;
			return pathCollider.OverlapPoint(grid.CellToWorld(cellPos));
    }

	private void pather(){
		targetV2 = new Vector2(targetObject.position.x,targetObject.position.y);
		if (
			grid != null && 
			(currentTarget == null || (Vector2.Distance(targetV2, currentTarget) > 2.0f ))
		) {
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
			fin = false;				
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
			//ended = true;
			print("FINISH PATHFINDING");
		}
	}


	public void startPath(string str){
		string[] toMoveTarget = str.Split('|');
		objectToMove = GameObject.Find(toMoveTarget[0]).GetComponent<Transform>();
		targetObject = GameObject.Find(toMoveTarget[1]).GetComponent<Transform>();
		started = true;
	}



using UnityEngine;
 
public class ThreadedBehaviour : MonoBehaviour
{
    
 
    void ChildThreadLoop()
    {
        ChildThreadWait.Reset();
        ChildThreadWait.WaitOne();
 
        while(true)
        {
            ChildThreadWait.Reset();
 
            // Do Update
 
            WaitHandle.SignalAndWait(MainThreadWait, ChildThreadWait);
        }
    }
 
    void Awake()
    {
        ChildThread = new Thread(ChildThreadLoop);
        ChildThread.Start();
        }
 
    void Update()
    {
        MainThreadWait.Reset();
        WaitHandle.SignalAndWait(ChildThreadWait, MainThreadWait);
    }
}


}