using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace pathfinding {

    public class Node {
		public Vector3 position;
		public float cout;
		public float heuristique;
		public Node parent = null;

		public Node (Vector3 p, float h, float c) {
			position = p;
			cout = c;
			heuristique = h;
		}

		public float GetFCost() {
			return heuristique + cout;
		}

        public bool samePositionTo (Node obj) {
			return position == obj.position;
		}

		public int CompareCostTo (Node obj) {
			return GetFCost().CompareTo (obj.GetFCost());
		}
	}

    public class AStar {


        public static List<Vector3> FindPath (Vector3 origin, Vector3 target, TilemapCollider2D mapCollider) {

            //init fermee
            List<Node> fermee = new List<Node> ();

            //init ouverte
            List<Node> ouverte = new List<Node> ();

            //init depart
            Node depart = new Node (origin, Vector3.Distance (origin, target), 0);

            //ajout du depart à ouvert
            ouverte.Add (depart);

            //debut de la recherche
            int loopCount = 0;
            while (ouverte.Count > 0 && loopCount < 2000) {
                loopCount ++;

                //on enleve le noeud qui a le plus grand coup de la liste ouverte
                Node current = removeSmallerCost(ouverte);
                //et on l'ajoute à la liste fermée
                fermee.Add (current);

                //si la distance entre la position du noeud courant et de la cible est inferrieur à deux cases, 
                //on termine la recherche
                if (Vector3.Distance(current.position, target) <= 2.0f ){
                    return MakePathFromLastNode (current, new List<Vector3> ());
                } 

                //on recupere les 4 noeuds voisins
                Node[] voisins = CreateAvailableNeighbours (current, target, fermee, current.cout + 1);

                foreach (Node voisin in voisins) {
                    // On parcours le voisin si le nœud est parcourable 
                    // et s'il n'est pas déjà fermé avec un coût moindre
                    if (!IsClosed (voisin, fermee, voisin.GetFCost ()) &&
                        !Collide (voisin.position, mapCollider)
                    ) {


                        // cherche s'il y a déjà un noeud ouvert
                        int indexOfSamePos = findIndexByPos(voisin, ouverte);
                        // s'il n'existe pas, ajouter
                        if (indexOfSamePos == -1) {
                            int indexOfSameCost = findIndexByCost(voisin, ouverte);
                            if(indexOfSameCost == -1){
                                ouverte.Add (voisin);
                            }else{
                                ouverte[indexOfSameCost] = voisin;
                            }
                        }else 
                        // s'il existe déjà mais qu'il est plus couteux, mettre à jour
                        if (ouverte[indexOfSamePos].CompareCostTo(voisin) >= 0) {
                            ouverte[indexOfSamePos].cout = voisin.cout;
                        }
                        

                        voisin.parent = current;

                    }
                }
            }


            return null;
        }

        private static int findIndexByPos (Node node, List<Node> list) {
            for (int i = 0; i < list.Count; i++) {
                if (node.samePositionTo(list[i])) {
                    return i;
                }
            }
            return -1;
        }

        private static int findIndexByCost (Node node, List<Node> list) {
            for (int i = 0; i < list.Count; i++) {
                if (node.CompareCostTo(list[i]) == 0) {
                    return i;
                }
            }
            return -1;
        }

        private static Node removeSmallerCost(List<Node> list){
            int indexNodeLarger = 0;
            for (int i = 1; i < list.Count; i++) {
                if (list[i].CompareCostTo(list[indexNodeLarger])  < 0 ) {
                    indexNodeLarger = i;
                }
            }
            Node nodeLarger = list[indexNodeLarger];
            list.RemoveAt(indexNodeLarger);
            return nodeLarger;
        }

        private static bool IsClosed (Node voisin, List<Node> fermee, float voisinFCost) {
            for (int i = 0; i < fermee.Count; i++) {
                Node noeudFerme = fermee[i];
                if (noeudFerme.position == voisin.position && noeudFerme.GetFCost () < voisinFCost) return true;
            }
            return false;
        }

        private static Node[] CreateAvailableNeighbours (Node current, Vector3 target, List<Node> closed, float cout) {
            Node[] nodes = new Node[4];
            Vector3 up = current.position + Vector3.up;
            nodes[0] = new Node (up, Vector3.Distance (up, target), cout);
            Vector3 right = current.position + Vector3.right;
            nodes[1] = new Node (right, Vector3.Distance (right, target), cout);
            Vector3 left = current.position + Vector3.left;
            nodes[2] = new Node (left, Vector3.Distance (left, target), cout);
            Vector3 down = current.position + Vector3.down;
            nodes[3] = new Node (down, Vector3.Distance (down, target), cout);
            return nodes;
        }

        private static List<Vector3> MakePathFromLastNode (Node current, List<Vector3> list) {
            if (current.parent != null) {
                MakePathFromLastNode (current.parent, list);
            }
            list.Add (current.position);
            return list;
        }

        private static bool Collide(Vector3 cellPos, Collider2D mapCollider)
        {
            if (mapCollider == null) return false;
                return mapCollider.OverlapPoint(cellPos);
        }

    }
}