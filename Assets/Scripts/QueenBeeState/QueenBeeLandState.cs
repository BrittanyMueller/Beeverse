using UnityEngine;
using System.Collections;

public class QueenBeeLandState : QueenBeeState {
  public override void Execute(QueenBee bee) {
    if (bee.IsDead) {
      bee.ChangeState(new QueenBeeDieState());
    } else if (bee.HasTask && bee.HasLanded) {
      if (bee.Task.taskType == QueenBeeTask.TaskType.LayEgg) {
        bee.ChangeState(new QueenBeeLayEggState());
      } else {
        bee.ChangeState(new QueenBeeIdleState());
        bee.Task = null;
      }
    } else {
      bee.LandState();
    }
  }
}