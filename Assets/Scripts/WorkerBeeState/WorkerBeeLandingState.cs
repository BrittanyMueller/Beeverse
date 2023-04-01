
using UnityEngine;
using System.Collections;

public class WorkerBeeLandingState : WorkerBeeState {

  public override void Execute(WorkerBee bee) {

    if (bee.IsDead) {
      bee.ChangeState(new WorkerBeeDieState());
    } else if (bee.HasTask && bee.HasLanded) {
      bee.ChangeState(new WorkerBeeWorkingState());
    } else if (!bee.HasTask) {
      bee.ChangeState(new WorkerBeeTakeOffState());
    } else {
      bee.LandAtTask();
    }
  }
}