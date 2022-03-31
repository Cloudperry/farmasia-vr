﻿using System;
using System.Linq;
using System.Collections.Generic;

public class ProgressManager {

    #region Fields
    private bool testMode;

    // Actual list of every task. No task is ever removed from this list
    private List<Task> trueAllTasksThatAreNeverRemoved;
    private Dictionary<TaskType, int> taskMaxPoints;

    // Oot actually all tasks
    private HashSet<Task> allTasks;
    public List<Package> packages;
    public Package CurrentPackage {
        get; private set;
    }
    public ScoreCalculator Calculator {
        get; private set;
    }


    #endregion

    #region Constructor
    /// <summary>
    /// Initiates ProgressManager fields.
    /// </summary>
    public ProgressManager(bool testMode) {
        this.testMode = testMode;
        allTasks = new HashSet<Task>();
        packages = new List<Package>();
    }

    public void ForceCloseTasks(Task calledTask) {

        Logger.Print("Total task count " + trueAllTasksThatAreNeverRemoved.Count.ToString());

        foreach (Task task in trueAllTasksThatAreNeverRemoved) {
            if (calledTask.TaskType == task.TaskType) {
                continue;
            }
            if (task.TaskType == TaskType.Finish || task.TaskType == TaskType.ScenarioOneCleanUp) {
                continue;
            }
            Logger.Print(string.Format(
                "max points: {0}, points: {1}",
                task.TaskType.ToString(),
                taskMaxPoints[task.TaskType].ToString()
            ));
            task.ForceClose(taskMaxPoints[task.TaskType] > 0);
        }
    }

    public void ForceCloseTask(TaskType type, bool killPoints = true) {
        foreach (Task task in trueAllTasksThatAreNeverRemoved) {
            if (task.TaskType == type && !task.Completed) {
                if (killPoints) {
                    task.ForceClose(taskMaxPoints[type] > 0);
                } else {
                    task.ForceClose(false);
                }

                return;
            }
        }
    }

    public void SetSceneType(SceneTypes scene) {


        switch (scene) {
            case SceneTypes.MainMenu:
                return;
            case SceneTypes.MedicinePreparation:
                /*Need Support for multiple Scenarios.*/
                AddTasks(scene);
                Calculator = new ScoreCalculator(trueAllTasksThatAreNeverRemoved);
                GenerateScenarioOne();
                break;
            case SceneTypes.MembraneFilteration:
                AddTasks(scene);
                Calculator = new ScoreCalculator(trueAllTasksThatAreNeverRemoved);
                GenerateScenarioTwo();
                break;
            case SceneTypes.Tutorial:
                return;
        }
        if (scene != SceneTypes.MainMenu) {

            CurrentPackage = packages.First();
            UpdateDescription();
            UpdateHint();
            CurrentPackage.StartTask();
        }
    }

    public void SetProgress(byte[] state) {
        ScoreCalculator c = DataSerializer.Deserializer<ScoreCalculator>(state);
        if (c != null) {
            Calculator = c;
        }
    }
    #endregion

    #region Initialization
    /// <summary>
    /// Used to generate every package. Package is defined with a list of tasks.
    /// </summary>
    private void GenerateScenarioOne() {
        TaskType[] selectTasks = {
            TaskType.SelectTools,
            TaskType.SelectMedicine,
            TaskType.CorrectItemsInThroughput
        };
        TaskType[] workSpaceTasks = {
            TaskType.CorrectItemsInLaminarCabinet,
            TaskType.MedicineToSyringe,
            TaskType.LuerlockAttach,
            TaskType.SyringeAttach,
            TaskType.CorrectAmountOfMedicineSelected,
            TaskType.ItemsToSterileBag
        };
        TaskType[] cleanUpTasks = {
            TaskType.ScenarioOneCleanUp,
            TaskType.Finish
        };
        Package equipmentSelection = CreatePackageWithList(PackageName.EquipmentSelection, new List<TaskType>(selectTasks));
        Package workSpace = CreatePackageWithList(PackageName.Workspace, new List<TaskType>(workSpaceTasks));
        Package cleanUp = CreatePackageWithList(PackageName.CleanUp, new List<TaskType>(cleanUpTasks));
        packages.Add(equipmentSelection);
        packages.Add(workSpace);
        packages.Add(cleanUp);
    }

    private void GenerateScenarioTwo() {
        TaskType[] selectTasks = {
            TaskType.CorrectItemsInThroughputMembrane
        };
        TaskType[] workSpaceTasks = {
            TaskType.CorrectItemsInLaminarCabinetMembrane,
            TaskType.WriteTextsToItems,
            TaskType.OpenAgarplates,
            TaskType.FillBottles,
            TaskType.AssemblePump,
            TaskType.WetFilter,
            TaskType.StartPump,
            TaskType.MedicineToFilter,
            TaskType.StartPumpAgain,
            TaskType.CutFilter,
            TaskType.FilterHalvesToBottles
        };
        Package equipmentSelection = CreatePackageWithList(PackageName.EquipmentSelection, new List<TaskType>(selectTasks));
        Package workSpace = CreatePackageWithList(PackageName.Workspace, new List<TaskType>(workSpaceTasks));
        packages.Add(equipmentSelection);
        packages.Add(workSpace);
    }

    #region Package Init Functions
    /// <summary>
    /// Creates a new package.
    /// </summary>
    /// <param name="name">Name of the new package.</param>
    /// <param name="tasks">List of predefined tasks.</param>
    /// <returns></returns>
    private Package CreatePackageWithList(PackageName name, List<TaskType> tasks) {
        Package package = new Package(name, this);
        TakeTasksToPackage(package, tasks);
        return package;
    }

    /// <summary>
    /// Takes tasks from ProgressManager to designated package.
    /// </summary>
    /// <param name="package">Package to move tasks to.</param>
    /// <param name="types">List of types to move.</param>
    private void TakeTasksToPackage(Package package, List<TaskType> types) {
        foreach (TaskType type in types) {
            MoveToPackage(package, type);
        }
    }
    #endregion
    #endregion

    #region Task Addition
    /// <summary>
    /// Creates a single task from every enum TaskType object.
    /// Adds tasks into currently activeTasks.
    /// </summary>
    public void AddTasks(SceneTypes scene) {
        allTasks = new HashSet<Task>(Enum.GetValues(typeof(TaskType))
            .Cast<TaskType>()
            .Select(v => TaskFactory.GetTask(v, scene))
            .Where(v => v != null)
            .ToList());

        trueAllTasksThatAreNeverRemoved = new List<Task>();
        taskMaxPoints = new Dictionary<TaskType, int>();
        foreach (Task task in allTasks) {
            trueAllTasksThatAreNeverRemoved.Add(task);
            taskMaxPoints.Add(task.TaskType, task.Points);
        }
    }

    public void AddTask(Task task) {
        if (!allTasks.Contains(task)) {
            allTasks.Add(task);
        }
    }
    #endregion

    #region Task Movement
    /// <summary>
    /// Moves task to package with given TaskType.
    /// </summary>
    /// <param name="package">Package to move task to.</param>
    /// <param name="taskType">Type of task to move.</param>
    public void MoveToPackage(Package package, TaskType taskType) {
        Task foundTask = FindTaskWithType(taskType);
        if (foundTask != null) {
            package.AddTask(foundTask);
            allTasks.Remove(foundTask);
        }
    }

    /// <summary>
    /// Finds task with given type.
    /// </summary>
    /// <param name="taskType">Type of task to find.</param>
    /// <returns></returns>
    public Task FindTaskWithType(TaskType taskType) {
        Task foundTask = null;
        foreach (Task task in allTasks) {
            if (task.TaskType == taskType) {
                foundTask = task;
                break;
            }
        }
        return foundTask;
    }
    #endregion


    public HashSet<Task> GetAllTasks() {
        return allTasks;
    }


    #region Finishing Packages and Manager

    public void ChangePackage() {
        int index = packages.IndexOf(CurrentPackage);
        if ((index + 1) >= packages.Count) {
            FinishProgress();
        } else {
            CurrentPackage = packages[index + 1];
            CurrentPackage.StartTask();
            UpdateHint();
        }
    }

    public void FinishProgress() {
        foreach (Task task in allTasks) {
            if (task.TaskType == TaskType.Finish) {
                RemoveTask(task);
                MedicinePreparationScene.SavedScoreState = null;
                (int score, string scoreString) = Calculator.GetScoreString();
                EndSummary.EnableEndSummary(scoreString);
                Player.SavePlayerData(score, scoreString);
                break;
            }
        }
    }

    /// <summary>
    /// Called when task is finished and set to remove itself.
    /// </summary>
    /// <param name="task">Reference to the task that will be removed.</param>
    public void RemoveTask(Task task) {
        if (allTasks.Contains(task)) {
            allTasks.Remove(task);
        }
    }
    #endregion

    #region Description Methods
    public void UpdateDescription() {
        if (!testMode) {
            if (CurrentPackage != null && CurrentPackage.CurrentTask != null) {
                UISystem.Instance.Descript = CurrentPackage.CurrentTask.Description;
#if UNITY_NONVRCOMPUTER
#else

                VRVibrationManager.Vibrate();
#endif
            } else {
                UISystem.Instance.Descript = "";
            }
        }
    }
    #endregion

    #region Hint Methods
    public void UpdateHint() {
        if (!testMode && CurrentPackage != null && CurrentPackage.CurrentTask != null) {
            HintBox.CreateHint(CurrentPackage.activeTasks[0].Hint);
        }
    }
    #endregion



    public bool IsCurrentPackage(PackageName name) {
        return CurrentPackage.name == name;
    }
}