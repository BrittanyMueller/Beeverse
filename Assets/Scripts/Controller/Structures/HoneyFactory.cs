using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyFactory : Honeycomb
{

  void OnMouseDown() {
    if(!OpenBuildController()) {
        _hudController.OpenStructureMenu(StructureType.HoneyFactory, this);
    }
  }

  public override void SetWorker(WorkerBee bee, int index) {
    base.SetWorker(bee,index);
    
    // Check if parent gave them a task
    if(!bee.hasTask) {
        bee.Task = new WorkerBeeTask(WorkerBeeTask.TaskType.HoneyFactory,
                                    workSpots[index].position, index);
    }

  }

}
