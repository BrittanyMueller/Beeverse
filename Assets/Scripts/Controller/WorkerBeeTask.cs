using UnityEngine;
using System.Collections;

public class WorkerBeeTask {
  public string taskName;
  public Vector3 taskLocation;

  public WorkerBeeTask(string name, Vector3 location) {
    taskName = name;
    taskLocation = location;
  }
}