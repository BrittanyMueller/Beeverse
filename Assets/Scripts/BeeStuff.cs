using System;

public class BeeStuff {

  private static string[] _queenNames = {
    "Queenie", "Beatrice",
    "Blossom",
  };
  
  private static string[] _beeNames = {
    "Jerry", "Barry", "Larry", "Bam", "Buzz", "Spike", "Trudy",
    "Bob", "Bee",
    "Bumbles",
    "Fuzzy",
    "Stinger",
    "Dart",
    "Beatrice",
    "Blossom",
    "Clover",
    "Bruce",
    "Toby",
    "Cosmo",
    "Bloom",
    "Dizzy",
    "Fuzzy",
    "Buzzy",
    "Celeste",
    "Goldie",
    "Jelly",
    "Marigold",
    "Tulip",
    "Zazzles",
    "Sparkle",
    "Dahlia",
    "Bean",
    "Bobby",
    "Bowie",
    "Bubba",
    "Bash",
    "Lily",
    "Poppy",
    "Rose",
    "Magnolia",
    "Orchid",
    "Pansy",
    "Myrtle",
    "Lilac",
    "Sage",
    "Pepper",
    "Star",
    "Lemon",
    "Sugar",
    "Honey",
    "Henry",
    "Pamela",
    "Sunny",
    "Daisy",
    "Petals",
    "Bella",
    "Buffy",
    "Bernie",
    "Billie",
    "Bailey",
    "Flutter",
    "Sweetie",
    "Bean",
    "Boba"
  };
  private static string[] _beePuns = {
    "I heard ya like jazz.",
    "Bee the change you want to see.",
    "Don’t worry, bee happy.",
    "Beelieve in yourself!",
    "Good Morning! It’s time to take your Vitamin Bee.",
    "Just bee yourself; you're the bees knees!",
    "Truth bee told, this is a lot of bee puns.",
    "Everyday I'm bumblin'",
    "What do bees like with their sushi?\n...Wasa-bee!",
    "According to all known laws of aviation, there is no way a bee should be able to fly...",
  };

  public static string GetRandomPun() {
    var rand = new Random();
    return _beePuns[rand.Next(0, _beePuns.Length)];
  }

  public static string GetRandomName() {
    var rand = new Random();
    return _beeNames[rand.Next(0, _beeNames.Length)];
  }
}
