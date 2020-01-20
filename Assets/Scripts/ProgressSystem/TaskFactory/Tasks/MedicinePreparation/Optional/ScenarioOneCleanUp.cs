using System;
using System.Collections.Generic;

/// <summary>
/// In case syringes were dropped, this task is created to check if the player puts the dropped syringes to trash before finishing the game.
/// </summary>
public class ScenarioOneCleanUp : TaskBase {
    #region Fields
    private string description = "Siivoa lopuksi työtila.";
    private string hint = "Vie pelin aikana lattialle pudonneet esineet roskakoriin.";
    private List<GeneralItem> itemsToBeCleaned;
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for ScenarioOneCleanUp task.
    ///  Is removed when finished and requires previous task completion.
    ///  </summary>
    public ScenarioOneCleanUp() : base(TaskType.ScenarioOneCleanUp, true, true) {
        Subscribe();
        points = 1;
        itemsToBeCleaned = new List<GeneralItem>();
    }
    #endregion

    #region Event Subscriptions
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
    public override void Subscribe() {
        base.SubscribeEvent(ItemDroppedOnFloor, EventType.ItemDroppedOnFloor);
        base.SubscribeEvent(ItemLiftedOffFloor, EventType.ItemLiftedOffFloor);
        base.SubscribeEvent(ItemDroppedInTrash, EventType.ItemDroppedInTrash);
        base.SubscribeEvent(ItemDroppedInWrongTrash, EventType.ItemDroppedInWrongTrash);
    }

    private void ItemDroppedOnFloor(CallbackData data) {


        if (G.Instance.Progress.IsCurrentPackage(PackageName.EquipmentSelection)) {
            return;
        }
        foreach (Package p in G.Instance.Progress.packages) {
            if (p.name == PackageName.CleanUp && p.activeTasks.Count == 1) {
                p.AddNewTaskBeforeTask(this, p.activeTasks[0]);
                break;
            }
        }
        GeneralItem item = data.DataObject as GeneralItem;
        if (!itemsToBeCleaned.Contains(item)) {
            itemsToBeCleaned.Add(item);
        }

        string meh = "";
        foreach (GeneralItem it in itemsToBeCleaned) {
            meh += it.name + "; ";
        }
        Logger.Warning(meh);
    }

    private void ItemLiftedOffFloor(CallbackData data) {
        if (G.Instance.Progress.IsCurrentPackage(PackageName.EquipmentSelection)) {
            return;
        }
        GeneralItem item = data.DataObject as GeneralItem;
        if (!item.IsClean && !G.Instance.Progress.IsCurrentPackage(PackageName.CleanUp)) {
            UISystem.Instance.CreatePopup("Siivoa pudonneet työvälineet vasta lopuksi.", MsgType.Mistake);
            G.Instance.Audio.Play(AudioClipType.MistakeMessage);
        }
    }

    private void ItemDroppedInTrash(CallbackData data) {
        if (G.Instance.Progress.IsCurrentPackage(PackageName.EquipmentSelection)) {
            return;
        }
        GeneralItem item = data.DataObject as GeneralItem;
        if (!item.IsClean) {
            if (!G.Instance.Progress.IsCurrentPackage(PackageName.CleanUp)) {
                UISystem.Instance.CreatePopup(-1, "Esine laitettiin roskakoriin liian aikaisin.", MsgType.Mistake);
                G.Instance.Audio.Play(AudioClipType.MistakeMessage);
                G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.ScenarioOneCleanUp);
            }
            itemsToBeCleaned.Remove(item);
        }
        if (itemsToBeCleaned.Count == 0 && base.package != null) {
            base.package.MoveTaskToManager(this);
        }
    }

    private void ItemDroppedInWrongTrash(CallbackData data) {
        UISystem.Instance.CreatePopup(-1, "Esine laitettiin väärään roskakoriin", MsgType.Mistake);
        G.Instance.Audio.Play(AudioClipType.MistakeMessage);
        G.Instance.Progress.Calculator.SubtractBeforeTime(TaskType.ScenarioOneCleanUp);
    }
    #endregion

    #region Public Methods
    public override void FinishTask() {
        if (itemsToBeCleaned.Count != 0) {
            G.Instance.Progress.Calculator.Subtract(TaskType.ScenarioOneCleanUp);
        }
        base.FinishTask();
    }

    public override string GetDescription() {
        return description;
    }

    public override string GetHint() {
        return hint;
    }
    #endregion
}