
using UnityEngine;
using System.Collections;

public class QueenBeeLayEggState : QueenBeeState {

  public override void Execute(QueenBee bee) {

    bee.LayEggState();
    bee.ChangeState(new QueenBeeTakeOffState());
  }
}