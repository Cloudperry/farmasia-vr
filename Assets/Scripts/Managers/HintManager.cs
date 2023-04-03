using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FarmasiaVR.New;

public class HintManager : MonoBehaviour
{
        private List<TextMeshPro> hintDescriptions;


    private void Awake() 
    {
        hintDescriptions = new List<TextMeshPro>();
        foreach (GameObject descObject in GameObject.FindGameObjectsWithTag("HintDescription"))
        {
            hintDescriptions.Add(descObject.GetComponent<TextMeshPro>());
        }
    }


    /// <summary>
    /// Updates all hint texts in the scene. Set in Unity editor as a Task Manager Task Event.
    /// </summary>
    public void UpdateHintDescriptions(Task task)
    {
        foreach (TextMeshPro taskHint in hintDescriptions)
        {
            taskHint.text = task.hint;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
