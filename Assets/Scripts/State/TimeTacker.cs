public struct TimeTracker {
  public int day;
  public int hour;
  public int minute;

  public TimeTracker(int d, int h, int m) {
    day = d;
    hour = h;
    minute = m;
  }

  public void AddMinutes(int delta) {
    if (minute + delta > 60) {
      minute = (minute + delta) % 60;

      if (hour == 23) {
        day++;
        hour = 0;
      } else {
        hour++;
      }
    } else {
      minute += delta;
    }
  }

  public void SubMinutes(int delta) {
    if (minute - delta < 0) {
      minute = (60 + minute - delta);

      if (hour == 0) {
        day--;
        hour = 23;

      } else {
        hour--;
      }
    } else {
      minute -= delta;
    }
  }

  public override string ToString() {
    return "Day: " + day.ToString() + " " + hour.ToString().PadLeft(2, '0') +
           ":" + minute.ToString().PadLeft(2, '0');
    ;
  }
}