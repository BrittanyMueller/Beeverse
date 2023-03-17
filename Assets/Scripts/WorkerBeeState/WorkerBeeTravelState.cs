using UnityEngine;
using System.Collections;

public class WorkerBeeTravelState : WorkerBeeState {

  public override void Execute(WorkerBee bee) {
    if (bee.IsDead) {
      bee.ChangeState(new WorkerBeeDieState());
    } else if (bee.hasTask && bee.atTask) {
      bee.ChangeState(new WorkerBeeLandingState());

    } else {
      bee.TravelToTask();
    }
  }
}