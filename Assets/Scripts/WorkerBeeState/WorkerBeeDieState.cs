using UnityEngine;
using System.Collections;

public class WorkerBeeDieState : WorkerBeeState {
  public override void Execute(WorkerBee bee) {
    bee.Die();
  }
}