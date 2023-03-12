using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum BeeType { Queen, Worker }
public class Bee : MonoBehaviour {

  public String beeName;
  public BeeType type;

  // Used to keep track of how much life
  // the be has left should be set by child
  // in start
  protected TimeTracker _lifeSpan;
  public int lifeSpanInDays;

  // Status of the bee needed for gamestate
  public bool isDead;

  // todo add role

  // Objects used for its dead state
  public GameObject _leftEye;
  public GameObject _rightEye;
  public GameObject _smile;
  public GameObject _leftEyeDead;
  public GameObject _rightEyeDead;
  public GameObject _frown;

  private Animator _anim;

  // Start is called before the first frame update
  protected virtual void Start() {
    _anim = gameObject.GetComponent<Animator>();
    _lifeSpan = new TimeTracker(lifeSpanInDays, 0, 0);
  }

  // Update is called once per frame
  protected virtual void FixedUpdate() {}

  public void UpdateTimeTick(int minutes) {
    _lifeSpan.SubMinutes(minutes);

    if (_lifeSpan.day < 0 && !isDead) {
      Die();
      isDead = true;
    }
  }

  public void Die() {
    _anim.SetBool("Flying", false);
    _anim.SetBool("Die", true);

    Debug.Log("Died");
    _smile.SetActive(false);
    _leftEye.SetActive(false);
    _rightEye.SetActive(false);

    _frown.SetActive(true);
    _leftEyeDead.SetActive(true);
    _rightEyeDead.SetActive(true);
  }
}
