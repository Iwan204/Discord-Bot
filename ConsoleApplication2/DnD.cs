using Discord;
using Discord.WebSocket;
using Discord.Audio;
using YoutubeExtractor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using System.IO;

namespace ConsoleApplication2
{
    class DnD
    {

        public SocketMessage Message;
        IChannel Channel;
        private DiscordSocketClient client;
        List<SocketUser> players;
        bool playerSet;
        bool gameon;
        int randSeed;

        public DnD(DiscordSocketClient client)
        {
            this.client = client;
        }

        public static void Main(DiscordSocketClient client)
            => new DnD(client).MainAsync(client).GetAwaiter().GetResult();

        public async Task MainAsync(DiscordSocketClient client)
        {
            Channel = Message.Channel;
            playerSet = false;
            gameon = true;
            randSeed = 5;
            players = new List<SocketUser>();
            Message.Channel.SendMessageAsync("Who is playing");
            loop();
            //await Task.Delay(-1);
        }

        public async void loop()
        {
            int floor = 1;
            room Room = ConstructRoom();
            moveroom();
            while (gameon)
            {
                if (playerSet)
                {
                    //update
                    bool test = true;
                    foreach (var monster in Room.inhabitants)
                    {
                        if (monster.health != 0)
                        {
                            test = false;
                        }
                    }
                    if (test)
                    {
                        moveroom();
                        floor = floor + 1;
                    }

                    //draw
                }

            }

        }

        private room moveroom()
        {
            room newroom = ConstructRoom();
            string horrors = "";
            int horrormark = 1;
            foreach (var item in newroom.inhabitants)
            {
                horrors = horrors + "["+horrormark+"]" + " "+ item.Name + ",";
                horrormark++;
            }
            Message.Channel.SendMessageAsync("You enter a room, " + newroom.Name);
            Message.Channel.SendMessageAsync("You see things stirring in the depths of the " + newroom.Name + ". You identify them as: " + horrors);
            return newroom;
        }

        struct room 
        {
            public string Name;
            public string description;
            public bool isBoss;
            public List<minion> inhabitants;
        }

        struct minion
        {
            public string Name;
            public string Description;
            public bool isBoss;
            public int health;
            public int speed;
            public int atk;
            public int def;
        }

        private room ConstructRoom()
        {
            room generated = new room();
            generated.inhabitants = new List<minion>();

            if (RandomInt(0,100) <= 25)
            {
                generated.isBoss = true;
            }
            if (generated.isBoss)
            {
                generated.inhabitants.Add(GenerateBossMinion());
            }
            else
            {
                var length = RandomInt(1,5);
                for (int i = 0; i < length; i++)
                {

                    generated.inhabitants.Add(GenerateMinion());
                }
            }
            generated.Name = "The " + RandomWord() + " " + RandomWord();
            return generated;
        }

        private minion GenerateMinion()
        {
            minion generated = new minion();

            generated.Name = RandomName();
            generated.isBoss = false;
            generated.health = RandomInt(1,10);
            generated.speed = RandomInt(1,5);
            generated.atk = RandomInt(1, 3);
            generated.atk = RandomInt(0, 3);
            generated.Description = "";
            return generated;
        }

        private minion GenerateBossMinion()
        {
            minion generated = new minion();

            generated.Name = RandomBossName();
            generated.isBoss = true;
            generated.health = RandomInt(1, 25);
            generated.speed = RandomInt(1, 10);
            generated.atk = RandomInt(1, 10);
            generated.atk = RandomInt(0, 10);
            generated.Description = "";
            return generated;
        }

        private string RandomWord()
        {
            string Word = "banana";

            List<string> RandomStrings = new List<string>();
            RandomStrings.Add("bannana");
            RandomStrings.Add("pear");
            RandomStrings.Add("sap");
            RandomStrings.Add("rat");
            RandomStrings.Add("nigger");
            RandomStrings.Add("osteoperosis");
            RandomStrings.Add("can't");
            RandomStrings.Add("be");
            RandomStrings.Add("arsed");
            RandomStrings.Add("adding");
            RandomStrings.Add("many");
            RandomStrings.Add("more");
            RandomStrings.Add("of");
            RandomStrings.Add("these");
            RandomStrings.Add("mental");
            RandomStrings.Add("note");
            RandomStrings.Add("use");
            RandomStrings.Add("API");
            RandomStrings.Add("next");
            RandomStrings.Add("time");

            Word = RandomStrings[RandomInt(1, RandomStrings.Count)];

            return Word;
        }

        private string RandomName()
        {
            string Word = "banana";

            List<string> RandomStrings = new List<string>();
            RandomStrings.Add("Shia");
            RandomStrings.Add("pear");
            RandomStrings.Add("sap");
            RandomStrings.Add("rat");
            RandomStrings.Add("nigger");
            RandomStrings.Add("osteoperosimon");
            RandomStrings.Add("an asian");
            RandomStrings.Add("LAMP");
            RandomStrings.Add("ash");
            RandomStrings.Add("calculon");
            RandomStrings.Add("robo-remidial");
            RandomStrings.Add("thegamerdude256");
            RandomStrings.Add("sam");
            RandomStrings.Add("gay");
            RandomStrings.Add("coffing (get it)");
            RandomStrings.Add("snak");
            RandomStrings.Add("moth");
            RandomStrings.Add("rogue");
            RandomStrings.Add("finn");
            RandomStrings.Add("nameless one");

            Word = RandomStrings[RandomInt(1, RandomStrings.Count)];

            return Word;
        }

        private string RandomBossName()
        {
            string Word = "banana";

            List<string> RandomStrings = new List<string>();
            RandomStrings.Add("Shia LaBeouf");
            RandomStrings.Add("Katrina");
            RandomStrings.Add("Diane Abbot");
            RandomStrings.Add("Jeremy Corbyn");
            RandomStrings.Add("Lord nigger");
            RandomStrings.Add("Maz Kanata");
            RandomStrings.Add("Ricegum");
            RandomStrings.Add("SUN");
            RandomStrings.Add("LIAM'S SECRET DESIRE TO PLAY SKYRIM");
            RandomStrings.Add("Rocket Man");
            RandomStrings.Add("SLEPP THE OLD ONE");
            RandomStrings.Add("Khans Kool Klan");
            RandomStrings.Add("SAM");
            RandomStrings.Add("Gaylord, lord of the gay");
            RandomStrings.Add("Mon MOTHma");
            RandomStrings.Add("Mike Wazowski");
            RandomStrings.Add("MothMan");
            RandomStrings.Add("Rogue One");
            RandomStrings.Add("TR8T0R");
            RandomStrings.Add("SARTHAL, SPAWN OF SLEPP");

            Word = RandomStrings[RandomInt(1, RandomStrings.Count)];

            return Word;
        }

        private int RandomInt(int min = 0, int max = 100)
        {
            int RandNum = 0;

            Random random = new Random(randSeed);
            RandNum = random.Next(min, max);

            randSeed = RandNum + randSeed / 2;

            return RandNum;
        }

        public async Task MessageReceived(SocketMessage msg)
        {
            if (!msg.Author.IsBot)
            {
                string[] message = msg.Content.Split(' ');
                try
                {
                    if (!playerSet)
                    {
                        string playernames = "";
                        foreach (var player in msg.MentionedUsers)
                        {
                            players.Add(player);
                            playernames = playernames + " " + player.Username;
                        }
                        playerSet = true;
                        await Message.Channel.SendMessageAsync("players are: " + playernames);
                    }
                }
                catch (Exception)
                {
                    throw;
                    //await Message.Channel.SendMessageAsync("error");
                }
                if (msg.Content.Contains("QUIT GAME"))
                {
                    gameon = false;
                }


            }
        }

    }
}
