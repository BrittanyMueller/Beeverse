using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogController : MonoBehaviour {
  public TMP_Text LogContent;
  public ScrollRect logScroll;

  // Borrowed from
  // https://stackoverflow.com/questions/47613015/how-do-i-get-a-unity-scroll-rect-to-scroll-to-the-bottom-after-the-contents-rec
  IEnumerator ApplyScrollPosition(ScrollRect sr, float verticalPos) {
    yield return new WaitForEndOfFrame();
    var delta = sr.verticalNormalizedPosition - verticalPos;
    if (verticalPos < 10) {
      sr.verticalNormalizedPosition = verticalPos;
    } else {
      sr.verticalNormalizedPosition = verticalPos - delta;
    }
    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)sr.transform);
  }

  public void UpdateLog(string update) {
    // New line to setup for next update message
    LogContent.text += update + "\n";

    var oldScroll = logScroll.verticalNormalizedPosition;
    StartCoroutine(ApplyScrollPosition(logScroll, oldScroll));
  }
}
