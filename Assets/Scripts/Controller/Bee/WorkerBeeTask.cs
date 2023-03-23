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

  // Where they should sit
  public Vector3 taskLocation;

  // Where they should look
  public Vector3 targetLocation;

  // the spot they currently hold in the int array
  public int workerSpotIndex;

  public WorkerBeeTask(TaskType task, Vector3 location, int index) {
    this.taskType = task;
    taskLocation = location;
    targetLocation = new Vector3(0, 0);
    workerSpotIndex = index;
  }

  public WorkerBeeTask(TaskType task, Vector3 location, int index,
                       Vector3 target) {
    this.taskType = task;
    taskLocation = location;
    targetLocation = target;
    workerSpotIndex = index;
  }

  public TaskType HoneycombTypeAsTaskType(Honeycomb.HoneycombType type) {
    switch (type) {
    case Honeycomb.HoneycombType.HoneyFactory:
      return TaskType.HoneyFactory;
    case Honeycomb.HoneycombType.RoyalJellyFactory:
      return TaskType.RoyalJellyFactory;
    case Honeycomb.HoneycombType.BeeswaxFactory:
      return TaskType.BeeswaxFactory;
    case Honeycomb.HoneycombType.BroodNest:
      return TaskType.Nurse;
    }
    return TaskType.Builder;
  }
}