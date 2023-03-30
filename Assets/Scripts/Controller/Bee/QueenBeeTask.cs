using UnityEngine;
using System.Collections;

public class QueenBeeTask {

  public enum TaskType {
    LayEgg,
    ReturnToNest,
  }
  public TaskType taskType;

  // Where they should sit
  public Vector3 taskLocation;

  public BeeEggSlot _curSlot = null;

  public QueenBeeTask(TaskType task, Vector3 location) {
    this.taskType = task;
    taskLocation = location;
  }

  public QueenBeeTask(TaskType task, Vector3 location, BeeEggSlot slot) {
    this.taskType = task;
    taskLocation = location;
    _curSlot = slot;
  }
}