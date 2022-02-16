using UnityEngine;
using System;
public class WriteTextsToItems : TaskBase
{
    #region Constants
    private const string DESCRIPTION = "Kirjoita tarvittavat tiedot pulloihin ja maljoihin";
    private const string HINT = "Kosketa kyn�ll� esinett�, johon haluat kirjoittaa, valitse kirjoitettavat tekstit (max 4) klikkaamalla niit�. Voit perua kirjoituksen painamalla teksti� uudestaan ennen kuin painat vihre�� nappia";
    #endregion

    #region Fields

    /// <summary>
    /// Conditions must be met to render task complete
    /// </summary>
    public enum Conditions { SoycaseinePlatesHaveText, SabouradDextrosiPlateHasText, BottlesHaveText }
    private int writtenBottles = 0;
    private int writtenSoycaseinePlates = 0;
    private int writtenSabouradPlates = 0;
    private int numberOfObjectsThatShouldHaveText = 1;
    #endregion

    public WriteTextsToItems() : base(TaskType.WriteTextsToItems, true, false)
    {
        SetCheckAll(true);
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        points = 2;
    }

    #region Event Subscriptions
    public override void Subscribe()
    {
        base.SubscribeEvent(DoSomething, EventType.WriteToObject);
    }

    private void DoSomething(CallbackData data)
    {
        GameObject gobj = (GameObject)data.DataObject;
        ObjectType type = gobj.GetComponent<GeneralItem>().ObjectType;
        Logger.Print("Progress system object type: " + type);
        Writable text = gobj.GetComponent<Writable>();
        Logger.Print("Progress sytemi� varten tekstin l�ytyminen: " + text.Text);

        if (type == ObjectType.Bottle)
        {
            writtenBottles++;
            if (writtenBottles == 1)
            {
                EnableCondition(Conditions.BottlesHaveText);
                EnableCondition(Conditions.SoycaseinePlatesHaveText);
                EnableCondition(Conditions.SabouradDextrosiPlateHasText);
            }
        }
        if (type == ObjectType.SoycaseinePlate)
        {
            writtenSoycaseinePlates++;
        }
        if (type == ObjectType.SabouradDextrosiPlate)
        {
            writtenSabouradPlates++;
        }

        if (writtenBottles == numberOfObjectsThatShouldHaveText)
        {
            CompleteTask();
        }

    }
    #endregion
    protected override void OnTaskComplete()
    {
    }

    #region Public Methods

    public override void CompleteTask()
    {
        base.CompleteTask();
        if (IsCompleted())
        {
            Popup("Hyvin kirjoitettu.", MsgType.Done);
        }
    }

    public override string GetDescription()
    {
        return DESCRIPTION;
    }

    public override string GetHint()
    {
        return HINT;
    }
    #endregion
}