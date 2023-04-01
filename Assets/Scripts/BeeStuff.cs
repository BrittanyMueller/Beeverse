using System;

public abstract class BeeStuff {

  private static readonly string[] QueenNames = {
    "Queenie", "Beatrice","Eleanor", "Celeste", "Callista", "Victoria", "Mira", "Raina", "Belle",
    "Tiana", "Aurora", "Eliza", "Elizabeth", "Juliet", "Marjorie", "Blossom", "Dahlia", "Lily",
    "Rose", "Magnolia", "Julie", "Meredith", "Margaret", "Cleo", "Marie", "Rosalina"
  };

  private static readonly string[] BeeNames = {
    "Jerry",   "Barry",    "Larry",    "Bam",      "Buzz",    "Spike",
    "Trudy",      "Bee",      "Bumbles",  "Fuzzy",   "Stinger",
    "Dart",    "Beatrice", "Blossom",  "Clover",   "Bruce",   "Toby",
    "Cosmo",   "Bloom",    "Dizzy",    "Fuzzy",    "Buzzy",   
    "Goldie",  "Jelly",    "Marigold", "Tulip",    "Zazzles", "Sparkle",
    "Dahlia",  "Bean",     "Bobby",    "Bowie",    "Bubba",   "Bash",
    "Lily",    "Poppy",    "Rose",     "Magnolia", "Orchid",  "Pansy",
    "Myrtle",  "Lilac",    "Sage",     "Pepper",   "Star",    "Lemon",
    "Sugar",   "Honey",    "Henry",    "Pamela",   "Sunny",   "Daisy", "Peach",
    "Petals",  "Bella",    "Buffy",    "Bernie",   "Billie",  "Bailey",
    "Flutter", "Sweetie",  "Bean",     "Boba", "Kirby", "Colby", "Pumpkin", "Pixie", 
    "Bonnie", "Buttercup", "Bess", "Buster", "Benny", "Ruby", "Maya", "Lucy", "Fritz",
    "Ziggy", "Holly", "Bucky", "Bobby", "Tinkerbell", "Willow", "Janet"
  };
  private static readonly string[] BeePuns = {
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
    "You're a force to bee reckoned with.",
    "These bee puns really sting.",
    "To bee or not to bee? The answer is always bees.",
    "It's a new day, bee positive.",
    "What blood type do happy bees have?\n...Bee Positive!",
    "The bees want more honey and less working flowers.",
    "What do bees use to style their hair?\n...Honeycombs!",
    "It’s a bright and honey day."
  };

  public static string GetRandomPun() {
    var rand = new Random();
    return BeePuns[rand.Next(0, BeePuns.Length)];
  }

  public static string GetRandomName() {
    var rand = new Random();
    return BeeNames[rand.Next(0, BeeNames.Length)];
  }
  
  public static string GetRandomQueenName() {
    var rand = new Random();
    return BeeNames[rand.Next(0, QueenNames.Length)];
  }
}
