using UnityEngine;
using UnityEngine.EventSystems;

public class BabyBee : Bee {
  public bool isGrowing;  // Indicates if BabyBee has started growing
  public bool isQueen;    // Indicates if BabyBee is type Queen or Worker
  public bool isEgg;      // BabyBees will be an egg until 3 days old

  public BroodNest broodNest;  // Brood Nest the bee is located in
  public int eggSlotIndex;
  [SerializeField]
  private GameObject eggModel;
  [SerializeField]
  private GameObject larvaModel;

  protected override void Start() {
    base.Start();
    LifeSpan = new TimeTracker(0, 0, 0);
    isEgg = true;
  }

  protected override void OnMouseDown() {
    if (isGrowing) {
      // Make sure UI isn't on UI or in main menu
      if (inMenu || EventSystem.current.IsPointerOverGameObject())
        return;
      ProfileController.Hide();
      BabyProfileController.Show(this);
    } else {
      HUDController.eggMenu.Show(this);
    }
  }

  // Override AgeInDays of BabyBees to be from lifespan
  public new int AgeInDays => LifeSpan.Day;
  public int AgeInMinutes => LifeSpan.TotalTime;

  public override void UpdateTimeTick(int minutes) {
    if (!isGrowing)
      return;
    var beeCount = 0;
    foreach (var bee in broodNest.bees) {
      if (bee)
        beeCount += 1;
    }
    // Growth speed modifier for number of bees in nest
    LifeSpan.AddMinutes((int)(minutes * beeCount / 2.0));
    if (LifeSpan.Day != 3 || !isEgg)
      return;
    // Change egg into larva bee model when old enough
    eggModel.SetActive(false);
    larvaModel.SetActive(true);
    isEgg = false;
  }
}
