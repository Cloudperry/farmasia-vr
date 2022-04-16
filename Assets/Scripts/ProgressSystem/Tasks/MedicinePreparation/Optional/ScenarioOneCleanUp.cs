using System;
using System.Collections.Generic;

/// <summary>
/// In case syringes were dropped, this task is created to check if the player puts the dropped syringes to trash before finishing the game.
/// </summary>

//Responsibility from this class was removed, check out CleanupObject.cs which is attached to WorkspaceRoom in the scene
public class ScenarioOneCleanUp : Task {
    #region Fields
    private List<GeneralItem> itemsToBeCleaned;
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for ScenarioOneCleanUp task.
    ///  Is removed when finished and requires previous task completion.
    ///  </summary>
    public ScenarioOneCleanUp() : base(TaskType.ScenarioOneCleanUp, true) {
        Subscribe();
        itemsToBeCleaned = new List<GeneralItem>();
    }
    #endregion

    #region Public Methods
    public override void CompleteTask() {
        base.CompleteTask();
    }

    public override void FinishTask() {
        if (itemsToBeCleaned.Count != 0) {
            Logger.Error("Deprecated cleanup minus");
            //G.Instance.Progress.Calculator.Subtract(TaskType.ScenarioOneCleanUp);
        }
        base.FinishTask();
    }

    protected override void OnTaskComplete() {
    }
    #endregion
}