using UnityEngine;
using System.Collections;

public class WorkerBeeWorkingState : WorkerBeeState {
  public override void Execute(WorkerBee bee) {
    if (bee.IsDead) {
      bee.ChangeState(new WorkerBeeDieState());
    } else if (!bee.HasTask || bee.taskChanged) {
      bee.taskChanged = false;
      bee.ChangeState(new WorkerBeeTakeOffState());
      // They stopped working so reset their current
      // job progress note this means the resources
      // will be lossed
      // todo the progress should be handled by the work
      // position not the bee
      bee.ResetWorkTimer();
    } else {
      bee.Work();
    }
  }
}