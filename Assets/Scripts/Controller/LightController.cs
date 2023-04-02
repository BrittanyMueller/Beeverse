using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour {
    // GameObject references
    [SerializeField] private Light sunlight;
    [SerializeField] private Light moonlight;
    [SerializeField] private GameState state;
    
    [SerializeField] private Color ambientDayLight;
    [SerializeField] private Color ambientNightLight;
    [SerializeField] private AnimationCurve lightCurve;
    
    [SerializeField] private Material skyMaterial;
    private float _skyOffset;

    [SerializeField] private float _maxSunIntensity;
    [SerializeField] private float _maxMoonIntensity;
    
    [SerializeField] private float sunriseHour;
    private TimeSpan _sunriseTime;
    
    [SerializeField] private float sunsetHour;
    private TimeSpan _sunsetTime;
    
    private TimeSpan _dayDuration;
    private TimeSpan _nightDuration;
    
    private void Start() {
        _sunriseTime = TimeSpan.FromHours(sunriseHour);
        _sunsetTime = TimeSpan.FromHours(sunsetHour);
        
        _dayDuration = CalculateTimeChange(_sunriseTime, _sunsetTime);
        _nightDuration = CalculateTimeChange(_sunsetTime, _sunriseTime);
    }

    private void Update() {
        RotateSun();
        UpdateLighting();
    }

    private void RotateSun() {
        float lightRotation;
        float percentDay;
        float skyOffset;

        var currentTime = TimeSpan.FromMinutes(state.CurrentTime.Hour * 60 + state.CurrentTime.Minute);
        if (currentTime == _sunriseTime || currentTime == _sunsetTime) return;
        var isDay = currentTime > _sunriseTime && currentTime < _sunsetTime;

        if (isDay) {
            var timeFromSunrise = CalculateTimeChange(_sunriseTime, currentTime);
            percentDay = (float)(timeFromSunrise.TotalMinutes / _dayDuration.TotalMinutes);
            skyOffset = Mathf.Lerp(0.7f, 1.15f, percentDay);
            lightRotation = Mathf.Lerp(0, 180, percentDay);
        }
        else {
            var timeFromSunset = CalculateTimeChange(_sunsetTime, currentTime);
            if (Math.Abs(timeFromSunset.TotalMinutes - _nightDuration.TotalMinutes) < 0.01) return;
            percentDay = (float)(timeFromSunset.TotalMinutes / _nightDuration.TotalMinutes);
            skyOffset = Mathf.Lerp(0.15f, 0.7f, percentDay);
            lightRotation = Mathf.Lerp(180, 360, percentDay);
        }
        sunlight.transform.rotation = Quaternion.AngleAxis(lightRotation, Vector3.right);
        skyMaterial.mainTextureOffset = new Vector2(skyOffset, 0);
    }

    private void UpdateLighting() {
        var dotProduct = Vector3.Dot(sunlight.transform.forward, Vector3.down);
        sunlight.intensity = Mathf.Lerp(0, _maxSunIntensity, lightCurve.Evaluate(dotProduct));
        moonlight.intensity = Mathf.Lerp(_maxMoonIntensity, 0, lightCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(ambientNightLight, ambientDayLight, lightCurve.Evaluate(dotProduct));
    }

    private static TimeSpan CalculateTimeChange(TimeSpan start, TimeSpan end) {
        // Calculate time difference
        var change = end - start;
        
        // Negative difference indicates time rolling to next day, adjust day
        if (change.TotalSeconds > 0f) return change;
        return change + TimeSpan.FromHours(24);
    }
}
