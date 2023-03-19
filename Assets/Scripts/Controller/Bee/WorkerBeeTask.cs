using UnityEngine;
using System.Collections;

public class WorkerBeeTask {

  public enum TaskType {
    HoneyFactory,
    BeeswaxFactory,
    RoyalJellyFactory,
    Forager,
    Nurse,
    Builder
  }
  public TaskType taskType;
  public Vector3 taskLocation;

  public WorkerBeeTask(TaskType task, Vector3 location) {
    this.taskType = task;
    taskLocation = location;
  }
}