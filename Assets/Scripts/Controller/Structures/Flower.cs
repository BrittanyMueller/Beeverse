using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {

  public List<Transform> flowerLocations;
  public List<WorkerBee> bees;

  public FlowerUIController _controller;

  // Start is called before the first frame update
  void Start() {
    foreach (var _ in flowerLocations) {
      bees.Add(null);
    }
  }

  // Update is called once per frame
  void Update() {}

  void OnMouseDown() {
    // todo close all other menus
    _controller.Show(this);
  }

  public void SetWorker(WorkerBee bee, int index) {
    var oldBee = bees[index];
    if (oldBee != null) {
        bee.Task = null;
      // tell be to drop task
    }

    bees[index] = bee;

    bee.Task = new WorkerBeeTask(WorkerBeeTask.TaskType.Forager,
                                 flowerLocations[index].position);
  }
}
