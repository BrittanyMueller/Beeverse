using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeEggSlot : MonoBehaviour {
  
  [SerializeField] private BroodNest broodNest;
  [SerializeField] private GameObject egg;

  public BabyBee babyBee;
  
  private bool _hasEgg;
  public bool HasEgg {
    get { return _hasEgg; }
    set {
      _hasEgg = value;
      if (_hasEgg) {
        var eggGameObject = Instantiate(egg, transform.position, transform.rotation);
        babyBee = eggGameObject.GetComponent<BabyBee>();
        babyBee.broodNest = broodNest;
      }
    }
  }
}
