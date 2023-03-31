using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BabyProfileController : MonoBehaviour
{
  [SerializeField]
  private TMP_Text beeName;
  [SerializeField]
  private TMP_Text beeAge;
  [SerializeField]
  private TMP_Text beeJob;
  [SerializeField]
  private Image beeSelfie;

  private Sprite _eggBee;
  private Sprite _babyBee;

  // Start is called before the first frame update
  private void Awake() {
    // Load images of bees for sprite swapping
    // TODO get pic of baby egg
    _eggBee = Resources.Load<Sprite>("BeePics/queenSelfie");
    _babyBee = Resources.Load<Sprite>("BeePics/larvaSelfie");
    gameObject.SetActive(false);
  }

  public void Hide() { gameObject.SetActive(false); }

  public void Show(BabyBee bee) {
    beeName.text = bee.beeName;
    beeAge.text = bee.AgeInDays + " day";
    if (bee.AgeInDays != 1)
      beeAge.text += "s";

    if (bee.isEgg) {
      // TODO this job title sucks
      beeJob.text = "Professional Napper";
      beeSelfie.sprite = _eggBee;
    } else {
      beeJob.text = "Troublemaker";
      beeSelfie.sprite = _babyBee;
    }
    if (bee.isQueen) {
      // Overwrite baby job with queen
      beeJob.text = "Future Queen";
    }
    gameObject.SetActive(true);
  }
}
