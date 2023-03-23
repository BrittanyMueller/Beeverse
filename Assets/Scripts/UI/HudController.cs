using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour {

  public GameObject menu;
  public GameObject credits;
  public GameObject gameOver;

  // Menus for structures
  public GameObject structureMenu;
  public FlowerUIController flowerMenu;
  public BuildingUIController buildingMenu;

  public GameState state;
  // Start is called before the first frame update
  void Start() {
    menu.SetActive(false);
    credits.SetActive(false);
    gameOver.SetActive(false);
    CloseStructureMenu();
  }

  public void Pause() {
    menu.SetActive(true);
    state.Paused = true;
  }

  public void Resume() {
    menu.SetActive(false);
    state.Paused = false;
  }

  public void CloseStructureMenu() {
    structureMenu.SetActive(false);
    flowerMenu.Hide();
    buildingMenu.Hide();
  }

  public void OpenStructureMenu(StructureType type, MonoBehaviour structure) {
    // Hide all children
    flowerMenu.Hide();
    buildingMenu.Hide();

    // Open the correct menu
    switch (type) {
    case StructureType.Flower:
      flowerMenu.Show((Flower)structure);
      break;
    case StructureType.Building:
      buildingMenu.Show((Honeycomb)structure);
      break;
    }

    structureMenu.SetActive(true);
  }
  public void Exit() { Application.Quit(); }

  public void Credits() { credits.SetActive(true); }

  public void CloseCredits() { credits.SetActive(false); }

  public void Save() { state.Save(); }

  // Game is over display screen and hide other GUIs
  public void GameOver() {
    credits.SetActive(false);
    menu.SetActive(false);
    gameOver.SetActive(true);
  }
}
