using UnityEngine;

public class BeeEggSlot : MonoBehaviour {
  [SerializeField]
  private BroodNest broodNest;
  [SerializeField]
  private GameObject egg;

  public BabyBee babyBee;  // Baby Bee in this slot
  public int index;        // Index of the egg slot

  private bool _hasEgg;
  public bool HasEgg {
    get => _hasEgg;
    set {
      _hasEgg = value;
      if (!_hasEgg) {
        broodNest.SetWorkerPillow(index);
        return;
      }
      var eggGameObject = Instantiate(egg, transform.position, transform.rotation);
      babyBee = eggGameObject.GetComponent<BabyBee>();
      babyBee.eggSlotIndex = index;
      babyBee.broodNest = broodNest;
    }
  }
}
