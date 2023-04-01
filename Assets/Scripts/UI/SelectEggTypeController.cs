using UnityEngine;
using UnityEngine.UI;

public class SelectEggTypeController : MonoBehaviour {
  public GameState state;

  [SerializeField]
  private Button setWorkerEgg;
  [SerializeField]
  private Button setQueenEgg;
  private BabyBee _currentEgg;

  private void Awake() {
    gameObject.SetActive(false);
  }

  private void Update() {
    setWorkerEgg.interactable = state.RoyalJellyCount >= 10 && state.HoneyCount >= 40;
    setQueenEgg.interactable = !state.HasQueenEgg && state.RoyalJellyCount >= 50;
  }

  public void Hide() {
    gameObject.SetActive(false);
  }

  public void Show(BabyBee egg) {
    gameObject.SetActive(true);
    _currentEgg = egg;
  }

  public void SetWorkerEgg() {
    _currentEgg.isGrowing = true;
    _currentEgg.isQueen = false;
    var cost = new BeeResources { Honey = 40, RoyalJelly = 10 };
    state.ConsumeResources(cost);
    // Update pillow colour to green worker pillow type
    _currentEgg.broodNest.SetWorkerPillow(_currentEgg.eggSlotIndex);
    _currentEgg.beeName = BeeStuff.GetRandomName();
    Hide();
  }

  public void SetQueenEgg() {
    _currentEgg.isGrowing = true;
    _currentEgg.isQueen = true;
    state.HasQueenEgg = true;
    var cost = new BeeResources { RoyalJelly = 50 };
    state.ConsumeResources(cost);
    // Update to red queen pillow type
    _currentEgg.broodNest.SetQueenPillow(_currentEgg.eggSlotIndex);
    _currentEgg.beeName = BeeStuff.GetRandomQueenName();
    Hide();
  }
}
