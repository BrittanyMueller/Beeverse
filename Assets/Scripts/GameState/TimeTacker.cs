
/**
 * TimeTracker is used to hold the internal time of gameobject
 * and state
 */
public class TimeTracker {

  public int Day => _day;
  public int Hour => _hour;
  public int Minute => _minute;
  public int TotalTime => _totalMinute;

  // Keep track of time cached after every change
  private int _day;
  private int _hour;
  private int _minute;

  // Keeps total time
  private int _totalMinute;

  public TimeTracker(int d, int h, int m) {
    _totalMinute = m + 60 * h + d * 60 * 24;
    CalcTimes();
  }

  public void AddMinutes(int delta) {
    _totalMinute += delta;
    CalcTimes();
  }

  public void SubMinutes(int delta) {
    _totalMinute -= delta;
    CalcTimes();
  }

  private void CalcTimes() {
    _day = _totalMinute / 24 / 60;
    _hour = (_totalMinute - _day * 24 * 60) / 60;
    _minute = _totalMinute - _hour * 60 - _day * 24 * 60;
  }

  public override string ToString() {
    return "Day " + Day + " " + Hour.ToString().PadLeft(2, '0') +
           ":" + Minute.ToString().PadLeft(2, '0');
  }
}