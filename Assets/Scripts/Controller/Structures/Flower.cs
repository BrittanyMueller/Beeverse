using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Flower : MonoBehaviour {

  public List<Transform> flowerLocations;
  public List<WorkerBee> bees;

  private HudController _controller;

  public int conversionTimeMinutes = 5;
  public int pollenCollectionRate = 1;
  public int nectarCollectionRate = 3;

  // Start is called before the first frame update
  void Start() {
    foreach (var _ in flowerLocations) {
      bees.Add(null);
    }
    _controller =
        GameObject.Find("HudController").GetComponent<HudController>();
  }

  void OnMouseDown() {
    // make sure UI isn't on UI
    if (!EventSystem.current.IsPointerOverGameObject()) {
      _controller.OpenStructureMenu(StructureType.Flower, this);
    }
  }

  public void SetWorker(WorkerBee bee, int index) {
    var oldBee = bees[index];
    if (oldBee != null) {
      oldBee.Task = null;
    }

    bees[index] = bee;
    bee.Task = new WorkerBeeTask(WorkerBeeTask.TaskType.Forager,
                                 flowerLocations[index].position, index);
    bee.flower = this;
  }

  public void RemoveWorker(int index) { bees[index].Task = null; }
}
