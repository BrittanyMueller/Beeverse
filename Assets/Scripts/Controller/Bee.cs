using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum BeeType { Queen, Worker }
public class Bee : MonoBehaviour {

  public String beeName;
  public BeeType type;

  // Used to keep track of how much life
  // the be has left should be set by child
  // in start
  protected TimeTracker _lifeSpan;
  public bool IsDead {
    get { return _lifeSpan.totalTime <= 0; }
  }
  public int lifeSpanInDays;

  // Status of the bee needed for gamestate
  public bool isDead;

  // todo add role

  // Start is called before the first frame update
  protected virtual void Start() {
    _lifeSpan = new TimeTracker(lifeSpanInDays, 0, 0);
  }

  // Update is called once per frame
  protected virtual void FixedUpdate() {}

  public void UpdateTimeTick(int minutes) { _lifeSpan.SubMinutes(minutes); }
}
