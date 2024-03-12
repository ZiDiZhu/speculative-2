using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BioMainMenu : MonoBehaviour
{
    public GameObject[] chapterBtn; 
    public GameObject[] chapterLevels;
    public GameObject LevelSelectPanel;

    private void Awake()
    {
        for (int i = 0; i < chapterBtn.Length; i++)
        {
            int index = i;
            chapterBtn[i].GetComponent<Button>().onClick.AddListener(() => SetChapterActive(index));
        }
    }

    void SetChapterActive(int index){
        foreach(Transform child in LevelSelectPanel.transform){
            child.gameObject.SetActive(false);
        }
        chapterLevels[index].SetActive(true);
        for(int i=0; i< chapterBtn.Length;i++){
            if(i == index){
                foreach(Transform child in chapterBtn[i].transform){
                    if(child.GetComponent<Image>() != null)
                        child.GetComponent<Image>().color = Color.white;
                }
            }else{
                foreach(Transform child in chapterBtn[i].transform){
                    if(child.GetComponent<Image>() != null)
                        child.GetComponent<Image>().color = Color.gray;
                }
            }
        }
    }
    
}
