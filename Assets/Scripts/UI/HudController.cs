using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour {

  public GameObject menu;
  public GameObject credits;
  public GameState state;
  // Start is called before the first frame update
  void Start() {
    menu.SetActive(false);
    credits.SetActive(false);
  }

  public void Pause() {
    menu.SetActive(true);
    state.Paused = true;
  }

  public void Resume() {
    menu.SetActive(false);
    state.Paused = false;
  }

  public void Exit() { Application.Quit(); }

  public void Credits() { credits.SetActive(true); }

  public void CloseCredits() { credits.SetActive(false); }

  public void Save() { state.Save(); }
}
