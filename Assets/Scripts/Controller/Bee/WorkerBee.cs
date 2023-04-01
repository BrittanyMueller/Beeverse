using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerBee : Bee {
  // Model facial features used for its dead state
  public GameObject _leftEye;
  public GameObject _rightEye;
  public GameObject _smile;
  public GameObject _leftEyeDead;
  public GameObject _rightEyeDead;
  public GameObject _frown;

  // location where the bee travels for idle animation;
  private Vector3 _idleLocation;
  private float _idleFlyHeight = 30f;

  // Objects for Task
  public Flower flower;
  public Honeycomb honeycomb;

  private Animator _anim;
  private CharacterController _controller;

  private WorkerBeeState _currentState = new WorkerBeeIdleState();
  private GameState _state;

  // Movement variables
  private float _rotationSpeed = 1f;
  private float _flySpeed = 10f;
  private float _flyHeight = 20f;

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

  public bool HasTask => Task != null;

  public bool AtTask {
    get {
      var distance = Task.taskLocation - transform.position;
      distance.y = 0;
      return distance.sqrMagnitude < 0.5;  // 2cm
    }
  }

  public bool AtIdleLocation {
    get {
      var distance = _idleLocation - transform.position;
      distance.y = 0;
      return distance.sqrMagnitude < 1;  // 1cm
    }
  }

  public bool HasLanded {
    get {
      var distance = Task.taskLocation - transform.position;
      return distance.sqrMagnitude < 10;  // basically on task
    }
  }

  public bool HasTakenOff => transform.position.y >= _flyHeight;  // basically on task

  public string JobTitle {
    get {
      if (_task == null) {
        return "Honey Enjoyer";
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
          return "Royal Jelly Architect";
        case WorkerBeeTask.TaskType.HoneyFactory:
          return "Honey Technician";
      }
      return "NULL";
    }
  }

  // Start is called before the first frame update
  protected override void Start() {
    base.Start();
    _anim = gameObject.GetComponentsInChildren<Animator>()[0];
    _controller = gameObject.GetComponent<CharacterController>();
    var state = GameObject.Find("GameState");
    if (state != null) {
      _state = GameObject.Find("GameState").GetComponent<GameState>();
    }
    ChangeState(new WorkerBeeIdleState());
  }

  // Update is called once per frame
  protected void Update() {
    _currentState.Execute(this);
  }

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
      GetComponent<AudioSource>().Play(0);
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

    // check if we have an idle location or if we are there
    // to see if we need a new location or if we should
    // keep traveling to it
    if (_idleLocation == Vector3.zero || AtIdleLocation) {
      _idleLocation = _state.GetIdleLocation();
    } else {
      TravelToIdle();
    }
  }

  public void TravelToIdle() {
    // Make sure it is set
    if (_idleLocation == Vector3.zero)
      return;
    RotateTo(_idleLocation);

    var taskBeeVec = _idleLocation - transform.position;
    var forwardVec = transform.forward;
    taskBeeVec.y = 0;
    forwardVec.y = 0;
    if ((Vector3.Angle(taskBeeVec, forwardVec) < 10)) {
      _controller.Move(transform.TransformDirection(Vector3.forward) *
                       (_flySpeed * Time.deltaTime * GameState.GetGameSpeed()));
    }

    // check if they are at the correct height
    if (Mathf.Abs(transform.position.y - _idleFlyHeight) > 0.05) {
      var dir = (transform.position.y < _idleFlyHeight) ? 1 : -1;
      _controller.Move(transform.TransformDirection(Vector3.up) *
                       (dir * _flySpeed * Time.deltaTime * GameState.GetGameSpeed()));
    }
  }

  public void TravelToTask() {
    _anim.speed = 1f;
    _anim.SetBool("Flying", true);
    RotateTo(Task.taskLocation);

    var taskBeeVec = Task.taskLocation - transform.position;
    var forwardVec = transform.forward;
    taskBeeVec.y = 0;
    forwardVec.y = 0;
    if ((Vector3.Angle(taskBeeVec, forwardVec) < 10)) {
      _controller.Move(transform.TransformDirection(Vector3.forward) *
                       (_flySpeed * Time.deltaTime * GameState.GetGameSpeed()));
    }

    // check if they are at the correct height
    if (Mathf.Abs(transform.position.y - _flyHeight) > 0.05) {
      var dir = (transform.position.y < _flyHeight) ? 1 : -1;
      _controller.Move(transform.TransformDirection(Vector3.up) *
                       (dir * _flySpeed * Time.deltaTime * GameState.GetGameSpeed()));
    }
  }
  public void LandAtTask() {
    RotateTo(Task.taskLocation);
    _anim.speed = 0.5f;

    // fly straight down
    _controller.Move(new Vector3(0, -_flySpeed * Time.deltaTime * GameState.GetGameSpeed(), 0));
  }

  public void Work() {
    _anim.speed = 1f;
    _anim.SetBool("Flying", false);
    _anim.SetBool("Working", true);

    
    // snap them to the work spot
    _controller.enabled = false;
    _controller.transform.position = Task.taskLocation;
    _controller.enabled = true;
    

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
    _controller.Move(new Vector3(0, _flySpeed * Time.deltaTime * GameState.GetGameSpeed(), 0));
  }

  /*** State Helpers ***/

  private void RotateTo(Vector3 location) {
    var singleStep = _rotationSpeed * Time.deltaTime * GameState.GetGameSpeed();
    var taskBeeVec = location - transform.position;

    // only want to rotate on the xz axis
    taskBeeVec.y = 0;
    taskBeeVec.y = 0;

    // based off of
    // https://docs.unity3d.com/ScriptReference/Vector3.RotateTowards.html
    var newDirection = Vector3.RotateTowards(transform.forward, taskBeeVec, singleStep, 0f);
    transform.rotation = Quaternion.LookRotation(newDirection);
  }

  // Makes the bee look in the correct direction when working
  private void RotateToJobTarget() {
    // no targetLocation ignore
    if (Task.targetLocation.x == 0 && Task.targetLocation.y == 0)
      return;

    var singleStep = _rotationSpeed * Time.deltaTime * GameState.GetGameSpeed();
    var targetBeeVec = Task.targetLocation - transform.position;

    // only want to rotate on the xz axis
    targetBeeVec.y = 0;
    targetBeeVec.y = 0;

    // based off of
    // https://docs.unity3d.com/ScriptReference/Vector3.RotateTowards.html
    var newDirection = Vector3.RotateTowards(transform.forward, targetBeeVec, singleStep, 0f);
    transform.rotation = Quaternion.LookRotation(newDirection);
  }

  private void WorkAtFactory() {
    if (_workTimerSet && _workTimer <= 0) {  // currently working see if we are done
      ((HoneycombFactory)honeycomb).AddResource(_state);
      _workTimerSet = false;
    }
    if (!_workTimerSet) {
      // see if we can afford to start working
      ((HoneycombFactory)honeycomb).ConsumeResources(_state);
      _workTimer = ((HoneycombFactory)honeycomb).conversionTimeMinutes;
      _workTimerSet = true;
    }
  }

  private void BuildHoneycomb() {
    honeycomb.BuildProgress +=
        honeycomb.buildSpeedPerSecond * Time.deltaTime * GameState.GetGameSpeed();
    // honeycomb finished building change job
    if (honeycomb.Built) {
      _task.taskType = WorkerBeeTask.StructureTypeAsTaskType(honeycomb.honeycombType);
    }
  }

  private void WorkAtFlower() {
    // Check if we have finished working
    if (_workTimerSet && _workTimer <= 0) {  // currently working see if we are done
      _state.AddPollen(flower.pollenCollectionRate);
      _state.AddNectar(flower.nectarCollectionRate);
      _workTimerSet = false;
    }
    // Start working
    if (!_workTimerSet) {
      _workTimerSet = true;
      _workTimer = flower.conversionTimeMinutes;
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
