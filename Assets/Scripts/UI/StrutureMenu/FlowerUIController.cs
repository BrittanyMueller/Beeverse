using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FlowerUIController : StructureUIController {
  // Start is called before the first frame update

  public TMP_Text conversionText;

  private Flower _curPatch;

  public void Show(Flower patch) {

    // set flower patch and callback
    _curPatch = patch;
    selectBeeCallback = _curPatch.SetWorker;
    removeBeeCallback = _curPatch.RemoveWorker;

    // Set text for resource collection speeds
    conversionText.text = (_curPatch.ConversionTimeMinutes).ToString();

    base.Show(patch.bees);
  }

  public override void Hide() {
    base.Hide();
    _curPatch = null;
  }
}
