using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour {

  private TimeTracker _currentTime;
  public TimeTracker CurrentTime {
    get { return _currentTime; }
  }

  public List<Bee> _bees = new List<Bee>();
  private List<Bee> _deadBees = new List<Bee>();
  private QueenBee _queen;

  public List<Honeycomb> _honeycombs = new List<Honeycomb>();

  // Controllers for GUIs
  public LogController logController;
  public ResourceController resourceController;
  public HudController hudController;
  public GameOverController gameOverController;
  public DayController dayController;
  public DebugController debugController;

  private BeeResources resources;

  public int Day {
    get { return _day; }
  }
  private int _day;

  // Game config settings
  public static int minutesPreSecond = 5;

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

  // stats for gameover screen
  private int _totalBees;
  private int _totalQueens;
  private BeeResources _totalResources;

  // Start is called before the first frame update
  void Start() {
    _paused = false;

    _secondTimer = 1;

    // Might change when saving comes in
    _currentTime = new TimeTracker(1, 0, 0);
    _day = 0;

    // Set default resources count
    // todo tweak
    resources = new BeeResources();
    resources.beeswax = 123;
    resources.honey = 321;
    resources.nectar = 500;
    resources.pollen = 500;
    resources.royalJelly = 5;
    resourceController.UpdateResources(resources);

    // set init totals
    _totalResources = resources;
    _totalBees = _bees.Count;
    _totalQueens = 1;
  }

  // Update is called once per frame
  void FixedUpdate() {
    _secondTimer -= Time.deltaTime;
    if (_secondTimer <= 0) {
      _secondTimer = 1;
      _currentTime.AddMinutes(minutesPreSecond);
      // Update game UI with new time
      dayController.UpdateDate(_currentTime.ToString());

      // Update all bees that time has passed
      foreach (Bee bee in _bees) {
        bee.UpdateTimeTick(minutesPreSecond);
      }
    }

    // Notify the logs a day has passed
    if (_day != _currentTime.day) {
      _day += 1;
      UpdateLog("Day " + _day.ToString());
      UpdateLog("> I heard you like jazz");
    }

    // Remove all bees that have died
    foreach (Bee bee in _bees) {
      if (bee.isDead) {
        StartCoroutine(RemoveBee(bee));
        UpdateLog(">" + bee.beeName + " has died. rest in Bees");
      }
    }
    _bees.RemoveAll((Bee bee)  => {
      _deadBees.Add(bee);
      return bee.isDead;
    });


    if (Input.GetKeyDown(KeyCode.F1)) {
      debugController.toggle();
    }
  }

  IEnumerator RemoveBee(Bee bee) {
    yield return new WaitForSeconds(10);
    Destroy(bee.gameObject);

    _deadBees.Remove(bee);
    // Bring up Lose Menu
    if (_bees.Count == 0 && _deadBees.Count == 0) {
      gameOverController.SetTotalBees(_totalBees);
      gameOverController.SetTotalQueen(_totalQueens);
      gameOverController.SetTotalResources(_totalResources);
      gameOverController.SetTotalHoneycombs(_honeycombs.Count);
      gameOverController.SetTotalDays(_currentTime.day);
      Paused = true;
      hudController.GameOver();
    }
  }

  /****************************
   * Functions to control the game state
   *****************************/

  // Tell everyone who cares the game is no longer paused
  private void NotifyPausedChanged() {

    if (_paused) {
      Time.timeScale = 0;
    } else {
      Time.timeScale = 1;
    }
  }

  public void UpdateLog(String update) { logController.UpdateLog(update); }

  public void Save() { Debug.Log("Your game is saved.. jk not impl yet"); }

  // Restarts the game into its default state
  public void Restart() { SceneManager.LoadScene("SampleScene"); }
}
