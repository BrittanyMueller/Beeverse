using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerBee : Bee {
  private Animator _anim;
  private CharacterController controller;

  private WorkerBeeState currentState;

  // Objects used for its dead state
  public GameObject _leftEye;
  public GameObject _rightEye;
  public GameObject _smile;
  public GameObject _leftEyeDead;
  public GameObject _rightEyeDead;
  public GameObject _frown;

  // Movement variables
  private float _rotationSpeed = 1f;
  private float flySpeed = 10f;
  public float flyThreshold = 10f;
  private Vector3 curForward;

  // used to keep track of work progress
  private bool workTimerSet = false;
  private int workTimer;

  private GameState _state;

  // Target where the bee should move to
  private WorkerBeeTask _task = null;

  public string jobTitle {
    get {
      if (_task == null) {
        return "Bum";
      }
      switch (_task.taskType) {
      case WorkerBeeTask.TaskType.Builder:
        return "Builder";
      case WorkerBeeTask.TaskType.Forager:
        return "Forager";
      case WorkerBeeTask.TaskType.Nurse:
        return "Nurse";
      case WorkerBeeTask.TaskType.BeeswaxFactory:
        return "Beeswax Engineer";
      case WorkerBeeTask.TaskType.RoyalJellyFactory:
        return "Royal Jelly Tech";
      case WorkerBeeTask.TaskType.HoneyFactory:
        return "Honey Enjoyer";
      }

      return "NULL";
    }
  }
  // TODO cleanup
  public WorkerBeeTask Task {
    get { return _task; }
    set {
      // set only if it already had a task
      if (_task != null) {

        taskChanged = true;
        // remove from old task
        switch (_task.taskType) {
        case WorkerBeeTask.TaskType.Forager:
          // set old flower spot to null
          flower.bees[_task.workerSpotIndex] = null;
          flower = null;
          break;
        default:
          // set old flower spot to null
          honeycomb.bees[_task.workerSpotIndex] = null;
          honeycomb = null;
          break;
        }
      }

      _task = value;
    }
  }
  public bool taskChanged = false;
  public bool hasTask {
    get { return Task != null; }
  }
  public bool atTask {
    get {
      var distance = Task.taskLocation - transform.position;

      distance.y = 0;
      return distance.sqrMagnitude < 4; // 2cm
    }
  }
  public bool hasLanded {
    get {
      var distance = Task.taskLocation - transform.position;
      return distance.sqrMagnitude < 20; // basically on task
    }
  }

  public bool hasTakenOff {
    get {
      return transform.position.y >= 20; // basically on task
    }
  }

  // Objects for job
  public Flower flower;
  public Honeycomb honeycomb;

  // Start is called before the first frame update
  protected override void Start() {
    base.Start();
    _anim = gameObject.GetComponentsInChildren<Animator>()[0];
    controller = gameObject.GetComponent<CharacterController>();
    _state = GameObject.Find("GameState").GetComponent<GameState>();

    ChangeState(new WorkerBeeIdleState());
  }

  // Update is called once per frame
  protected void Update() { currentState.Execute(this); }

  public override void UpdateTimeTick(int minutes) {
    base.UpdateTimeTick(minutes);

    // decrement the work timer if it is set
    if (workTimerSet) {
      workTimer -= minutes;
    }
  }
  /** States*/
  public void Die() {
    if (!isDead) {
      isDead = true;

      _anim.SetBool("Flying", false);
      _anim.SetBool("Working", false);

      _anim.SetBool("Die", true);

      _smile.SetActive(false);
      _leftEye.SetActive(false);
      _rightEye.SetActive(false);

      _frown.SetActive(true);
      _leftEyeDead.SetActive(true);
      _rightEyeDead.SetActive(true);
    } else {
      controller.Move(new Vector3(0, -0.1f, 0));
    }
  }

  public void Idle() {
    _anim.speed = 1f;
    _anim.SetBool("Flying", true);
  }

  public void TravelToTask() {
    _anim.speed = 1f;
    _anim.SetBool("Flying", true);
    RotateToTask();

    Vector3 taskBeeVec = Task.taskLocation - transform.position;
    Vector3 forwardVec = transform.forward;
    taskBeeVec.y = 0;
    forwardVec.y = 0;
    if ((Vector3.Angle(taskBeeVec, forwardVec) < 10)) {
      controller.Move(transform.TransformDirection(Vector3.forward) * flySpeed *
                      Time.deltaTime * GameState.minutesPreSecond/5 );
    }
  }

  public void LandAtTask() {
    RotateToTask();
    _anim.speed = 0.5f;

    // fly straight down
    controller.Move(new Vector3(0, -flySpeed * Time.deltaTime * GameState.minutesPreSecond/5, 0));
  }

  public void Work() {
    _anim.speed = 1f;
    _anim.SetBool("Flying", false);
    _anim.SetBool("Working", true);

    // Make sure we are touching the floor
    if (!controller.isGrounded)
      controller.Move(new Vector3(0, -flySpeed * Time.deltaTime, 0));

    switch (_task.taskType) {
    case WorkerBeeTask.TaskType.Forager:

      if (workTimerSet) { // currently working see if we are done
        if (workTimer <= 0) {
          _state.AddPollen(flower.PollenPerSecond);
          _state.AddNectar(flower.NectarPerSecond);
          workTimerSet = false;
        }
      }

      if (!workTimerSet) {
        // start working
        workTimerSet = true;
        workTimer = 5; // todo change to value from flower
      }
      break;
    case WorkerBeeTask.TaskType.Builder:
      honeycomb.buildProgress += honeycomb.buildSpeedPerSecond * Time.deltaTime * GameState.minutesPreSecond/5;
      // honeycomb finished building change job
      if (honeycomb.built) {
        _task.taskType =
            WorkerBeeTask.StructureTypeAsTaskType(honeycomb.honeycombType);
      }
      break;
    case WorkerBeeTask.TaskType.HoneyFactory:
    case WorkerBeeTask.TaskType.BeeswaxFactory:
    case WorkerBeeTask.TaskType.RoyalJellyFactory:

      if (workTimerSet) { // currently working see if we are done
        if (workTimer <= 0) {
          ((HoneycombFactory)honeycomb).AddResource(_state);
          workTimerSet = false;
        }
      }
      if (!workTimerSet) {
        // see if we can afford to start working
        ((HoneycombFactory)honeycomb).ConsumeResources(_state);
        workTimer = ((HoneycombFactory)honeycomb).conversionTimeMinutes;
        workTimerSet = true;
      }
      break;
    }

    RotateToJobTarget();
  }

  public void TakeOff() {
    _anim.speed = 0.5f;
    _anim.SetBool("Flying", true);
    _anim.SetBool("Working", false);

    // fly straight up
    controller.Move(new Vector3(0, flySpeed * Time.deltaTime * GameState.minutesPreSecond/5 , 0));
  }

  public void ChangeState(WorkerBeeState newState) {
    currentState = newState;
    Debug.Log(currentState.GetType().Name);
  }

  /** Movement helpers */
  public void RotateToTask() {
    float singleStep = _rotationSpeed * Time.deltaTime * GameState.minutesPreSecond/5;
    Vector3 taskBeeVec = Task.taskLocation - transform.position;
    Vector3 forwardsVec = transform.forward;

    // only want to rotate on the xz axis
    taskBeeVec.y = 0;
    taskBeeVec.y = 0;

    // based off of
    // https://docs.unity3d.com/ScriptReference/Vector3.RotateTowards.html
    var newDirection =
        Vector3.RotateTowards(transform.forward, taskBeeVec, singleStep, 0f);
    transform.rotation = Quaternion.LookRotation(newDirection);
  }

  // Makes the bee look in the correct direciton when working
  public void RotateToJobTarget() {
    // no targetLocation ignore
    if (Task.targetLocation.x == 0 && Task.targetLocation.y == 0)
      return;

    float singleStep = _rotationSpeed * Time.deltaTime * GameState.minutesPreSecond/5;
    Vector3 targetBeeVec = Task.targetLocation - transform.position;
    Vector3 forwardsVec = transform.forward;

    // only want to rotate on the xz axis
    targetBeeVec.y = 0;
    targetBeeVec.y = 0;

    // based off of
    // https://docs.unity3d.com/ScriptReference/Vector3.RotateTowards.html
    var newDirection =
        Vector3.RotateTowards(transform.forward, targetBeeVec, singleStep, 0f);
    transform.rotation = Quaternion.LookRotation(newDirection);
  }

  /**
   * This should be done when the state transitions
   * from work to any other state
   */
  public void ResetWorkTimer() {
    workTimerSet = false;
    workTimer = 0;
  }
}
