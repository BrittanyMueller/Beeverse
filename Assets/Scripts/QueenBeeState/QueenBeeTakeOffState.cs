using UnityEngine;
using System.Collections;

public class QueenBeeTakeOffState : QueenBeeState {

  public override void Execute(QueenBee bee) {

    if (bee.IsDead) {
      bee.ChangeState(new QueenBeeDieState());
    } else if (bee.hasTakenOff) {
      if (bee.hasEgg) {
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