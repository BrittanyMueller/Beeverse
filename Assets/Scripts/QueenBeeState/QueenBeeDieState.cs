
using UnityEngine;
using System.Collections;

public class QueenBeeDieState : QueenBeeState {

  public override void Execute(QueenBee bee) { bee.DieState(); }
}