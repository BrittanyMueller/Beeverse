using System;

public class BeeStuff {
    
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
}
