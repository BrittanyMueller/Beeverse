using UnityEngine;
using System.Collections;

public class QueenBeeTask {
  public enum TaskType { LayEgg, ReturnToNest, LeaveHive }
  public TaskType taskType;

  // Where they should sit
  public Vector3 taskLocation;

  public BeeEggSlot _curSlot;

  public QueenBeeTask(TaskType task, Vector3 location) {
    taskType = task;
    taskLocation = location;
  }

  public QueenBeeTask(TaskType task, Vector3 location, BeeEggSlot slot) {
    taskType = task;
    taskLocation = location;
    _curSlot = slot;
  }
}