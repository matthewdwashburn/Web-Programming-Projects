namespace Portal.Services
{
    //Helper class to generate a random glados quote for glados controller
    public class GladosService : IGladosService
    {
        private readonly Random random = new Random();

        private readonly List<string> quotes = new()
        {
            "I'm not even angry. I'm being most sincere right now.",
            "Look at you. Sailing through the air majestically. Like an eagle. Piloting a blimp.",
            "Speedy thing goes in, speedy thing comes out.",
            "How are you holding up? Because I'm a potato.",
            "We both said a lot of things that you're going to regret. But I think we can put our differences behind us. For science. You monster."
        };
        public string GetGladosQuote()
        {
            return quotes[random.Next(quotes.Count)];
        }
    }
}
