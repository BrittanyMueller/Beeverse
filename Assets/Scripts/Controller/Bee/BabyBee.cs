using UnityEngine;
using UnityEngine.EventSystems;

public class BabyBee : Bee {

  public bool isGrowing;
  public bool isQueen;

  // Flag to manage age of baby bee
  public bool isEgg;

  // Brood Nest the bee is located in
  public BroodNest broodNest;

  [SerializeField]
  private GameObject eggModel;
  [SerializeField]
  private GameObject larvaModel;

  protected override void Start() {
    base.Start();
    _lifeSpan = new TimeTracker(0, 0, 0);
    isEgg = true;
  }

  protected override void OnMouseDown() {
    if (isGrowing) {
      // Make sure UI isn't on UI or in main menu
      if (inMenu || EventSystem.current.IsPointerOverGameObject())
        return;
      profileController.Hide();
      babyProfileController.Show(this);
    } else {
      hudController.eggMenu.Show(this);
    }
  }

  public new int AgeInDays { get => _lifeSpan.day;
}

public override void UpdateTimeTick(int minutes) {
  if (!isGrowing)
    return;
  var beeCount = 0;
  foreach (var bee in broodNest.bees) {
    if (bee)
      beeCount += 1;
  }
  // Growth speed modifier for number of bees in nest
  _lifeSpan.AddMinutes((int)(minutes * beeCount / 2.0));
  if (_lifeSpan.day == 3 && isEgg) {
    // Egg grows into larva bee model
    eggModel.SetActive(false);
    larvaModel.SetActive(true);
    isEgg = false;
  }
}
}
