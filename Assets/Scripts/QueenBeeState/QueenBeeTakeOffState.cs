using UnityEngine;
using System.Collections;

public class QueenBeeTakeOffState : QueenBeeState {

  public override void Execute(QueenBee bee) {

    if (bee.IsDead) {
      bee.ChangeState(new QueenBeeDieState());
    } else if (bee.HasTakenOff) {
      if (bee.IsLeavingHive) {
        bee.SetLeaveHiveTask();
      } else if (bee.HasEgg) {
        bee.SetEggTask();
      } else {
        bee.SetHomeTask();
      }
      bee.ChangeState(new QueenBeeTravelState());
    } else {
      bee.TakeOffState();
    }
  }
}