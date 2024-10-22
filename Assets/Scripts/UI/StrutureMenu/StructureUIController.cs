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

  private Sprite _beeSelfie;
  private Sprite _beeShadow;

  // Each child is expected to implement a show and hide function
  public virtual void Hide() {
    selectBee.Hide();
    gameObject.SetActive(false);
  }

  protected void Show(List<WorkerBee> bees) {
    if (_beeShadow == null) {
      // Load images of bees for sprite swapping
      _beeSelfie = Resources.Load<Sprite>("BeePics/beeSelfie_transparent");
      _beeShadow = Resources.Load<Sprite>("BeePics/beeSelfie_shadow");
    }
    // create ui
    // Clear the current list
    {
      var children = new List<GameObject>();
      foreach (Transform child in beeList.transform) children.Add(child.gameObject);
      children.ForEach(child => Destroy(child));
    }

    // add worker bees to the top of the list
    for (int index = 0; index < bees.Count; index++) {
      if (bees[index] == null)
        continue;
      GameObject obj = Instantiate(beeSlot, new Vector3(0, 0, 0), Quaternion.identity);
      obj.transform.SetParent(beeList.transform, false);

      // Set bee name and active profile pic
      obj.GetComponentsInChildren<TMP_Text>()[0].text = bees[index].beeName;
      obj.transform.Find("Frame/BeeSelfie").GetComponent<Image>().sprite = _beeSelfie;
      // obj.GetComponentsInChildren<Image>()[0].GetComponentsInChildren<Image>()[0].sprite
      // = _beeSelfie;

      // set onclick with the index of the bee so we can backtrack which bee was
      // chosen Need a tmp variable because c# sucks
      int tmpIndex = index;
      obj.GetComponentsInChildren<Button>()[0].onClick.AddListener(() => { SelectBee(tmpIndex); });

      // callback for close button
      obj.GetComponentsInChildren<Button>()[1].onClick.AddListener(() => { RemoveBee(tmpIndex); });

    }

    // add empty spots to the bottom
    for (int index = 0; index < bees.Count; index++) {
      if (bees[index] != null)
        continue;
      GameObject obj = Instantiate(beeSlot, new Vector3(0, 0, 0), Quaternion.identity);
      obj.transform.SetParent(beeList.transform, false);

      // set info about the bee. If the spot is empty just make the text <EMPTY>
      // obj.GetComponentsInChildren<Image>()[0].sprite = _beeShadow;
      obj.transform.Find("Frame/BeeSelfie").GetComponent<Image>().sprite = _beeShadow;
      obj.GetComponentsInChildren<TMP_Text>()[0].text = "<EMPTY>";

      // set onclick with the index of the bee so we can backtrack which bee was
      // chosen Need a tmp variable because c# sucks
      int tmpIndex = index;
      obj.GetComponentsInChildren<Button>()[0].onClick.AddListener(() => { SelectBee(tmpIndex); });

      // disable the button no bee is assigned
      obj.GetComponentsInChildren<Button>()[1].gameObject.SetActive(false);

      // obj.GetComponent<RectTransform>().localScale = new Vector3(1, 1f, 1f);
    }

    gameObject.SetActive(true);
  }

  public void SelectBee(int index) {
    if (state.Paused)
      return;
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

  private void RemoveBee(int index) {
    if (state.Paused)
      return;
    // remove the bee with the given callback
    if (removeBeeCallback != null) {
      removeBeeCallback(index);
    } else {
      Debug.Log("ERROR: RemoveBee callback not set");
    }
    refreshCallback();
  }
}
