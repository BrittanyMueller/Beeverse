using UnityEngine;
using System.Collections;

public class WorkerBeeTravelState : WorkerBeeState {

  public override void Execute(WorkerBee bee) {
    if (bee.IsDead) {
      bee.ChangeState(new WorkerBeeDieState());
    } else if (bee.HasTask && bee.AtTask) {
      bee.ChangeState(new WorkerBeeLandingState());
    } else if (!bee.HasTask) {
      bee.ChangeState(new WorkerBeeIdleState());

    } else {
      bee.TravelToTask();
    }
  }
}