using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Honeycomb class will be responsible for any common behaviour
 * between the different types of honeycombs but will not
 * implement any behaviour specific to honeycombs
 * This includes onMouseDown() events as well as any specific UI
 * or update calls with the exception of the build Menu
 */
public class Honeycomb : MonoBehaviour {
  public List<Transform> workSpots;
  public List<WorkerBee> bees;

  // The structure in the center to show the hive type.
  // after it is built
  public GameObject structure;

  public StructureType honeycombType;
  public string Name {
    get {
      switch (honeycombType) {
        case StructureType.HoneyFactory:
          return "Honey Factory";
        case StructureType.BeeswaxFactory:
          return "Beeswax Factory";
        case StructureType.RoyalJellyFactory:
          return "Royal Jelly Factory";
        case StructureType.BroodNest:
          return "Brood Nest";
        case StructureType.QueenNest:
          return "Queen Nest";
        default:
          return "INVALID NAME";
      }
    }
  }

  public bool Built => _buildProgress >= 1;

  public bool showedStructure = false;

  public float BuildProgress {
    get => _buildProgress;
    set {
      _buildProgress = value;
      if (Built && !showedStructure) {
        showedStructure = true;
        ShowStructure();
      }
    }
  }
  private float _buildProgress = 0.0f;
  public float buildSpeedPerSecond = 0.1f;

  private HudController _hudController;
  private GameState _state;

  // Start is called before the first frame update
  private void Start() {
    foreach (var _ in workSpots) {
      bees.Add(null);
    }

    // find the hudController in the scene
    GameObject controller = GameObject.Find("HudController");
    if (controller != null) {
      _hudController = controller.GetComponent<HudController>();
    }
    GameObject stateObj = GameObject.Find("GameState");
    if (stateObj != null) {
      _state = stateObj.GetComponent<GameState>();
    }

    // Structures should start unbuilt so hide
    // the structure if it exists
    if (structure != null) {
      structure.SetActive(false);
    }
  }

  /**
   * Sets the works to a specific spot in the honeycomb
   * this will also check if the structure is built
   * and if it isn't it will automatilly set them
   * as a builder
   */
  public void SetWorker(WorkerBee bee, int index) {
    var oldBee = bees[index];
    if (oldBee) {
      oldBee.Task = null;
      oldBee.honeycomb = null;
    }
    bees[index] = bee;
    bee.honeycomb = this;

    // Set Their task to the honeycomb
    bee.Task = new WorkerBeeTask(WorkerBeeTask.TaskType.Builder, workSpots[index].position, index,
                                 structure.transform.position);

    // If the honeycomb is already built update their role
    if (Built) {
      bee.Task.taskType = WorkerBeeTask.StructureTypeAsTaskType(honeycombType);
    }
  }

  /**
   * Removes a worker from a current index if it exists
   */
  public void RemoveWorker(int index) {
    if (bees[index]) {
      bees[index].Task = null;
    }
  }

  /**
   * Called when construction is finished to
   * created the centre structure
   */
  private void ShowStructure() {
    if (!structure)
      return;
    structure.SetActive(true);
    _state.UpdateLog("> The " + Name + " has finished building!");
    if (honeycombType == StructureType.BroodNest) {
      // Add the egg slots to the GameState so Queen knows to lay eggs
      foreach (Transform child in this.transform) {
        if (child.CompareTag("BabySlot"))
          _state._beeEggSlots.Add(child.gameObject.GetComponent<BeeEggSlot>());
      }
    }
  }

  /**
   * Opens the controller for the honeycomb if the honeycomb isn't built
   * yet it will instead open the building menu
   */
  private void OnMouseDown() {
    // make sure UI isn't on UI and this isn't the queen honeycomb
    if (EventSystem.current.IsPointerOverGameObject() || honeycombType == StructureType.QueenNest)
      return;

    // Show build progress menu or completed structure menu if built
    _hudController.OpenStructureMenu(!Built ? StructureType.Building : honeycombType, this);
  }
}
