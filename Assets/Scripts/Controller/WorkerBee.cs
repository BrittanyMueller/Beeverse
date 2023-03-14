using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerBee : Bee {
  // Start is called before the first frame update
  protected override void Start() {
    base.Start();
    Debug.Log(_lifeSpan.day);
  }

  // Update is called once per frame
  protected void Update() { Debug.Log(_lifeSpan.day); }
}
