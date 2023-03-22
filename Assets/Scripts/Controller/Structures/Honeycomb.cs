using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Honeycomb class will be responsible for any common behaviour
 * between the different types of honeycombs but will not
 * implement any behaviour specific to honeycombs
 * This includes onMouseDown() events as well as any specific UI
 * or update calls with the exception of the build Menu
 */
public class Honeycomb : MonoBehaviour {

  // honeycomb type num todo
  public List<Transform> workSpots;
  public List<WorkerBee> bees;

  // The structure in the center to show the hive type.
  // after it is built
  public GameObject structure;


  public enum HoneycombType : int {
    HoneyFactory,
    BeeswaxFactory,
    RoyalJellyFactory,
    BroodNest,
    QueenNest
  }

  public HoneycombType type;
  public string Name;
  public bool built {
    get { return _buildProgress >= 1; }
  }

  public float buildProgress {
    get {
      return _buildProgress;
    }
    set {
      _buildProgress = value;
      if (built) {
        ShowStructure();
      }
    }
  }
  private float _buildProgress = 0.0f;

  // TODO maybe this should be faster
  public float buildSpeedPerSecond = 0.1f;

  protected HudController _hudController;

  // Start is called before the first frame update
  void Start() {
    foreach (var _ in workSpots) {
      bees.Add(null);
    }

    // find the builderController in the scene
    _hudController = GameObject.Find("HudController").GetComponent<HudController>();
  }



  public virtual void SetWorker(WorkerBee bee, int index) {
    
    var oldBee = bees[index];
    if (oldBee != null) {
      oldBee.Task = null;
      oldBee.honeycomb = null;
    }
    bees[index] = bee;
    bee.honeycomb = this;

    // reset the bee task without trigging job change
    bee._task = null;

    // if the structure isn't built yet force them to be a builder
    if (!built) {
        bee.Task = new WorkerBeeTask(WorkerBeeTask.TaskType.Builder,
                                    workSpots[index].position, index);
    } 
  }

  public bool OpenBuildController() {
    if (!built) {
      _hudController.OpenStructureMenu(StructureType.Building, this);
      return true;
    } else {
      return false;
    }
  }

  public void ShowStructure() {
    if(structure != null) {
      structure.SetActive(true);
    }
  }
}
