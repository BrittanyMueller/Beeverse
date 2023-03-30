using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SelectBeeInfo : MonoBehaviour {

  // holds the bee that this panel is about
  public WorkerBee bee;
  public int targetSelect;
  // Start is called before the first frame update
  void Start() { 
      if (bee != null) {
        gameObject.GetComponentsInChildren<TMP_Text>()[0].text = bee.beeName;
        gameObject.GetComponentsInChildren<TMP_Text>()[1].text =
            bee.AgeInDays + " days old";
          
      }
  }
}
