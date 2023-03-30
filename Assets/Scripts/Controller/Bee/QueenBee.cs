using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenBee : Bee {

  // Movement variables
  private float _rotationSpeed = 1f;
  private float _flySpeed = 10f;
  private float _flyHeight = 20f;
  private QueenBeeState _currentState = new QueenBeeState();
  private GameState _state;

  public QueenBeeTask Task {
    get { return _task; }
    set { _task = value; }
  }
  private QueenBeeTask _task = null;

  public bool hasTask {
    get { return Task != null; }
  }

  public bool atTask {
    get {
      var distance = Task.taskLocation - transform.position;
      distance.y = 0;
      return distance.sqrMagnitude < 1; // 2cm
    }
  }

  public bool hasLanded {
    get {
      var distance = Task.taskLocation - transform.position;
      return distance.sqrMagnitude < 10; // basically on task
    }
  }

  public bool hasEgg {
    get {
      
      return false;
    }
  }

  public bool hasTakenOff {
    get {
      return transform.position.y >= _flyHeight; // basically on task
    }
  }

  // Start is called before the first frame update
  protected override void Start() {
    base.Start();

    GameObject state = GameObject.Find("GameState");
    if (state != null) {
      _state = GameObject.Find("GameState").GetComponent<GameState>();
    }
  }

  // Update is called once per frame
  protected void Update() { _currentState.Execute(this); }

  public void SetEggTask() {}

  public void SetHomeTask() {}

  /*** State methods ***/
  public void ChangeState(QueenBeeState newState) {
    _currentState = newState;
    Debug.Log(_currentState.GetType().Name);
  }

  public void IdleState() {}

  public void DieState() {
    if (!isDead) {
      isDead = true;

      // _anim.SetBool("Flying", false);
      // _anim.SetBool("Working", false);

      // _anim.SetBool("Die", true);

    } else {
      _controller.Move(new Vector3(0, -0.1f, 0));
    }
  }

  public void LayEggState() {
    // todo lay the egg
  }

  public void TravelState() {
    _anim.speed = 1f;
    _anim.SetBool("Flying", true);
    RotateTo(Task.taskLocation);

    Vector3 taskBeeVec = Task.taskLocation - transform.position;
    Vector3 forwardVec = transform.forward;
    taskBeeVec.y = 0;
    forwardVec.y = 0;
    if ((Vector3.Angle(taskBeeVec, forwardVec) < 10)) {
      _controller.Move(transform.TransformDirection(Vector3.forward) *
                       _flySpeed * Time.deltaTime * GameState.GetGameSpeed());
    }

    // check if they are at the correct height
    if (Mathf.Abs(transform.position.y - _flyHeight) > 0.05) {
      int dir = (transform.position.y < _flyHeight) ? 1 : -1;
      _controller.Move(transform.TransformDirection(Vector3.up) * dir *
                       _flySpeed * Time.deltaTime * GameState.GetGameSpeed());
    }
  }

  public void LandState() {
    RotateTo(Task.taskLocation);
    _anim.speed = 0.5f;

    // fly straight down
    _controller.Move(new Vector3(
        0, -_flySpeed * Time.deltaTime * GameState.GetGameSpeed(), 0));
  }

  public void TakeOffState() {
    _anim.speed = 0.5f;
    _anim.SetBool("Flying", true);
    _anim.SetBool("Working", false);

    // fly straight up
    _controller.Move(new Vector3(
        0, _flySpeed * Time.deltaTime * GameState.GetGameSpeed(), 0));
  }

  public void RotateTo(Vector3 location) {
    float singleStep =
        _rotationSpeed * Time.deltaTime * GameState.GetGameSpeed();
    Vector3 taskBeeVec = location - transform.position;
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
}
