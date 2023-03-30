using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StructureUIController : MonoBehaviour {

  // common components for StructureUIControllers
  public GameState state;
  public HudController hudController;

  // GameObject to the selectbee screen which
  // can be used to select a given bee when the
  // beeslot is pressed
  public SelectBee selectBee;

  // Prefab for a beeslot which can either be empty
  // or contain a bee and its information
  // and is used to assign bees to a task
  public GameObject beeSlot;

  // vertical group layout that contains all the bee slots
  public GameObject beeList;

  // Callbacks which will be used when a bee is selected
  // which should be implemented by the child class
  protected Action<WorkerBee, int> selectBeeCallback;
  protected Action<int> removeBeeCallback;
  protected Action refreshCallback;

  // Each child is expected to implement a show and hide function
  public virtual void Hide() {
    selectBee.Hide();
    gameObject.SetActive(false);
  }

  protected void Show(List<WorkerBee> bees) {
    // create ui
    // Clear the current list
    {
      var children = new List<GameObject>();
      foreach (Transform child in beeList.transform)
        children.Add(child.gameObject);
      children.ForEach(child => Destroy(child));
    }

    // add worker bees to the top of the list
    for (int index = 0; index < bees.Count; index++) {
      if (bees[index] == null)
        continue;
      GameObject obj =
          Instantiate(beeSlot, new Vector3(0, 0, 0), Quaternion.identity);
      obj.transform.SetParent(beeList.transform);

      // set info about the bee.
      obj.GetComponentsInChildren<TMP_Text>()[0].text = bees[index].beeName;

      // set onclick with the index of the bee so we can backtrack which bee was
      // chosen Need a tmp variable because c# sucks
      int tmpIndex = index;
      obj.GetComponentsInChildren<Button>()[0].onClick.AddListener(
          () => { SelectBee(tmpIndex); });

      // callback for close button
      obj.GetComponentsInChildren<Button>()[1].onClick.AddListener(
          () => { RemoveBee(tmpIndex); });
    }

    // add empty spots to the bottom
    for (int index = 0; index < bees.Count; index++) {
      if (bees[index] != null)
        continue;
      GameObject obj =
          Instantiate(beeSlot, new Vector3(0, 0, 0), Quaternion.identity);
      obj.transform.SetParent(beeList.transform);

      // set info about the bee. If the spot is empty just make the text <EMPTY>
      obj.GetComponentsInChildren<TMP_Text>()[0].text = "<EMPTY>";

      // set onclick with the index of the bee so we can backtrack which bee was
      // chosen Need a tmp variable because c# sucks
      int tmpIndex = index;
      obj.GetComponentsInChildren<Button>()[0].onClick.AddListener(
          () => { SelectBee(tmpIndex); });

      // disable the button no bee is assigned
      obj.GetComponentsInChildren<Button>()[1].gameObject.SetActive(false);
    }

    gameObject.SetActive(true);
  }

  public void SelectBee(int index) {
    if (state.Paused) return;
    // Open up the select bee menu given the index
    // and a references to this so it can return
    selectBee.Show(index, (WorkerBee bee) => {
      if (selectBeeCallback != null) {
        selectBeeCallback(bee, index);
      } else {
        Debug.Log("ERROR: SelectBee callback not set");
      }
      refreshCallback();
    });
  }

  public void RemoveBee(int index) {
    if (state.Paused) return;
    // remove the bee with the given callback
    if (removeBeeCallback != null) {
      removeBeeCallback(index);
    } else {
      Debug.Log("ERROR: RemoveBee callback not set");
    }
    refreshCallback();
  }
}
