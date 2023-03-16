using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

  public GameObject credits;
  // Start is called before the first frame update
  void Start() { credits.SetActive(false); }

  public void StartGame() {
    // Make sure game isn't paused
    Time.timeScale = 1;
    SceneManager.LoadScene("Beeverse");
  }

  public void Credits() { credits.SetActive(true); }
  public void CloseCredits() { credits.SetActive(false); }

  public void Exit() { Application.Quit(); }
}
