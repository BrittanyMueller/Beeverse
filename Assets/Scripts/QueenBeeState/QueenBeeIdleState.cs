
using UnityEngine;
using System.Collections;

public class QueenBeeIdleState : QueenBeeState {

  public override void Execute(QueenBee bee) {

    if (bee.IsDead) {
      bee.ChangeState(new QueenBeeDieState());
    } else if (bee.hasTask && !bee.atTask) {
      bee.ChangeState(new QueenBeeTravelState());
    } else if (bee.hasTask && bee.atTask) {
      bee.ChangeState(new QueenBeeLandingState());

    } else {
      bee.IdleState();
    }
  }
}