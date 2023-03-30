using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintBoxNew : MonoBehaviour
{
    // Add TextMeshPro object in Unity editor
    [SerializeField]
    private TextMeshPro hintDesc;

    private Transform[] hintBoxObjects;

    // Start is called before the first frame update
    void Start()
    {
        // Get transforms of children objects
        hintBoxObjects = this.gameObject.GetComponentsInChildren<Transform>();
        // Hide text in the beginning
        hintDesc.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        RotateHintBox();
    }

/*
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Controller (Left)") || other.CompareTag("Controller (Right)"))
        {
            hintDesc.gameObject.SetActive(true);
            //hintDescription.SetActive(true);
        }
    }*/

    public void ShowText()
    {
        hintDesc.gameObject.SetActive(true);
    }

    public void HideText()
    {
        hintDesc.gameObject.SetActive(false);
    }

    /// <summary>
    /// Method to rotate the question mark and hintbox
    /// </summary>
    private void RotateHintBox() 
    {
        // Index 2 rotates the second child of HintBoxNew which is "Body", 
        // index 3 is the first child of "Body" AKA "HintBoxShape". 
        hintBoxObjects[2].Rotate(Vector3.up * 20 * Time.deltaTime);   
        hintBoxObjects[3].Rotate(Vector3.left * 20 * Time.deltaTime);
           
   
    }
}
