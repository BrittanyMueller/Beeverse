using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** Responsible for controller what
 * pops up and is closed for the UI of the
 * entire application
 */
public class HudController : MonoBehaviour {

  public GameObject menu;
  public GameObject gameOver;

  // Menus for structures
  public GameObject structureMenu;
  public FlowerUIController flowerMenu;
  public BuildingUIController buildingMenu;
  public FactoryUIController factoryMenu;
  public BeeProfileController beeProfileController;

  public List<Button> createHoneycombButtons;
  private Button createHoneyFactory;
  private Button createBeeswaxFactory;
  private Button createRoyalJellyFactory;
  private Button createBroodNest;

  public GameState state;
  // Start is called before the first frame update
  void Start() {
    menu.SetActive(false);
    gameOver.SetActive(false);
    CloseStructureMenu();

    foreach (Button button in createHoneycombButtons) {
      if (button.gameObject.name == "HoneyFactory") {
        createHoneyFactory = button;
      } else if (button.gameObject.name == "BeeswaxFactory") {
        createBeeswaxFactory = button;
      } else if (button.gameObject.name == "RoyalJellyFactory") {
        createRoyalJellyFactory = button;
      } else if (button.gameObject.name == "BroodNest") {
        createBroodNest = button;
      }
    }
  }

  void Update() {

    // disable and enable buttons
    createHoneyFactory.interactable =
        (state.pollenCount >= 100 && state.nectarCount >= 50);
    createBeeswaxFactory.interactable =
        (state.honeyCount >= 100 && state.nectarCount >= 50);
    createRoyalJellyFactory.interactable =
        (state.honeyCount >= 100 && state.beeswaxCount >= 100);
    createBroodNest.interactable =
        (state.beeswaxCount >= 100 && state.honeyCount >= 50 &&
         state.royalJellyCount >= 10);
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
    if (state.Paused) return;
    structureMenu.SetActive(false);
    flowerMenu.Hide();
    buildingMenu.Hide();
    factoryMenu.Hide();
  }

  public void OpenStructureMenu(StructureType type, MonoBehaviour structure) {
    if (state.Paused) return;
    // Hide all children
    flowerMenu.Hide();
    buildingMenu.Hide();
    factoryMenu.Hide();

    // Open the correct menu
    switch (type) {
    case StructureType.Flower:
      flowerMenu.Show((Flower)structure);
      break;
    case StructureType.Building:
      buildingMenu.Show((Honeycomb)structure);
      break;
    case StructureType.HoneyFactory:
    case StructureType.BeeswaxFactory:
    case StructureType.RoyalJellyFactory:
      factoryMenu.Show((HoneycombFactory)structure);
      break;
    }
    structureMenu.SetActive(true);
  }
  public void Exit() { Application.Quit(); }

  public void Save() { state.Save(); }

  // Game is over display screen and hide other GUIs
  public void GameOver() {
    menu.SetActive(false);
    gameOver.SetActive(true);
  }
}
