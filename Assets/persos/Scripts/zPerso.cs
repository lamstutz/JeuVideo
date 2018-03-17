using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
[RequireComponent (typeof (GameObject))]
public class zPerso : MonoBehaviour {

    public GameObject tilemapGO;

    private TilemapCollider2D tilemapCollider;
    private TilemapRenderer tilemapRenderer;

    private Collider2D persoCollider;
    private SpriteRenderer persoRenderer;
    
    private bool inCollider = false;

    void Start () {
        persoRenderer = GetComponent<SpriteRenderer>();
        persoCollider = GetComponent<Collider2D>();
        tilemapRenderer = tilemapGO.GetComponent<TilemapRenderer>();
        tilemapCollider = tilemapGO.GetComponent<TilemapCollider2D>();
    }

    void Update () {
       if(!inCollider && tilemapCollider.IsTouching(persoCollider)){
           inCollider = true;
           persoRenderer.sortingOrder = tilemapRenderer.sortingOrder - 1;
       }else if(inCollider && !tilemapCollider.IsTouching(persoCollider)){
           inCollider = false;
           persoRenderer.sortingOrder = tilemapRenderer.sortingOrder + 1;
       }
    }
}
