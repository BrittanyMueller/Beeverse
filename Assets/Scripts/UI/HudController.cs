using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
  public BroodNestUIController broodNestMenu;
  public SelectEggTypeController eggMenu;
  public BeeProfileController beeProfileController;
  public BabyProfileController babyProfileController;
  public TMP_Text populationText;

  public List<Button> createHoneycombButtons;
  private Button _createHoneyFactory;
  private Button _createBeeswaxFactory;
  private Button _createRoyalJellyFactory;
  private Button _createBroodNest;

  public GameState state;
  // Start is called before the first frame update
  private void Start() {
    menu.SetActive(false);
    gameOver.SetActive(false);
    CloseStructureMenu();

    foreach (Button button in createHoneycombButtons) {
      if (button.gameObject.name == "HoneyFactory") {
        _createHoneyFactory = button;
      } else if (button.gameObject.name == "BeeswaxFactory") {
        _createBeeswaxFactory = button;
      } else if (button.gameObject.name == "RoyalJellyFactory") {
        _createRoyalJellyFactory = button;
      } else if (button.gameObject.name == "BroodNest") {
        _createBroodNest = button;
      }
    }
  }

  private void Update() {
    // Set the value of the population
    populationText.text = state._bees.Count.ToString();
    
    // disable and enable buttons
    _createHoneyFactory.interactable = (state.pollenCount >= 100 && state.nectarCount >= 50);
    _createBeeswaxFactory.interactable = (state.honeyCount >= 100 && state.nectarCount >= 50);
    _createRoyalJellyFactory.interactable = (state.honeyCount >= 100 && state.beeswaxCount >= 100);
    _createBroodNest.interactable =
        (state.beeswaxCount >= 100 && state.honeyCount >= 50 && state.royalJellyCount >= 10);
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
    if (state.Paused)
      return;
    structureMenu.SetActive(false);
    flowerMenu.Hide();
    buildingMenu.Hide();
    factoryMenu.Hide();
    broodNestMenu.Hide();
  }

  public void OpenStructureMenu(StructureType type, MonoBehaviour structure) {
    if (state.Paused)
      return;
    // Hide all children
    flowerMenu.Hide();
    buildingMenu.Hide();
    factoryMenu.Hide();
    broodNestMenu.Hide();

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
      case StructureType.BroodNest:
        broodNestMenu.Show((BroodNest)structure);
        break;
    }
    structureMenu.SetActive(true);
  }
  public void Exit() {
    Application.Quit();
  }

  public void Save() {
    state.Save();
  }

  // Game is over display screen and hide other GUIs
  public void GameOver() {
    menu.SetActive(false);
    gameOver.SetActive(true);
  }
}
