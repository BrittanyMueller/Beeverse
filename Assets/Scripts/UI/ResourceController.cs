using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceController : MonoBehaviour {

  public TMP_Text honeyText;
  public TMP_Text nectarText;
  public TMP_Text royalJellyText;
  public TMP_Text pollenText;
  public TMP_Text beeswaxText;

  public void UpdateResources(BeeResources resources) {
    honeyText.text = resources.Honey.ToString();
    nectarText.text = resources.Nectar.ToString();
    royalJellyText.text = resources.RoyalJelly.ToString();
    pollenText.text = resources.Pollen.ToString();
    beeswaxText.text = resources.Beeswax.ToString();
  }
}
