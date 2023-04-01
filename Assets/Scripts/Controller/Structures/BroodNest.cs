using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BroodNest : Honeycomb {
  [SerializeField]
  private Material queenPillow;
  [SerializeField]
  private Material workerPillow;

  public void SetWorkerPillow(int index) {
    var materials = transform.Find("BabyBeds").GetComponent<Renderer>().materials;
    materials[index + 4] = workerPillow;
    transform.Find("BabyBeds").GetComponent<Renderer>().materials = materials;
  }

  public void SetQueenPillow(int index) {
    var materials = transform.Find("BabyBeds").GetComponent<Renderer>().materials;
    materials[index + 4] = queenPillow;
    transform.Find("BabyBeds").GetComponent<Renderer>().materials = materials;
  }
}
