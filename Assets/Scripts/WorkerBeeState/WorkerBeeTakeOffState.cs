
using UnityEngine;
using System.Collections;

public class WorkerBeeTakeOffState : WorkerBeeState {

  public override void Execute(WorkerBee bee) {

    if (bee.IsDead) {
      bee.ChangeState(new WorkerBeeDieState());
    } else if (bee.HasTakenOff && bee.HasTask) {
      bee.ChangeState(new WorkerBeeTravelState());
    } else if (bee.HasTakenOff && !bee.HasTask) {
      bee.ChangeState(new WorkerBeeIdleState());
    } else {
      bee.TakeOff();
    }
  }
}