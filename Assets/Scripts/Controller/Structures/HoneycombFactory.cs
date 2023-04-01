using System.Collections;
using System;

using System.Collections.Generic;
using UnityEngine;

public class HoneycombFactory : Honeycomb {
  // Resources IO conversions
  public List<BeeResources.Type> inResources;
  public List<int> inResourcesConversionRate;
  public BeeResources.Type outResource;
  public int outResourceCollectionRate = 1;

  // How long it takes to make outResourceCollectionRate of the outResource
  public int conversionTimeMinutes;

  /** Abstraction of adding the resource of any given honeycomb
   * to the game state
   */
  public void AddResource(GameState state) {
    switch (outResource) {
      case BeeResources.Type.Beeswax:
        state.AddBeeswax(outResourceCollectionRate);
        break;
      case BeeResources.Type.Honey:
        state.AddHoney(outResourceCollectionRate);
        break;
      case BeeResources.Type.Nectar:
        state.AddNectar(outResourceCollectionRate);
        break;
      case BeeResources.Type.Pollen:
        state.AddPollen(outResourceCollectionRate);
        break;
      case BeeResources.Type.RoyalJelly:
        state.AddRoyalJelly(outResourceCollectionRate);
        break;
    }
  }

  /**
   * Tests if a bee can consume the needed resources to generate the
   * outputResource if it can the resources will be consumed by the GameState
   */
  public bool ConsumeResources(GameState state) {
    BeeResources res = new BeeResources();

    // Calculate the needed resources and package it up for game state
    for (int i = 0; i < inResources.Count && i < inResourcesConversionRate.Count; i++) {
      switch (inResources[i]) {
        case BeeResources.Type.Beeswax:
          res.Beeswax = inResourcesConversionRate[i];
          break;
        case BeeResources.Type.Honey:
          res.Honey = inResourcesConversionRate[i];
          break;
        case BeeResources.Type.Nectar:
          res.Nectar = inResourcesConversionRate[i];
          break;
        case BeeResources.Type.RoyalJelly:
          res.RoyalJelly = inResourcesConversionRate[i];
          break;
        case BeeResources.Type.Pollen:
          res.Pollen = inResourcesConversionRate[i];
          break;
      }
    }

    // return if the resources got consumed
    return state.ConsumeResources(res);
  }

  public string CostToString() {
    string str = "";

    for (int i = 0; i < inResources.Count && i < inResourcesConversionRate.Count; i++) {
      if (i != 0)
        str += " + ";

      str += ResourceToString(inResources[i], inResourcesConversionRate[i].ToString());
    }
    return str + " -> ".ToString() + " " + ResourceToString(outResource, outResourceCollectionRate.ToString());
  }

  private string ResourceToString(BeeResources.Type type, string amount) {
    switch (type) {
      case BeeResources.Type.Beeswax:
        return amount + " Beeswax";
      case BeeResources.Type.Honey:
        return amount + " Honey";
      case BeeResources.Type.Nectar:
        return amount + " Nectar";
      case BeeResources.Type.RoyalJelly:
        return amount + " Royal Jelly";
      case BeeResources.Type.Pollen:
        return amount + " Pollen";
      default:
        return "Unknown";
    }
  }
}
