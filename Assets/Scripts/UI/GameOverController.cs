using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverController : MonoBehaviour {

  public TMP_Text totalBeesText;
  public TMP_Text totalQueensText;

  public TMP_Text totalDaysText;

  public TMP_Text totalHoneyText;
  public TMP_Text totalBeeswaxText;
  public TMP_Text totalRoyalJellyText;
  public TMP_Text totalPollenText;
  public TMP_Text totalNectarText;

  public TMP_Text totalHoneycombsText;

  public void SetTotalBees(int bees) { totalBeesText.text = bees.ToString(); }

  public void SetTotalQueen(int queens) {
    totalQueensText.text = queens.ToString();
  }

  public void SetTotalDays(int days) { totalDaysText.text = days.ToString(); }

  public void SetTotalResources(BeeResources resources) {
    totalBeeswaxText.text = resources.Beeswax.ToString();
    totalHoneyText.text = resources.Honey.ToString();
    totalNectarText.text = resources.Nectar.ToString();
    totalRoyalJellyText.text = resources.RoyalJelly.ToString();
    totalPollenText.text = resources.Pollen.ToString();
  }

  public void SetTotalHoneycombs(int count) {
    totalHoneycombsText.text = count.ToString();
  }
}
