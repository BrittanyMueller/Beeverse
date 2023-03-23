using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingUIController : StructureUIController {

  public TMP_Text buildingStructureName;

  // used to control progress for progress bar
  public RectTransform progressBarLimit;
  public RectTransform progressBar;

  // references to target honeycomb
  private Honeycomb _honeycomb;

  // Update is called once per frame
  void Update() { updateProgress(_honeycomb.buildProgress); }

  /**
   * Used to update the current progress of the building
   * shown in the menu
   * @param progress a percent as a float which should be clamped 0-1
   */
  void updateProgress(float progress) {
    var oldPos = progressBar.anchoredPosition;
    progressBar.SetInsetAndSizeFromParentEdge(
        RectTransform.Edge.Left, 0, progressBarLimit.sizeDelta.x * (progress));
    // progressBar.(RectTransform.Axis.Horizontal,
    //                                       progressBarLimit.sizeDelta.x *
    //                                           (progress));
    progressBar.ForceUpdateRectTransforms();
    // progressBar.anchoredPosition = oldPos;
  }

  public void Show(Honeycomb honeycomb) {

    // set title
    buildingStructureName.text = honeycomb.Name;

    _honeycomb = honeycomb;
    selectBeeCallback = _honeycomb.SetWorker;

    base.Show(honeycomb.bees);
  }

  public override void Hide() {
    base.Hide();
    _honeycomb = null;
  }
}
