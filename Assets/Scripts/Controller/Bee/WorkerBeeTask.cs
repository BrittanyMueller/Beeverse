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

  // the spot they currently hold in the int array
  public int workerSpotIndex;

  public WorkerBeeTask(TaskType task, Vector3 location, int index) {
    this.taskType = task;
    taskLocation = location;
    workerSpotIndex = index;
  }
}