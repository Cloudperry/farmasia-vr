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
    private int numberOfObjectsThatHaveText = 0;
    private int numberOfObjectsThatShouldHaveText = 8;
    #endregion

    public WriteTextsToItems() : base(TaskType.WriteTextsToItems, true, false)
    {
        Subscribe();

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
    }
    #endregion
    protected override void OnTaskComplete()
    {
        //////////////////////// TODO /////////////////////////
        throw new NotImplementedException();
    }

    #region Public Methods

    public override void FinishTask()
    {
        base.FinishTask();

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