using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SelectBeeInfo : MonoBehaviour,
                             IPointerEnterHandler,
                             IPointerExitHandler {

  // holds the bee that this panel is about
  public WorkerBee bee;
  public int targetSelect;
  // Start is called before the first frame update
  void Start() {

    gameObject.GetComponentsInChildren<TMP_Text>()[0].text =
        bee.beeName + " - " + bee.lifeSpanInDays + " Days" + "\n" +
        bee.jobTitle;
  }

  // Update is called once per frame
  void Update() {}

  public void OnPointerEnter(PointerEventData eventData) {}

  public void OnPointerExit(PointerEventData eventData) {
    // disable bee info ui
  }
}
