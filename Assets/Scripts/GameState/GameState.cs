using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour {
  // A bool to notify we aren't actually playing the game
  public bool inMenu = false;
  // Audio stuff
  public AudioSource backgroundMusic;

  // Audio clips
  public AudioClip audioDayTimeMusic;
  public AudioClip audioNightTimeMusic;

  public Light sunLight;
  public Material skyMaterial;

  private TimeTracker _currentTime;
  public TimeTracker CurrentTime => _currentTime;

  public List<WorkerBee> _bees;
  public List<BeeEggSlot> _beeEggSlots;
  private int _deadBees = 0;
  public QueenBee _queen;

  private bool _hasQueenEgg;
  public bool HasQueenEgg {
    get => _hasQueenEgg;
    set => _hasQueenEgg = value;
  }

  // Bounding box where bees are allowed to idle too.
  public BoxCollider idleArea = null;

  public List<Honeycomb> _honeycombs = new List<Honeycomb>();

  private System.Random _rand = new System.Random();

  // Queen honeycomb will always be the first one generated
  public Honeycomb QueenHoneycomb => _honeycombs[0];

  // Bee models
  public GameObject queenModel;
  public GameObject workerModel;

  // Controllers for GUIs
  public LogController logController;
  public ResourceController resourceController;
  public HudController hudController;
  public GameOverController gameOverController;
  public DayController dayController;

  private BeeResources resources;
  public float HoneyCount => resources.Honey;

  public float NectarCount => resources.Nectar;

  public float BeeswaxCount => resources.Beeswax;

  public float PollenCount => resources.Pollen;

  public float RoyalJellyCount => resources.RoyalJelly;

  // generator to create honeycombs
  public HoneycombGenerator honeycombGenerator;

  public int Day => _day;
  private int _day;

  // Game config settings
  public static int MinutesPerSecond = 5;

  // This timer will be used to
  // update the clock every second
  private double _secondTimer;

  public bool Paused {
    get => _paused;
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
    // Make bees ignore collision for other bees
    Physics.IgnoreLayerCollision(6, 6);
    if (inMenu)
      return;

    _paused = false;

    _secondTimer = 1 / MinutesPerSecond;
    MinutesPerSecond = 5;

    // Might change when saving comes in
    _currentTime = new TimeTracker(1, 6, 55);
    _day = 1;

    // Set default resources count
    resources = new BeeResources {
      // TODO revert resources when done debugging (leave till after demo)
      Beeswax = 2000, Honey = 2000, Nectar = 2000, Pollen = 2000, RoyalJelly = 2000
    };
    resourceController.UpdateResources(resources);

    // set init totals
    _totalResources = resources;
    _totalBees = _bees.Count;
    _totalQueens = 1;
  }

  // Update is called once per frame
  private void FixedUpdate() {
    if (inMenu)
      return;
    _secondTimer -= Time.deltaTime;
    if (_secondTimer <= 0) {
      _secondTimer = 5.0f / MinutesPerSecond;
      _currentTime.AddMinutes(5);
      // Update game UI with new time
      dayController.UpdateDate(_currentTime.ToString());

      // daylight nighttime
      if (_currentTime.Hour == 7 && _currentTime.Minute == 0) {
        // Day message
        UpdateLog("Day " + _day);
        UpdateLog("> " + BeeStuff.GetRandomPun());
        // Play morning song
        backgroundMusic.clip = audioDayTimeMusic;
        backgroundMusic.Play(0);
        sunLight.transform.rotation = Quaternion.Euler(70, 90, 90);
        skyMaterial.mainTextureOffset = new Vector2(0f, 0);
      } else if (_currentTime.Hour == 19 && _currentTime.Minute == 0) {
        sunLight.transform.rotation = Quaternion.Euler(30, 90, 90);
        backgroundMusic.clip = audioNightTimeMusic;
        backgroundMusic.Play(0);
        skyMaterial.mainTextureOffset = new Vector2(0.4f, 0);
      }

      // Update all bees that time has passed
      foreach (Bee bee in _bees) {
        bee.UpdateTimeTick(5);
      }
      // Update the queen that time has passed
      if (_queen)
        _queen.UpdateTimeTick(5);

      foreach (var beeSlot in _beeEggSlots) {
        if (beeSlot.HasEgg) {
          beeSlot.babyBee.UpdateTimeTick(5);
          if (beeSlot.babyBee.AgeInDays == 5) {
            var spawnPoint = beeSlot.babyBee.transform.position + new Vector3(0, 5, 0);
            if (beeSlot.babyBee.isQueen) {
              // Baby bee grows into queen bee
              var newQueen = Instantiate(queenModel, spawnPoint, beeSlot.babyBee.transform.rotation)
                                 .GetComponent<QueenBee>();
              newQueen.beeName = beeSlot.babyBee.beeName;
              newQueen.ChangeState(new QueenBeeTakeOffState());
              UpdateLog("> The new Queen Bee was born. Welcome to the hive, " + newQueen.beeName +
                        "!");

              if (_queen) {
                // Remove old queen from the game! Good bye Queen
                UpdateLog("> " + _queen.beeName +
                          " feels replaced and moves on to another hive...");
                _queen.IsLeavingHive = true;
                _queen.ChangeState(new QueenBeeTakeOffState());
                StartCoroutine(RemoveQueenBee(_queen, 15));
              }
              _queen = newQueen;
              _hasQueenEgg = false;
            } else {
              // Baby bee grows into worker bee
              var workerBee =
                  Instantiate(workerModel, spawnPoint, beeSlot.babyBee.transform.rotation)
                      .GetComponent<WorkerBee>();
              workerBee.beeName = beeSlot.babyBee.beeName;
              workerBee.ChangeState(new WorkerBeeTakeOffState());
              _bees.Add(workerBee);
              UpdateLog("> A new Worker Bee was born. Welcome to the hive, " + workerBee.beeName +
                        "!");
            }
            Destroy(beeSlot.babyBee.gameObject);  // RIP
            beeSlot.HasEgg = false;
          }
        }
      }
    }

    // Notify the logs a day has passed
    if (_day != _currentTime.Day) {
      _day += 1;
    }

    // Remove all bees that have died
    foreach (var bee in _bees) {
      if (bee.isDead) {
        StartCoroutine(RemoveBee(bee));
        UpdateLog("> " + bee.beeName + " has died at the old age of " + bee.AgeInDays +
                  " days. Rest in Bees");
      }
    }
    _bees.RemoveAll((WorkerBee bee) => {
      if (bee.isDead)
        _deadBees++;
      return bee.isDead;
    });

    if (_queen && _queen.isDead) {
      StartCoroutine(RemoveQueenBee(_queen));
    }

    // Bring up Lose Menu
    if (_bees.Count == 0 && _deadBees == 0) {
      gameOverController.SetTotalBees(_totalBees);
      gameOverController.SetTotalQueen(_totalQueens);
      gameOverController.SetTotalResources(_totalResources);
      gameOverController.SetTotalHoneycombs(_honeycombs.Count);
      gameOverController.SetTotalDays(_currentTime.Day);
      Paused = true;
      hudController.GameOver();
    }
    resourceController.UpdateResources(resources);
  }

  private IEnumerator RemoveBee(Bee bee) {
    yield return new WaitForSeconds(10);
    _deadBees--;
    Destroy(bee.gameObject);
  }

  private IEnumerator RemoveQueenBee(Bee bee, int wait = 10) {
    _queen = null;
    yield return new WaitForSeconds(wait);
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
  public void UpdateLog(string update) {
    logController.UpdateLog(update);
  }

  public void Save() {
    Debug.Log("Your game is saved.. jk not implemented yet.");
  }

  // Restarts the game into its default state
  public void Restart() {
    Paused = false;
    SceneManager.LoadScene("Beeverse");
  }

  /**
   * Gets the current speed of the game assuming 5 minutes is 1x
   */
  public static float GetGameSpeed() {
    // ReSharper disable once PossibleLossOfFraction
    return MinutesPerSecond / 5;
  }

  /**
   * Gets a random idle location for a bee to travel
   * to when not doing anything.
   */
  public Vector3 GetIdleLocation() {
    if (!idleArea)
      return new Vector3(0, 0, 0);
    var size = idleArea.size;
    // Calculates a point in the box collider for idle
    // note uses -z because the game object will be at the top left of the
    // idleArea
    return idleArea.transform.position +
           new Vector3(_rand.Next(0, (int)size.x), 0, -_rand.Next(0, (int)size.z));
  }

  /**
   * Creates a honeycomb at pos of a given type
   */
  public void CreateHoneycomb(Vector3 pos, StructureType type) {
    if (Paused)
      return;
    honeycombGenerator.CreateHoneycomb(pos, type);
    honeycombGenerator.HideBuildingHints();
  }

  /**
   * Functions for setting resources
   * resources will be capped at 9999
   */
  public void AddPollen(int pollen) {
    resources.Pollen += pollen;
    _totalResources.Pollen += pollen;
    if (resources.Pollen > 9999)
      resources.Pollen = 9999;
  }

  public void AddNectar(int nectar) {
    resources.Nectar += nectar;
    _totalResources.Nectar += nectar;
    if (resources.Nectar > 9999)
      resources.Nectar = 9999;
  }

  public void AddHoney(int honey) {
    resources.Honey += honey;
    _totalResources.Honey += honey;
    if (resources.Honey > 9999)
      resources.Honey = 9999;
  }

  public void AddRoyalJelly(int royalJelly) {
    resources.RoyalJelly += royalJelly;
    _totalResources.RoyalJelly += royalJelly;
    if (resources.RoyalJelly > 9999)
      resources.RoyalJelly = 9999;
  }

  public void AddBeeswax(int beeswax) {
    resources.Beeswax += beeswax;
    _totalResources.Beeswax += beeswax;
    if (resources.Beeswax > 9999)
      resources.Beeswax = 9999;
  }

  /**
   * Checks to see if we have enough resources and consumes them
   * if we can.
   * @returns true if the resources were consumed
   * @returns false if the resources weren't consumed
   */
  public bool ConsumeResources(BeeResources consume) {
    // test if we can consume the resources
    if (consume.Beeswax > resources.Beeswax)
      return false;
    if (consume.Honey > resources.Honey)
      return false;
    if (consume.RoyalJelly > resources.RoyalJelly)
      return false;
    if (consume.Nectar > resources.Nectar)
      return false;
    if (consume.Pollen > resources.Pollen)
      return false;

    // consume the resources.
    resources.Beeswax -= consume.Beeswax;
    resources.Honey -= consume.Honey;
    resources.RoyalJelly -= consume.RoyalJelly;
    resources.Nectar -= consume.Nectar;
    resources.Pollen -= consume.Pollen;
    return true;
  }

  /**
   * Sets the run speed of the game in minutes
   * 1x speed is 5 minutes
   * 2x speed is 10 minutes
   * 5x speed is 25 minutes
   */
  public void SetGameSpeed(int minutes) {
    if (Paused)
      return;
    MinutesPerSecond = minutes;
  }
}
