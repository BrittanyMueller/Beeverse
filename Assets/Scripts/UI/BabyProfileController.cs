using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BabyProfileController : MonoBehaviour {
  [SerializeField]
  private TMP_Text beeName;
  [SerializeField]
  private TMP_Text beeAge;
  [SerializeField]
  private TMP_Text beeJob;
  [SerializeField]
  private Image beeSelfie;

  // used to control progress for progress bar
  [SerializeField]
  private RectTransform progressBarLimit;
  [SerializeField]
  private RectTransform progressBar;

  private BabyBee _babyBee;
  private Sprite _eggBeeSprite;
  private Sprite _babyBeeSprite;

  // Start is called before the first frame update
  private void Awake() {
    // Load images of bees for sprite swapping
    _eggBeeSprite = Resources.Load<Sprite>("BeePics/eggSelfie");
    _babyBeeSprite = Resources.Load<Sprite>("BeePics/larvaSelfie");
    gameObject.SetActive(false);
  }

  private void Update() {
    if (_babyBee) {
      Show(_babyBee);
    }
  }

  /**
   * Used to update the current progress of the building
   * shown in the menu
   * @param progress a percent as a float which should be clamped 0-1
   */
  private void UpdateProgress(float progress) {
    var oldPos = progressBar.anchoredPosition;
    progressBar.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0,
                                              progressBarLimit.sizeDelta.x * (progress));
    progressBar.ForceUpdateRectTransforms();
  }

  public void Hide() {
    gameObject.SetActive(false);
    _babyBee = null;
  }

  public void Show(BabyBee bee) {
    _babyBee = bee;
    beeName.text = bee.beeName;
    beeAge.text = bee.AgeInDays + " day";
    if (bee.AgeInDays != 1)
      beeAge.text += "s";

    if (bee.isEgg) {
      beeJob.text = "Professional Napper";
      beeSelfie.sprite = _eggBeeSprite;
    } else {
      beeJob.text = "Troublemaker";
      beeSelfie.sprite = _babyBeeSprite;
    }
    if (bee.isQueen) {
      // Overwrite baby job with queen
      beeJob.text = "Future Queen";
    }
    UpdateProgress(_babyBee.AgeInMinutes / (5.0f * 25 * 60));
    
    gameObject.SetActive(true);
  }
}
