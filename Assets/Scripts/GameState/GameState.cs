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

  public List<WorkerBee> _bees;
  private int _deadBees = 0;
  public QueenBee _queen;

  public List<Honeycomb> _honeycombs = new List<Honeycomb>();
  // Queen honeycomb will always be the first one generated
  public Honeycomb QueenHoneycomb {
    get { return _honeycombs[0]; }
  }

  // Controllers for GUIs
  public LogController logController;
  public ResourceController resourceController;
  public HudController hudController;
  public GameOverController gameOverController;
  public DayController dayController;
  public DebugController debugController;

  private BeeResources resources;

  // generator to create honeycombs
  public HoneycombGenerator honeycombGenerator;

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
    resources = new BeeResources();
    resources.beeswax = 0;
    resources.honey = 0;
    resources.nectar = 0;
    resources.pollen = 0;
    resources.royalJelly = 0;
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
      UpdateLog("> " + BeeStuff.GetRandomPun());
    }

    // Remove all bees that have died
    foreach (Bee bee in _bees) {
      if (bee.isDead) {
        StartCoroutine(RemoveBee(bee));
        UpdateLog(">" + bee.beeName + " has died. rest in Bees");
      }
    }
    _bees.RemoveAll((WorkerBee bee) => {
      if (bee.isDead)
        _deadBees++;
      return bee.isDead;
    });

    // Bring up Lose Menu
    if (_bees.Count == 0 && _deadBees == 0) {
      gameOverController.SetTotalBees(_totalBees);
      gameOverController.SetTotalQueen(_totalQueens);
      gameOverController.SetTotalResources(_totalResources);
      gameOverController.SetTotalHoneycombs(_honeycombs.Count);
      gameOverController.SetTotalDays(_currentTime.day);
      Paused = true;
      hudController.GameOver();
    }

    resourceController.UpdateResources(resources);
  }

  void Update() {
    if (Input.GetKeyDown(KeyCode.F1)) {
      debugController.toggle();
    }
  }

  IEnumerator RemoveBee(Bee bee) {
    yield return new WaitForSeconds(10);
    _deadBees--;
    Destroy(bee.gameObject);
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

  // Update the log with information

  public void UpdateLog(String update) { logController.UpdateLog(update); }

  public void Save() { Debug.Log("Your game is saved.. jk not impl yet"); }

  // Restarts the game into its default state
  public void Restart() {
    Paused = false;
    SceneManager.LoadScene("Beeverse");
  }

  public void CreateHoneycomb(Vector3 pos, Honeycomb.HoneycombType type) {
    // Double check you can afford it TODO
    honeycombGenerator.CreateHoneycomb(pos, type);
    honeycombGenerator.HideBuildingHints();
  }

  /** Functions for setting resources */
  public void AddPollen(float pollen) {
    resources.pollen += pollen;
    _totalResources.pollen += pollen;
  }

  public void AddNectar(float nectar) {
    resources.nectar += nectar;
    _totalResources.nectar += nectar;
  }

  public void AddHoney(float honey) {
    resources.honey += honey;
    _totalResources.honey += honey;
  }
}
