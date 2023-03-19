using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneycombGenerator : MonoBehaviour {

  public GameObject honeycomb;
  public GameState state;

  // Special bool that is used to autogenerate honeycombs
  // if it is in the menu
  public bool inMenu = false;
  private List<GameObject> _menuGeneratedHoneycombs = new List<GameObject>();

  private float _verticalOffset;
  private float _horizontalOffset;

  public List<Vector3> openList = new List<Vector3>();
  private List<Vector3> closedList = new List<Vector3>();

  // Start is called before the first frame update
  void Start() {
    var size = honeycomb.GetComponent<BoxCollider>().bounds.size;
    _verticalOffset = size.x;
    var hexBase =
        _verticalOffset / Mathf.Sqrt(3); // Calculate by special triangle
    _horizontalOffset = hexBase + (size.z - hexBase) / 2;

    closedList.Add(honeycomb.transform.position);
    GenerateAvailablePositions(honeycomb.transform.position);
    Debug.Log(honeycomb.transform.position.x);
    // test generation
    StartCoroutine(RandomGeneration());
  }

  // Update is called once per frame
  void Update() {}

  IEnumerator RandomGeneration() {
    var rand = new System.Random();
    while (true) {
      yield return new WaitForSeconds(1);
      CreateHoneycomb(openList[rand.Next(0, openList.Count)]);
    }
  }

  private void GenerateAvailablePositions(Vector3 position) {

    var possibleSpaces = new List<Vector3>() {
      new(position.x + _verticalOffset, position.y,
          position.z), // Space vertically above
      new(position.x - _verticalOffset, position.y,
          position.z), // Space vertically below
      new(position.x + _verticalOffset / 2, position.y,
          position.z + _horizontalOffset), // Above to right space
      new(position.x - _verticalOffset / 2, position.y,
          position.z + _horizontalOffset), // Below to right space
      new(position.x + _verticalOffset / 2, position.y,
          position.z - _horizontalOffset), // Above to left space
      new(position.x - _verticalOffset / 2, position.y,
          position.z + _horizontalOffset), // Below to left space
    };

    foreach (var space in possibleSpaces) {
      // Add new space to available list if not already found/used
      var existsInOpen =
          openList.Exists((value) => (Mathf.Abs(value.x - space.x) < 0.001) &&
                                     Mathf.Abs(value.z - space.z) < 0.001);
      var existsInClosed =
          closedList.Exists((value) => (Mathf.Abs(value.x - space.x) < 0.001) &&
                                       Mathf.Abs(value.z - space.z) < 0.001);
      if (!existsInOpen && !existsInClosed) {
        openList.Add(space);
      }
    }
  }

  private void CreateHoneycomb(Vector3 offset) { // take type of honeycombs
    var pos = honeycomb.transform.position;
    GameObject newHoneycomb =
        Instantiate(honeycomb, offset, honeycomb.transform.rotation);
    GenerateAvailablePositions(offset);
    closedList.Add(offset);
    openList.Remove(offset);

    // if we are in menu add it to the honeycomb list
    // else add it to the gamestate
    if (inMenu) {
      _menuGeneratedHoneycombs.Add(newHoneycomb);

      if (_menuGeneratedHoneycombs.Count > 100) {
        // reset everything after 100 have been generated
        openList.Clear();
        closedList.Clear();
        GenerateAvailablePositions(honeycomb.transform.position);
        while (_menuGeneratedHoneycombs.Count != 0) {
          Destroy(_menuGeneratedHoneycombs[0]);
          _menuGeneratedHoneycombs.RemoveAt(0);
        }
      }
    } else {
      state._honeycombs.Add(newHoneycomb.GetComponent<Honeycomb>());
    }
  }
}
