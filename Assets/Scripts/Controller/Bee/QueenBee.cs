using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenBee : Bee {

  // Movement variables
  private float _rotationSpeed = 1f;
  private float _flySpeed = 10f;
  private float _flyHeight = 20f;
  private QueenBeeState _currentState = new QueenBeeIdleState();
  private Animator _anim;
  private CharacterController _controller;
  private GameState _state;

  public QueenBeeTask Task {
    get => _task;
    set => _task = value;
  }
  private QueenBeeTask _task = null;

  public bool HasTask => Task != null;

  public bool AtTask {
    get {
      var distance = Task.taskLocation - transform.position;
      distance.y = 0;
      return distance.sqrMagnitude < 1; // 2cm
    }
  }

  public bool HasLanded {
    get {
      var distance = Task.taskLocation - transform.position;
      return distance.sqrMagnitude < 10; // basically on task
    }
  }

  public bool HasEgg {
    get { return _state._beeEggSlots.Find((value) => !value.HasEgg); }
  }

  public bool HasTakenOff => transform.position.y >= _flyHeight; // basically on task

  // Start is called before the first frame update
  protected override void Start() {
    base.Start();
    _anim = gameObject.GetComponentsInChildren<Animator>()[0];
    _controller = gameObject.GetComponent<CharacterController>();
    GameObject state = GameObject.Find("GameState");
    if (state != null) {
      _state = GameObject.Find("GameState").GetComponent<GameState>();
    }
    ChangeState(new QueenBeeIdleState());
  }

  // Update is called once per frame
  protected void Update() { _currentState.Execute(this); }

  public void SetEggTask() {
    BeeEggSlot slot = _state._beeEggSlots.Find((value) => !value.HasEgg);
    Task = new QueenBeeTask(QueenBeeTask.TaskType.LayEgg,
                            slot.gameObject.transform.position, slot);
  }

  public void SetHomeTask() {
    Task = new QueenBeeTask(QueenBeeTask.TaskType.ReturnToNest,
                            _state.QueenHoneycomb.workSpots[0].position);
  }

  /*** State methods ***/
  public void ChangeState(QueenBeeState newState) {
    _currentState = newState;
    Debug.Log(_currentState.GetType().Name);
  }

  public void IdleState() {
    _anim.SetBool("Flying", false);
    _anim.SetBool("Idle", true);

    // Make sure we are touching the floor
    if (!_controller.isGrounded)
      _controller.Move(new Vector3(0, -_flySpeed * Time.deltaTime, 0));
  }

  public void DieState() {
    if (!isDead) {
      isDead = true;
      _anim.SetBool("Flying", false);
      _anim.SetBool("Idle", true);
    } else {
      _controller.Move(new Vector3(0, -0.1f, 0));
    }
  }

  public void LayEggState() {
    // add egg to slot
    if (Task != null && Task._curSlot)
      Task._curSlot.HasEgg = true;
  }

  public void TravelState() {
    _anim.SetBool("Flying", true);
    _anim.SetBool("Idle", false);

    RotateTo(Task.taskLocation);

    var taskBeeVec = Task.taskLocation - transform.position;
    var forwardVec = transform.forward;
    taskBeeVec.y = 0;
    forwardVec.y = 0;
    if ((Vector3.Angle(taskBeeVec, forwardVec) < 10)) {
      _controller.Move(transform.TransformDirection(Vector3.forward) * (_flySpeed * Time.deltaTime * GameState.GetGameSpeed()));
    }

    // check if they are at the correct height
    if (Mathf.Abs(transform.position.y - _flyHeight) > 0.05) {
      var dir = (transform.position.y < _flyHeight) ? 1 : -1;
      _controller.Move(transform.TransformDirection(Vector3.up) * (dir * _flySpeed * Time.deltaTime * GameState.GetGameSpeed()));
    }
  }

  public void LandState() {
    RotateTo(Task.taskLocation);

    // fly straight down
    _controller.Move(new Vector3(
        0, -_flySpeed * Time.deltaTime * GameState.GetGameSpeed(), 0));
  }

  public void TakeOffState() {
    _anim.SetBool("Flying", true);
    _anim.SetBool("Idle", false);

    // fly straight up
    _controller.Move(new Vector3(
        0, _flySpeed * Time.deltaTime * GameState.GetGameSpeed(), 0));
  }

  public void RotateTo(Vector3 location) {
    var singleStep =
        _rotationSpeed * Time.deltaTime * GameState.GetGameSpeed();
    var taskBeeVec = location - transform.position;

    // only want to rotate on the xz axis
    taskBeeVec.y = 0;
    taskBeeVec.y = 0;

    // based off of
    // https://docs.unity3d.com/ScriptReference/Vector3.RotateTowards.html
    var newDirection =
        Vector3.RotateTowards(transform.forward, taskBeeVec, singleStep, 0f);
    transform.rotation = Quaternion.LookRotation(newDirection);
  }
}
