using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneycombHint : MonoBehaviour {

  public StructureType type;
  public GameState state;
  private void OnMouseDown() { state.CreateHoneycomb(transform.position, type); }
}
