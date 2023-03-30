using UnityEngine;
using System.Collections;

public class WorkerBeeTask {

  public enum TaskType {
    LayEgg,
    ReturnToNest,
  }
  public TaskType taskType;

  // Where they should sit
  public Vector3 taskLocation;

  public QueenBeeTask(TaskType task, Vector3 location) {
    this.taskType = task;
    taskLocation = location;
  }
}