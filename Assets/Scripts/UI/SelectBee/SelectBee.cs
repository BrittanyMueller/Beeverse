using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;

public class SelectBee : MonoBehaviour {
  // Prefab containing details about the bee
  // as well as a button which can be used to select the bee
  public GameObject beeDetails;

  // Vertical layout group object that
  // holds all the bees that can be selected
  public GameObject beeList;

  private GameState _state;

  /**
   * Shows A list of current bees that can be used for the job
   * If a bee is selected then the callback will be triggered with the chosen
   * bee as the arg.
   */
  public void Show(int index, Action<WorkerBee> callback) {
    _state = GameObject.Find("GameState").GetComponent<GameState>();

    // Clear the current list
    {
      var children = new List<GameObject>();
      foreach (Transform child in beeList.transform) children.Add(child.gameObject);
      children.ForEach(child => Destroy(child));
    }

    // get the game state we we can get a list of bees
    var bees = _state._bees;
    foreach (var bee in bees) {
      // if they have a task skip
      if (bee.HasTask)
        continue;
      GameObject obj = Instantiate(beeDetails, new Vector3(0, 0, 0), Quaternion.identity);
      obj.transform.SetParent(beeList.transform, false);

      // Set the bee and select
      var info = obj.GetComponent<SelectBeeInfo>();
      info.bee = bee;
      info.targetSelect = index;

      obj.GetComponentsInChildren<Button>()[0].onClick.AddListener(() => {
        if (_state.Paused)
          return;
        callback(bee);
        Hide();
      });
    }
    gameObject.SetActive(true);
  }

  /**
   * Hides the Select BeeScreen
   */
  public void Hide() {
    if (_state == null)
      _state = GameObject.Find("GameState").GetComponent<GameState>();
    if (_state.Paused)
      return;
    // Clear the current list
    {
      var children = new List<GameObject>();
      foreach (Transform child in beeList.transform) children.Add(child.gameObject);
      children.ForEach(child => Destroy(child));
    }

    gameObject.SetActive(false);
  }
}
