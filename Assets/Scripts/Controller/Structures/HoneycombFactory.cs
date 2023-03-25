using System.Collections;
using System;

using System.Collections.Generic;
using UnityEngine;

public class HoneycombFactory : Honeycomb {

  // Resources In  conversions
  public List<BeeResources.Type> inResources;
  public List<int> inResourcesConversionRate;
  public BeeResources.Type outResource;
  public int conversionTimeMinutes;

  public void AddResource(GameState state) {
    switch (outResource) {
    case BeeResources.Type.Beeswax:
      state.AddBeeswax(1);
      break;
    case BeeResources.Type.Honey:
      state.AddHoney(1);
      break;
    case BeeResources.Type.Nectar:
      state.AddNectar(1);

      break;
    case BeeResources.Type.Pollen:
      state.AddPollen(1);
      break;
    case BeeResources.Type.RoyalJelly:
      state.AddRoyalJelly(1);
      break;
    }
  }

  /**
   * Tests if a bee can consume the needed resources to generate the
   * outputResource if it can the resources will be consumed by the GameState
   */
  public bool ConsumeResources(GameState state) {
    BeeResources res = new BeeResources();

    // Calculate the needed resources and package it up for gamestate
    for (int i = 0;
         i < inResources.Count && i < inResourcesConversionRate.Count; i++) {
      switch (inResources[i]) {
      case BeeResources.Type.Beeswax:
        res.beeswax = inResourcesConversionRate[i];
        break;
      case BeeResources.Type.Honey:
        res.honey = inResourcesConversionRate[i];
        break;
      case BeeResources.Type.Nectar:
        res.nectar = inResourcesConversionRate[i];
        break;
      case BeeResources.Type.RoyalJelly:
        res.royalJelly = inResourcesConversionRate[i];
        break;
      case BeeResources.Type.Pollen:
        res.pollen = inResourcesConversionRate[i];
        break;
      }
    }

    // return if the resources got consumed
    return state.ConsumeResources(res);
  }
}
