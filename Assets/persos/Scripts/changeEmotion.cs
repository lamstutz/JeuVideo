using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class changeEmotion : MonoBehaviour
{
    public enum Emotion { Neutre, Triste, Joie, Confusion, Colere };

	public currentLevel level;

    public AnimationCourse neutre;
    public AnimationCourse triste;
    public AnimationCourse joie;
    public AnimationCourse confusion;
    public AnimationCourse colere;

    public UnityEvent actionOnClick;

    public Emotion emotionID;

    // Use this for initialization
    void Start()
    {
        Emotion robotEmotion;
        robotEmotion = Emotion.Neutre;
        
        // triste.GetComponent<GameObject>().SetActive(false);

        if (actionOnClick == null)
            actionOnClick = new UnityEvent();
    }

    public void OnMouseDown()
    {
        actionOnClick.Invoke();
    }

    public void changeEmotions(GameObject robot)
    {
        switch (emotionID)
        {
            case Emotion.Neutre:
                robot.GetComponent<AnimationCourse>().anims = neutre.anims;
                break;
            case Emotion.Triste:
                if(level.level >= 2){
                    robot.GetComponent<AnimationCourse>().anims = triste.anims;
                }
                break;
            case Emotion.Joie:
                if(level.level >= 3){
                    robot.GetComponent<AnimationCourse>().anims = joie.anims;
                }
                break;
            case Emotion.Confusion:
                if(level.level >= 4){
                    robot.GetComponent<AnimationCourse>().anims = confusion.anims;
                }
                break;
            case Emotion.Colere:
                if(level.level >= 5){
                    robot.GetComponent<AnimationCourse>().anims = colere.anims;
                }
                break;
        }
    }
}
