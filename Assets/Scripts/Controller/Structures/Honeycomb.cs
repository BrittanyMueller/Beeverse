using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Honeycomb : MonoBehaviour {

  // honeycomb type num todo
  public List<Transform> workSpots;

  public enum HoneycombType:int {
    HoneyFactory,
    BeeswaxFactory,
    RoyalJellyFactory,
    BroodNest,
    QueenNest
  }

  public HoneycombType type;
  public bool built;
  public float buildProgress;

  // Start is called before the first frame update
  void Start() {}

  // Update is called once per frame
  void Update() {}
}
