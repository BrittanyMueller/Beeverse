using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {

  public List<Transform> flowerLocations;
  public List<WorkerBee> bees;

  private HudController _controller;

  public float NectarPerSecond = 1;
  public float PollenPerSecond = 1;

  // Start is called before the first frame update
  void Start() {
    foreach (var _ in flowerLocations) {
      bees.Add(null);
    }
    _controller = GameObject.Find("HudController").GetComponent<HudController>();
  }


  void OnMouseDown() {
    _controller.OpenStructureMenu(StructureType.Flower, this);

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
}
