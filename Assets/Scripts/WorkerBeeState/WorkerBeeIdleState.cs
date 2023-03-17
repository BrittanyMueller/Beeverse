
using UnityEngine;
using System.Collections;

public class WorkerBeeIdleState : WorkerBeeState {

  public override void Execute(WorkerBee bee) {

    if (bee.IsDead) {
      bee.ChangeState(new WorkerBeeDieState());
    } else if (bee.hasTask && !bee.atTask) {
      bee.ChangeState(new WorkerBeeTravelState());
    } else if (bee.hasTask && bee.atTask) {
      bee.ChangeState(new WorkerBeeLandingState());

    } else {
      bee.Idle();
    }
  }
}