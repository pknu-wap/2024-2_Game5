using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSetting : MonoBehaviour
{
    public bool isBossScene;
    public bool isStage1;
    public bool isStage2;
    public bool isStage3;


void Awake()
{
    PlayerPrefs.SetInt("isBossScene", System.Convert.ToInt16(isBossScene));
    PlayerPrefs.SetInt("isStage1", System.Convert.ToInt16(isStage1));
    PlayerPrefs.SetInt("isStage2", System.Convert.ToInt16(isStage2));
    PlayerPrefs.SetInt("isStage3", System.Convert.ToInt16(isStage3));
}

}


