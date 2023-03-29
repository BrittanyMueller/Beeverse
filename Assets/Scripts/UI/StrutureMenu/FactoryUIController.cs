using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FactoryUIController : StructureUIController {

  public TMP_Text factoryNameText;
  public TMP_Text minutesPerResourceText;
  public TMP_Text costText;

  private HoneycombFactory _curFactory;

  public void Show(HoneycombFactory factory) {

    // set factory and callback
    _curFactory = factory;
    selectBeeCallback = _curFactory.SetWorker;
    removeBeeCallback = _curFactory.RemoveWorker;

    // Set text for resource collection speeds
    factoryNameText.text = _curFactory.Name;

    costText.text = _curFactory.CostToString();
    minutesPerResourceText.text = _curFactory.conversionTimeMinutes.ToString();

    // todo set production time text and cost text
    base.Show(factory.bees);
  }

  public override void Hide() {
    base.Hide();
    _curFactory = null;
  }
}