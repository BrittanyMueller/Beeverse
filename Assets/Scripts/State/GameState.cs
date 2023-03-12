using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public struct BeeResources {
  public int honey;
  public int beeswax;
  public int royalJelly;
  public int nectar;
  public int pollen;
}

public struct TimeTracker {
  public int day;
  public int hour;
  public int minute;

  public TimeTracker(int d, int h, int m) {
    day = d;
    hour = h;
    minute = m;
  }

  public void AddMinutes(int delta) {
    if (minute + delta > 60) {
      minute = (minute + delta) % 60;

      if (hour == 23) {
        day++;
        hour = 0;
      } else {
        hour++;
      }
    } else {
      minute += delta;
    }
  }

  public override string ToString() {
    return "Day: " + day.ToString() + " " + hour.ToString().PadLeft(2, '0') +
           ":" + minute.ToString().PadLeft(2, '0');
    ;
  }
}

public class GameState : MonoBehaviour {

  private TimeTracker _currentTime;
  public TimeTracker CurrentTime {
    get { return _currentTime; }
  }

  private List<Bee> _bees = new List<Bee>();
  private QueenBee _queen;

  // Controllers for GUIs
  public LogController logController;
  public ResourceController resourceController;
  public DayController dayController;

  private BeeResources resources;

  public int Day {
    get { return _day; }
  }
  private int _day;

  // Game config settings
  public int minutesPreSecond = 5;

  // This timer will be used to
  // update the clock every second
  private double _secondTimer;

  public bool Paused {
    get { return _paused; }
    set {

      _paused = value;
      NotifyPausedChanged();
    }
  }
  private bool _paused;

  // Start is called before the first frame update
  void Start() {
    _paused = false;

    _secondTimer = 1;

    // Might change when saving comes in
    _currentTime = new TimeTracker(1, 0, 0);
    _day = 0;

    resources = new BeeResources();
    resources.beeswax = 123;
    resources.honey = 321;
    resources.nectar = 500;
    resources.pollen = 500;
    resources.royalJelly = 5;
    resourceController.UpdateResources(resources);

    // Create the queen bee
  }

  // Update is called once per frame
  void FixedUpdate() {
    _secondTimer -= Time.deltaTime;
    if (_secondTimer <= 0) {
      _secondTimer = 1;
      _currentTime.AddMinutes(minutesPreSecond);
      // Update game UI with new time
      dayController.UpdateDate(_currentTime.ToString());
    }

    if (_day != _currentTime.day) {
      _day += 1;
      UpdateLog("Day " + _day.ToString());
      UpdateLog("> I heard you like jazz");
    }

    if (_bees.Count == 0) {
      // Big up lose game menu
    }
  }

  /****************************
   * Functions to control the game state
   *****************************/

  // Tell everyone who cares the game is no longer paused
  private void NotifyPausedChanged() {}

  public void UpdateLog(String update) { logController.UpdateLog(update); }

  public void Save() { Debug.Log("Your game is saved.. jk not impl yet"); }
}
