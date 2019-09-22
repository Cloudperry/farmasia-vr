using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmountOfItems : TaskBase {
    #region Fields
    private string[] conditions = {"Syringe", "Needle", "Luerlock", "RightSizeBottle"};
    #endregion

    #region Constructor
    public AmountOfItems() : base(TaskType.AmountOfItems, true, false) {
        Subscribe();
        AddConditions(conditions);
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() { 
        base.SubscribeEvent(Amount, EventType.AmountOfItems);
    }
    private void Amount(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        if (item == null) {
            Logger.Print("was null");
            return;
        }
        ObjectType type = item.ObjectType;
        
        if (type == ObjectType.Syringe) {
            EnableCondition("Syringe");
        }
        if (type == ObjectType.Needle) {
            EnableCondition("Needle");
        }
        if (type == ObjectType.Luerlock) {
            EnableCondition("Luerlock");
        }
        if (type == ObjectType.Bottle) {
            //check that the chosen bottle is the wanted size
            EnableCondition("RightSizeBottle");
        }
        bool check = CheckClearConditions(true);
        //checked when player exits the room
        if (!check) {
            Logger.Print("All conditions not fulfilled but task closed.");
            G.Instance.ProgressManager.Calculator.Subtract(TaskType.AmountOfItems);
            base.UnsubscribeAllEvents();
        }
    }
    #endregion

    #region Public Methods
    public override void FinishTask() {
        Logger.Print("All conditions fulfilled, task finished!");
        G.Instance.ProgressManager.Calculator.Add(TaskType.AmountOfItems);
        base.FinishTask();
    }
    public override string GetDescription() {
        return "Tarkista valitsemiesi välineiden määrä.";
    }
    public override string GetHint() {
        return "Tarkista välineitä kaappiin viedessäsi, että olet valinnut oikean määrän välineitä ensimmäisellä hakukerralla."; 
    }
    #endregion
}