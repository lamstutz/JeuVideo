using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class winEmotion: MonoBehaviour
{
    public Sprite colereIcon;
    public Sprite joieIcon;
    public Sprite tristesseIcon;
    public Sprite confusionIcon;
    public currentLevel level;



    public SpriteRenderer colereIcon_trans;
    public SpriteRenderer joieIcon_trans;
    public SpriteRenderer tristesseIcon_trans;
    public SpriteRenderer confusionIcon_trans;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateEmotion(){
        if(level.level == 2){
            tristesseIcon_trans.sprite = tristesseIcon;
        }
        if(level.level == 3){
            joieIcon_trans.sprite = joieIcon;
        }
    }
}
