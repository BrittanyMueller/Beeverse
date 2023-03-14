using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugController : MonoBehaviour {
  public GameObject debugMenu;
  public GameState state;

  // UI elements
  public TMP_InputField misInput;

  private bool _active = false;
  // Start is called before the first frame update
  void Start() {}

  // Update is called once per frame
  void Update() {}

  public void toggle() {
    _active = !_active;
    debugMenu.SetActive(_active);
  }

  public void setMIS() {
    try {
      GameState.minutesPreSecond = Int32.Parse(misInput.text);
    } catch (Exception) {
      // do nothing why did they give an invalid input...
    }
  }
}
