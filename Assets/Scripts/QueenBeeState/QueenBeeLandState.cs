using UnityEngine;
using System.Collections;

public class QueenBeeLandState : QueenBeeState {

  public override void Execute(QueenBee bee) {

    if (bee.IsDead) {
      bee.ChangeState(new QueenBeeDieState());
    } else if (bee.hasTask && bee.hasLanded) {
      bee.ChangeState(new QueenBeeLayEggState());
    } else {
      bee.LandState();
    }
  }
}