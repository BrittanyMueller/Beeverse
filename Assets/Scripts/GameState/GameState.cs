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

  // Bounding box where bees are allowed to idle too.
  public BoxCollider idleArea = null;

  public List<Honeycomb> _honeycombs = new List<Honeycomb>();

  private System.Random _rand = new System.Random();

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

  private BeeResources resources;
  public float honeyCount {
    get { return resources.honey; }
  }
  public float nectarCount {
    get { return resources.nectar; }
  }
  public float beeswaxCount {
    get { return resources.beeswax; }
  }
  public float pollenCount {
    get { return resources.pollen; }
  }
  public float royalJellyCount {
    get { return resources.royalJelly; }
  }

  // generator to create honeycombs
  public HoneycombGenerator honeycombGenerator;

  public int Day {
    get { return _day; }
  }
  private int _day;

  // Game config settings
  public static int minutesPerSecond = 5;

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

    _secondTimer = 1 / minutesPerSecond;

    GameState.minutesPerSecond = 5;

    // Might change when saving comes in
    _currentTime = new TimeTracker(1, 0, 0);
    _day = 0;

    // Set default resources count
    resources = new BeeResources();
    resources.beeswax = 0;
    resources.honey = 0;
    resources.nectar = 50;
    resources.pollen = 100;
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
      _secondTimer = 5.0f / minutesPerSecond;
      _currentTime.AddMinutes(5);
      // Update game UI with new time
      dayController.UpdateDate(_currentTime.ToString());

      // Update all bees that time has passed
      foreach (Bee bee in _bees) {
        bee.UpdateTimeTick(5);
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

  /**
   * Gets the current speed of the game assuming 5 minutes is 1x
   */
  public static float GetGameSpeed() { return GameState.minutesPerSecond / 5; }

  /**
   * Gets a random idle location for a bee to travel
   * to when not doing anything.
   */
  public Vector3 GetIdleLocation() {
    if (idleArea == null)
      return new Vector3(0, 0, 0);
    var size = idleArea.size;
    // Calculates a point in the box collider for idle
    // note uses -z because the game object will be at the top left of the
    // idleArea
    return idleArea.transform.position +
           new Vector3(_rand.Next(0, (int)size.x), 0,
                       -_rand.Next(0, (int)size.z));
  }

  /**
   * Creates a honeycomb at pos of a given type
   */
  public void CreateHoneycomb(Vector3 pos, StructureType type) {
    if (Paused) return;
    honeycombGenerator.CreateHoneycomb(pos, type);
    honeycombGenerator.HideBuildingHints();
  }

  /**
   * Functions for setting resources
   * resources will be capped at 9999
   */
  public void AddPollen(int pollen) {
    resources.pollen += pollen;
    _totalResources.pollen += pollen;
    if (resources.pollen > 9999)
      resources.pollen = 9999;
  }

  public void AddNectar(int nectar) {
    resources.nectar += nectar;
    _totalResources.nectar += nectar;
    if (resources.nectar > 9999)
      resources.nectar = 9999;
  }

  public void AddHoney(int honey) {
    resources.honey += honey;
    _totalResources.honey += honey;
    if (resources.honey > 9999)
      resources.honey = 9999;
  }

  public void AddRoyalJelly(int royalJelly) {
    resources.royalJelly += royalJelly;
    _totalResources.royalJelly += royalJelly;
    if (resources.royalJelly > 9999)
      resources.royalJelly = 9999;
  }

  public void AddBeeswax(int beeswax) {
    resources.beeswax += beeswax;
    _totalResources.beeswax += beeswax;
    if (resources.beeswax > 9999)
      resources.beeswax = 9999;
  }

  /**
   * Checks to see if we have enough resources and consumes them
   * if we can.
   * @returns true if the resources were consumed
   * @returns false if the resources weren't consumed
   */
  public bool ConsumeResources(BeeResources consume) {

    // test if we can consume the resources
    if (consume.beeswax > resources.beeswax)
      return false;
    if (consume.honey > resources.honey)
      return false;
    if (consume.royalJelly > resources.royalJelly)
      return false;
    if (consume.nectar > resources.nectar)
      return false;
    if (consume.pollen > resources.pollen)
      return false;

    // consume the resources.
    resources.beeswax -= consume.beeswax;
    resources.honey -= consume.honey;
    resources.royalJelly -= consume.royalJelly;
    resources.nectar -= consume.nectar;
    resources.pollen -= consume.pollen;
    return true;
  }

  /**
   * Sets the run speed of the game in minutes
   * 1x speed is 5 minutes
   * 2x speed is 10 minutes
   * 5x speed is 25 minutes
   */
  public void setGameSpeed(int minutes) {
    if (Paused) return;
    GameState.minutesPerSecond = minutes;
  }
}
