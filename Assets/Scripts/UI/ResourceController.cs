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
    honeyText.text = ((int)resources.honey).ToString();
    nectarText.text = ((int)resources.nectar).ToString();
    royalJellyText.text = ((int)resources.royalJelly).ToString();
    pollenText.text = ((int)resources.pollen).ToString();
    beeswaxText.text = ((int)resources.beeswax).ToString();
  }
}
