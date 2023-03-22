using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;

public class SelectBee : MonoBehaviour {

  public GameObject beeDetails;

  public GameObject beeList;

  void Start() {
    gameObject.SetActive(false);
  }

  /**
   * Shows A list of current bees that can be used for the job
   * If a bee is selected then the callback will be triggered with the chosen
   * bee as the arg.
   */
  public void Show(int index, Action<WorkerBee> callback) {

    // Clear the current list
    {
      var children = new List<GameObject>();
      foreach (Transform child in beeList.transform)
        children.Add(child.gameObject);
      children.ForEach(child => Destroy(child));
    }

    // get the game state we we can get a list of bees
    var bees = GameObject.Find("GameState").GetComponent<GameState>()._bees;

    foreach (WorkerBee bee in bees) {
      GameObject obj =
          Instantiate(beeDetails, new Vector3(0, 0, 0), Quaternion.identity);
      obj.transform.SetParent(beeList.transform);

      // Set the bee and select
      SelectBeeInfo info = obj.GetComponent<SelectBeeInfo>();
      info.bee = bee;
      info.targetSelect = index;

      obj.GetComponentsInChildren<Button>()[0].onClick.AddListener(() => {
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
    // Clear the current list
    {
      var children = new List<GameObject>();
      foreach (Transform child in beeList.transform)
        children.Add(child.gameObject);
      children.ForEach(child => Destroy(child));
    }

    gameObject.SetActive(false);
  }
}
