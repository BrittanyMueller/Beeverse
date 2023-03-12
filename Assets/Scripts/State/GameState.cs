using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{

    private DateTime _currentTime;
    public DateTime CurrentTime { get { return _currentTime; }}

    

    public LogController logController;

    // Game config settings
    public int minutesPreSecond = 5;



    // todo remove
    int previousSecond = 0;

    public bool Paused {
        get { return _paused; }
        set {
            
            _paused = value;
            NotifyPausedChanged();
        }
    }
    private bool _paused;
    //todo add readonly property


    // Start is called before the first frame update
    void Start()
    {
        _paused = false;
        _currentTime = new DateTime();
        previousSecond = _currentTime.Minute;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _currentTime = _currentTime.AddMinutes(minutesPreSecond * Time.deltaTime);

        if (previousSecond != _currentTime.Minute) {
            UpdateLog(_currentTime.Minute.ToString());
            previousSecond = _currentTime.Minute;
        }
    

    }


    /****************************
     * Functions to control the game state
     *****************************/

    // Tell everyone who cares the game is no longer paused
    private void NotifyPausedChanged() {

    }

    public void UpdateLog(String update) {
        logController.UpdateLog(update);
    }


    public void Save() {
        Debug.Log("Your game is saved.. jk not impl yet");
    }


}
