using UnityEngine;
using System.Collections;

public class WorkerBeeWorkingState : WorkerBeeState {

  public override void Execute(WorkerBee bee) {

    if (bee.IsDead) {
      bee.ChangeState(new WorkerBeeDieState());
    } else if (!bee.hasTask || bee.taskChanged) {
      bee.taskChanged = false;
      bee.ChangeState(new WorkerBeeTakeOffState());
    } else {
      bee.Work();
    }
  }
}