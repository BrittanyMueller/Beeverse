using UnityEngine;
using System.Collections;

public class QueenBeeTravelState : QueenBeeState {

  public override void Execute(QueenBee bee) {

    if (bee.IsDead) {
      bee.ChangeState(new QueenBeeDieState());
    } else if (bee.atTask) {
      bee.ChangeState(new QueenBeeLandState());
    } else {
      bee.TravelState();
    }
  }
}