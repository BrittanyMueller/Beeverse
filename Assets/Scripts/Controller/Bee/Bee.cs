using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/**
 * Currently we support 2 types of bees
 * the queen bee and the workers
 * The worker bees are responsible foraging,
 * build, working in factories, and taking care
 * of baby bees. They will also have a much shorter lifespan than the queen
 *
 * The Queen responsibility is to lay eggs, which will be done in the broods
 * nest.
 */
public enum BeeType { Queen, Worker }

/**
 * The Bee class is the main controller of the NPC holding any common
 * functionality between the queen and worker bee.
 */
public class Bee : MonoBehaviour {

  /** Name of the bee that will be shown in the UI*/
  public String beeName;
  public BeeType type;

  // Used to keep track of how much life
  // the be has left should be set by child
  // in start
  protected TimeTracker _lifeSpan;
  public bool IsDead {
    get { return _lifeSpan.totalTime <= 0; }
  }

  // Returns how old a given bee is.
  public int AgeInDays {
    get { return lifeSpanInDays - _lifeSpan.day; }
  }
  public int lifeSpanInDays;

  // Indicates to the game state if the bee has current died
  public bool isDead;

  // Start is called before the first frame update
  protected virtual void Start() {
    _lifeSpan = new TimeTracker(lifeSpanInDays, 0, 0);
  }

  /**
   * Internal game clock which is triggered based on the speed of the game
   * This can be overwritten by a child class but the base class still must
   * be called in order to maintain the bees lifespan and dead time.
   */
  public virtual void UpdateTimeTick(int minutes) {
    _lifeSpan.SubMinutes(minutes);
  }
}
