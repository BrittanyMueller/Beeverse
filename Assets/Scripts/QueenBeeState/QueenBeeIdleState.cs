
using UnityEngine;
using System.Collections;

public class QueenBeeIdleState : QueenBeeState {
  public override void Execute(QueenBee bee) {
    if (bee.IsDead) {
      bee.ChangeState(new QueenBeeDieState());
    } else if (bee.HasTask || bee.HasEgg) {
      bee.ChangeState(new QueenBeeTakeOffState());
    } else {
      bee.IdleState();
    }
  }
}