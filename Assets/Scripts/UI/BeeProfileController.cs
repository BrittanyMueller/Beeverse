using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeeProfileController : MonoBehaviour {
  [SerializeField]
  private TMP_Text beeName;
  [SerializeField]
  private TMP_Text beeAge;
  [SerializeField]
  private TMP_Text beeJob;
  [SerializeField]
  private Image beeSelfie;

  private Sprite _queenBee;
  private Sprite _workerBee;

  // Start is called before the first frame update
  private void Awake() {
    // Load images of bees for sprite swapping
    _queenBee = Resources.Load<Sprite>("BeePics/queenSelfie");
    _workerBee = Resources.Load<Sprite>("BeePics/beeSelfie");
    gameObject.SetActive(false);
  }

  public void Hide() { gameObject.SetActive(false); }

  public void Show(Bee bee) {
    beeName.text = bee.beeName;
    beeAge.text = bee.AgeInDays + " day";
    if (bee.AgeInDays != 1)
      beeAge.text += "s";

    switch (bee.type) {
    case BeeType.Queen:
      beeJob.text = "Queen";
      beeSelfie.sprite = _queenBee;
      break;
    case BeeType.Worker:
      beeJob.text = ((WorkerBee)bee).JobTitle;
      beeSelfie.sprite = _workerBee;
      break;
    }
    gameObject.SetActive(true);
  }
}
