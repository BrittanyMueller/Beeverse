using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoneyFactory : Honeycomb {

  public float HoneyPerSecond = 1f;

  void OnMouseDown() {
    // make sure UI isn't on UI
    if (EventSystem.current.IsPointerOverGameObject())
      return;
    if (!OpenBuildController()) {
      _hudController.OpenStructureMenu(StructureType.HoneyFactory, this);
    }
  }

  public override void SetWorker(WorkerBee bee, int index) {

    bee.Task = new WorkerBeeTask(WorkerBeeTask.TaskType.HoneyFactory,
                                 workSpots[index].position, index,
                                 structure.transform.position);

    // Check if parent wants to change the task
    base.SetWorker(bee, index);
  }
}
