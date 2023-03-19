using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FlowerUIController : MonoBehaviour {
  // Start is called before the first frame update

  public GameState state;
  public SelectBee selectBee;
  public GameObject beeSlot;

  public GameObject beeList;

  private Flower _curPatch;

  // todo link to a flower patch

  void Start() { gameObject.SetActive(false); }

  // Update is called once per frame
  void Update() {}

  public void Show(Flower patch) {

    _curPatch = patch;
    // create ui
    // Clear the current list
    {
      var children = new List<GameObject>();
      foreach (Transform child in beeList.transform)
        children.Add(child.gameObject);
      children.ForEach(child => Destroy(child));
    }

    int index = 0;
    // readd list
    foreach (WorkerBee bee in _curPatch.bees) {

      GameObject obj =
          Instantiate(beeSlot, new Vector3(0, 0, 0), Quaternion.identity);
      obj.transform.SetParent(beeList.transform);

      // set info
      obj.GetComponentsInChildren<TMP_Text>()[0].text =
          (bee != null) ? bee.beeName : "<EMPTY>";
      // set onclick
      obj.GetComponentsInChildren<Button>()[0].onClick.AddListener(
          () => {
            int newIndex = index;
            SelectBee(_curPatch.bees.IndexOf(bee));
    });
    }

    gameObject.SetActive(true);
  }

  public void Hide() {
    _curPatch = null;
    gameObject.SetActive(false);
  }

  public void SelectBee(int index) {
    Debug.Log(index);
    // Open up the select bee menu given the index
    // and a references to this so it can return
    selectBee.Show(index, (WorkerBee bee) => {
      _curPatch.SetWorker(bee, index);
      Hide();
    });
  }
}
