using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisherTutorialSceneManager : MonoBehaviour
{
    private TaskManager taskManager;
    public FireGrid fire;

    private void Awake()
    {
        taskManager = GetComponent<TaskManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PressQ();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            PressW();
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            PressE();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            PressR();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            PressT();
        }
    }

    public void PickUp()
    {
        taskManager.CompleteTask("Pickup");
    }

    public void SafetyPin()
    {
        if (!taskManager.IsTaskCompleted("Pickup"))
        {
            taskManager.GenerateTaskMistake("Tartu ensin kiinni sammuttimesta toisella k�dell�", 0);
                return;
        }
        taskManager.CompleteTask("SafetyPin");

    }

    public void Hose()
    {
        if (!taskManager.IsTaskCompleted("SafetyPin"))
        {
            taskManager.GenerateTaskMistake("Irroita ensin sokka", 0);
            return;
        }
        taskManager.CompleteTask("Hose");
    }

    public void Activate()
    {
        if (!taskManager.IsTaskCompleted("Hose"))
        {
            taskManager.GenerateTaskMistake("Tartu ensin kiinni sammuttimen letkun p��st�", 0);
            return;
        }
        taskManager.CompleteTask("Activate");

        fire.gameObject.SetActive(true);
    }


    public void Extinguish()
    {
        taskManager.CompleteTask("Extinguish");
    }
    public void PressQ()
    {
        taskManager.CompleteTask("Q");
    }

    public void PressW()
    {
        taskManager.CompleteTask("W");
    }

    public void PressE()
    {
        if(!taskManager.IsTaskCompleted("Q") | !taskManager.IsTaskCompleted("W"))
        {
            taskManager.GenerateTaskMistake("You have to press Q and W before pressing E", 5);
            return;
        }

        taskManager.CompleteTask("E");
    }

    public void PressR()
    {
        ///If pressing R isn't the current task, generate a general mistake
        if (taskManager.GetCurrentTask().key != "R")
        {
            taskManager.GenerateGeneralMistake("Make sure to press Q, W and E before pressing R", 2);
            return;
        }
        taskManager.CompleteTask("R");
    }

    public void PressT()
    {
        if (!taskManager.IsTaskCompleted("R"))
        {
            taskManager.GenerateGeneralMistake("Make sure to press Q, W, E and R before pressing T", 3);
        }
        taskManager.CompleteTask("T");
    }

}
