
using UnityEngine;
using System.Collections;

public class WorkerBeeLandingState : WorkerBeeState {

  public override void Execute(WorkerBee bee) {

    if (bee.IsDead) {
      bee.ChangeState(new WorkerBeeDieState());
    } else if (bee.hasTask && bee.hasLanded) {
      bee.ChangeState(new WorkerBeeWorkingState());
    } else if (!bee.hasTask) {
      bee.ChangeState(new WorkerBeeIdleState());
    } else {
      bee.LandAtTask();
    }
  }
}