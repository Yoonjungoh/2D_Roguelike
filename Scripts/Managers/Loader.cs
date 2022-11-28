using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;
    void Awake()
    {
        if(GameManager.instance == null)
        {
            //Instantiate(gameManager, new Vector3(0f, 0f, 0f), Quaternion.identity);
            ResourceManager.Instantiate(gameManager);
        }
    }
}
