using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BroodNestUIController : StructureUIController {

    [SerializeField] private TMP_Text growthModifier;
    private BroodNest _broodNest;
    
    public void Show(BroodNest honeycomb) {
        _broodNest = honeycomb;
        selectBeeCallback = _broodNest.SetWorker;
        removeBeeCallback = _broodNest.RemoveWorker;
        refreshCallback = Refresh;

        var beeCount = 0;
        foreach (var bee in honeycomb.bees) {
            if (bee != null) {
                beeCount += 1;
            }
        }
        growthModifier.text = beeCount * 50 + "%";
        base.Show(honeycomb.bees);
    }

    public override void Hide() {
        base.Hide();
        _broodNest = null;
    }

    private void Refresh() { Show(_broodNest); }
}
