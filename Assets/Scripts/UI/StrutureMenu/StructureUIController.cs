using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StructureUIController : MonoBehaviour {

  // common components for StructureUIControllers
  public GameState state;
  public SelectBee selectBee;
  public GameObject beeSlot;
  public GameObject beeList;
  public HudController hudController;

  // Callback which will be used when a bee is selected
  // which should be implemented by the child class

  protected Action<WorkerBee, int> selectBeeCallback;

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

    for (int index = 0; index < bees.Count; index++) {
      GameObject obj =
          Instantiate(beeSlot, new Vector3(0, 0, 0), Quaternion.identity);
      obj.transform.SetParent(beeList.transform);

      // set info about the bee. If the spot is empty just make the text <EMPTY>
      obj.GetComponentsInChildren<TMP_Text>()[0].text =
          (bees[index] != null) ? bees[index].beeName : "<EMPTY>";

      // set onclick with the index of the bee so we can backtrack which bee was
      // chosen Need a tmp variable because c# sucks
      int tmpIndex = index;
      obj.GetComponentsInChildren<Button>()[0].onClick.AddListener(
          () => { SelectBee(tmpIndex); });
    }

    gameObject.SetActive(true);
  }

  public void SelectBee(int index) {
    // Open up the select bee menu given the index
    // and a references to this so it can return
    selectBee.Show(index, (WorkerBee bee) => {
      if (selectBeeCallback != null) {
        selectBeeCallback(bee, index);
      } else {
        Debug.Log("ERROR: SelectBee callback not set");
      }
      hudController.CloseStructureMenu();
    });
  }
}
