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

  public static TaskType StructureTypeAsTaskType(StructureType type) {
    switch (type) {
    case StructureType.HoneyFactory:
      return TaskType.HoneyFactory;
    case StructureType.RoyalJellyFactory:
      return TaskType.RoyalJellyFactory;
    case StructureType.BeeswaxFactory:
      return TaskType.BeeswaxFactory;
    case StructureType.BroodNest:
      return TaskType.Nurse;
    case StructureType.Flower:
      return TaskType.Forager;
    }
    return TaskType.Builder;
  }
}