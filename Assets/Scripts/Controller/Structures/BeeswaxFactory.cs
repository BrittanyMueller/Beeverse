using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeeswaxFactory : Honeycomb {

  public float BeeswaxFactoryPerSecond = 1f;

  void OnMouseDown() {
    // make sure UI isn't on UI
    if (EventSystem.current.IsPointerOverGameObject())
      return;
    if (!OpenBuildController()) {
      _hudController.OpenStructureMenu(StructureType.BeeswaxFactory, this);
    }
  }

  public override void SetWorker(WorkerBee bee, int index) {

    bee.Task = new WorkerBeeTask(WorkerBeeTask.TaskType.BeeswaxFactory,
                                 workSpots[index].position, index,
                                 structure.transform.position);

    // Check if parent wants to change the task
    base.SetWorker(bee, index);
  }
}
