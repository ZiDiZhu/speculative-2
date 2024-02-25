using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Test script to display the modular body parts.
//Maybe look into how to do 2D rigging
public class Body : MonoBehaviour
{

    //todo: create a body part class
    [SerializeField]private List<Sprite> torsos = new List<Sprite>(); //TODO: Load  the content from a resource folder
    [SerializeField]private List<Sprite> left_arms = new List<Sprite>();
    [SerializeField]private List<Sprite> right_arms = new List<Sprite>();
    [SerializeField]private List<Sprite> left_legs = new List<Sprite>();
    [SerializeField]private List<Sprite> right_legs = new List<Sprite>();
    [SerializeField]private List<Sprite> tops = new List<Sprite>();
    [SerializeField]private List<Sprite> bottoms = new List<Sprite>();


    [SerializeField]private Image torso, left_arm, right_arm,left_leg,right_leg,top,bottom;

    [ContextMenu("Generate")]
    public void GenerateBody(){
        SetRandomPart(torso, torsos);
        SetRandomPart(left_arm,left_arms);
        SetRandomPart(right_arm, right_arms);
        SetRandomPart(left_leg,left_legs);
        SetRandomPart(right_leg, right_legs);
        SetRandomPart(top, tops);
        SetRandomPart(bottom, bottoms);
    }

    void SetRandomPart(Image image, List<Sprite>sprites){
        image.sprite = sprites[Random.Range(0,sprites.Count)];
    }


}
