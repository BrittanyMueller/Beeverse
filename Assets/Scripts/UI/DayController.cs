using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayController : MonoBehaviour {
  public TMP_Text Day;

  public void UpdateDate(string newDate) {
    Day.text = newDate;
  }
}
