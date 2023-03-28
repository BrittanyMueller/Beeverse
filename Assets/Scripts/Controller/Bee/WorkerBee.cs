using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerBee : Bee {

  // Objects used for its dead state
  public GameObject _leftEye;
  public GameObject _rightEye;
  public GameObject _smile;
  public GameObject _leftEyeDead;
  public GameObject _rightEyeDead;
  public GameObject _frown;

  // Objects for Task
  public Flower flower;
  public Honeycomb honeycomb;

  private Animator _anim;
  private CharacterController _controller;

  private WorkerBeeState _currentState;
  private GameState _state;

  // Movement variables
  private float _rotationSpeed = 1f;
  private float _flySpeed = 10f;

  // Used to keep track of work progress
  private bool _workTimerSet = false;
  private int _workTimer;

  /****** Task management ******/

  public bool taskChanged = false;
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
  private WorkerBeeTask _task = null;

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

  // Start is called before the first frame update
  protected override void Start() {
    base.Start();
    _anim = gameObject.GetComponentsInChildren<Animator>()[0];
    _controller = gameObject.GetComponent<CharacterController>();
    _state = GameObject.Find("GameState").GetComponent<GameState>();

    ChangeState(new WorkerBeeIdleState());
  }

  // Update is called once per frame
  protected void Update() { _currentState.Execute(this); }

  public override void UpdateTimeTick(int minutes) {
    base.UpdateTimeTick(minutes);

    // decrement the work timer if it is set
    if (_workTimerSet) {
      _workTimer -= minutes;
    }
  }

  /** States*/
  public void ChangeState(WorkerBeeState newState) {
    _currentState = newState;
    Debug.Log(_currentState.GetType().Name);
  }

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
      _controller.Move(new Vector3(0, -0.1f, 0));
    }
  }

  public void Idle() {
    _anim.speed = 1f;
    _anim.SetBool("Flying", true);

    // Todo improve IDLE
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
      _controller.Move(transform.TransformDirection(Vector3.forward) *
                       _flySpeed * Time.deltaTime * GameState.minutesPreSecond /
                       5);
    }
  }

  public void LandAtTask() {
    RotateToTask();
    _anim.speed = 0.5f;

    // fly straight down
    _controller.Move(new Vector3(
        0, -_flySpeed * Time.deltaTime * GameState.minutesPreSecond / 5, 0));
  }

  public void Work() {
    _anim.speed = 1f;
    _anim.SetBool("Flying", false);
    _anim.SetBool("Working", true);

    // Make sure we are touching the floor
    if (!_controller.isGrounded)
      _controller.Move(new Vector3(0, -_flySpeed * Time.deltaTime, 0));

    switch (_task.taskType) {
    case WorkerBeeTask.TaskType.Forager:
      WorkAtFlower();
      break;
    case WorkerBeeTask.TaskType.Builder:
      BuildHoneycomb();
      break;
    case WorkerBeeTask.TaskType.HoneyFactory:
    case WorkerBeeTask.TaskType.BeeswaxFactory:
    case WorkerBeeTask.TaskType.RoyalJellyFactory:
      WorkAtFactory();
      break;
    }

    RotateToJobTarget();
  }

  public void TakeOff() {
    _anim.speed = 0.5f;
    _anim.SetBool("Flying", true);
    _anim.SetBool("Working", false);

    // fly straight up
    _controller.Move(new Vector3(
        0, _flySpeed * Time.deltaTime * GameState.minutesPreSecond / 5, 0));
  }

  /*** State Helpers ***/

  public void RotateToTask() {
    float singleStep =
        _rotationSpeed * Time.deltaTime * GameState.minutesPreSecond / 5;
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

  // Makes the bee look in the correct direction when working
  public void RotateToJobTarget() {
    // no targetLocation ignore
    if (Task.targetLocation.x == 0 && Task.targetLocation.y == 0)
      return;

    float singleStep =
        _rotationSpeed * Time.deltaTime * GameState.minutesPreSecond / 5;
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

  public void WorkAtFactory() {
    if (_workTimerSet) { // currently working see if we are done
      if (_workTimer <= 0) {
        ((HoneycombFactory)honeycomb).AddResource(_state);
        _workTimerSet = false;
      }
    }
    if (!_workTimerSet) {
      // see if we can afford to start working
      ((HoneycombFactory)honeycomb).ConsumeResources(_state);
      _workTimer = ((HoneycombFactory)honeycomb).conversionTimeMinutes;
      _workTimerSet = true;
    }
  }

  public void BuildHoneycomb() {
    honeycomb.buildProgress += honeycomb.buildSpeedPerSecond * Time.deltaTime *
                               GameState.minutesPreSecond / 5;
    // honeycomb finished building change job
    if (honeycomb.built) {
      _task.taskType =
          WorkerBeeTask.StructureTypeAsTaskType(honeycomb.honeycombType);
    }
  }

  public void WorkAtFlower() {
    if (_workTimerSet) { // currently working see if we are done
      if (_workTimer <= 0) {
        _state.AddPollen(1);
        _state.AddNectar(1);
        _workTimerSet = false;
      }
    }

    if (!_workTimerSet) {
      // start working
      _workTimerSet = true;
      _workTimer = flower.ConversionTimeMinutes; 
    }
  }

  /**
   * This should be done when the state transitions
   * from work to any other state
   */
  public void ResetWorkTimer() {
    _workTimerSet = false;
    _workTimer = 0;
  }
}
