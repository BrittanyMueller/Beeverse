using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeEggSlot : MonoBehaviour {

  public BroodNest nest;
  public GameObject egg;
  public bool hasEgg {
    get { return _hasEgg; }
    set {
      _hasEgg = value;
      if (_hasEgg) {
        GameObject eggGameObject =
            Instantiate(egg, transform.position, transform.rotation);
        // TODO add egg to brood nest @b
      }
    }
  }
  private bool _hasEgg = false;

  public TimeTracker timeToHatch;
}
