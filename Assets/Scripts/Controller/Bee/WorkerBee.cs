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

  // Target where the bee should move to
  public WorkerBeeTask _task = null;
  public WorkerBeeTask Task {
    get { return _task; }
    set {
      // set only if it already had a task
      if (_task != null)
        taskChanged = true;
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
      return distance.sqrMagnitude < 10; // basically on task
    }
  }

  public bool hasTakenOff {
    get {
      return transform.position.y >= 20; // basically on task
    }
  }

  // Start is called before the first frame update
  protected override void Start() {
    base.Start();
    _anim = gameObject.GetComponentsInChildren<Animator>()[0];
    controller = gameObject.GetComponent<CharacterController>();
    ChangeState(new WorkerBeeIdleState());
  }

  // Update is called once per frame
  protected void Update() { currentState.Execute(this); }

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
                      Time.deltaTime);
    }
  }

  public void LandAtTask() {
    RotateToTask(true);
    _anim.speed = 0.5f;

    // fly straight down
    controller.Move(new Vector3(0, -flySpeed * Time.deltaTime, 0));
  }

  public void Work() {
    _anim.speed = 1f;
    _anim.SetBool("Flying", false);
    _anim.SetBool("Working", true);

    // Make sure we are touching the floor
    if (!controller.isGrounded)
      controller.Move(new Vector3(0, -flySpeed * Time.deltaTime, 0));
  }

  public void TakeOff() {
    _anim.speed = 0.5f;
    _anim.SetBool("Flying", true);
    _anim.SetBool("Working", false);

    // fly straight up
    controller.Move(new Vector3(0, flySpeed * Time.deltaTime, 0));
  }

  public void ChangeState(WorkerBeeState newState) { currentState = newState; }

  /** Movement helpers */
  public void RotateToTask(bool rotateDown = false) {
    float singleStep = _rotationSpeed * Time.deltaTime;
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
}
