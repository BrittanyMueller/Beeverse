using System;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Currently we support 2 types of bees
 * the queen bee and the workers
 * The worker bees are responsible foraging,
 * build, working in factories, and taking care
 * of baby bees. They will also have a much shorter lifespan than the queen
 *
 * The Queen is responsible for laying eggs in the brood nest.
 */
public enum BeeType { Queen, Worker, Baby }

/**
 * The Bee class is the main controller of the NPC holding any common
 * functionality between the queen and worker bee.
 */
public class Bee : MonoBehaviour {

  /** Name of the bee that will be shown in the UI*/
  public string beeName;
  public BeeType type;
  // If we are in the menu our idle animation
  // should be different.
  public bool inMenu = false;

  protected HudController HUDController;
  protected BeeProfileController ProfileController;
  protected BabyProfileController BabyProfileController;

  // Used to keep track of how much life
  // the be has left should be set by child
  // in start
  protected TimeTracker LifeSpan;
  public bool IsDead => LifeSpan.TotalTime <= 0;

  // Returns how old a given bee is in days
  public int AgeInDays => lifeSpanInDays - LifeSpan.Day + startAge;
  public int lifeSpanInDays;

  // How old the bee was when it gets instantiated from a baby
  public int startAge = 5;

  // Indicates to the game state if the bee has current died
  public bool isDead;

  // Start is called before the first frame update
  protected virtual void Start() {

    // Make their lifespan random by a 5 day deviation
    System.Random rand = new System.Random();
    LifeSpan = new TimeTracker(lifeSpanInDays, 0, 0);
    LifeSpan.AddMinutes(rand.Next((5 * 60 * 25) * 2) - 7200);
    lifeSpanInDays = LifeSpan.Day;
    HUDController =
        GameObject.Find("/HudController").GetComponent<HudController>();
    ProfileController = HUDController.beeProfileController;
    BabyProfileController = HUDController.babyProfileController;
  }

  /**
   * Internal game clock which is triggered based on the speed of the game
   * This can be overwritten by a child class but the base class still must
   * be called in order to maintain the bees lifespan and dead time.
   */
  public virtual void UpdateTimeTick(int minutes) {
    LifeSpan.SubMinutes(minutes);
  }

  /**
   * Opens profile window for bee on click
   */
  protected virtual void OnMouseDown() {
    // Make sure UI isn't on UI or in main menu
    if (inMenu || EventSystem.current.IsPointerOverGameObject())
      return;
    BabyProfileController.Hide();
    ProfileController.Show(this);
  }
}
