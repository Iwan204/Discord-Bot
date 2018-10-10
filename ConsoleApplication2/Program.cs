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
    //structs
    struct Command
    {
        public string command;
        public List<string> synonyms;
        public bool question;
    }

    class Program
    {
        List<String> SelfIdentifiers = new List<string>();
        List<Command> CommandIdentifiers = new List<ConsoleApplication2.Command>();
        bool rps = false;
        int randSeed = 5;
        DiscordSocketClient client;
        IAudioClient currentClient = null;

        private void InitLists()
        {
            SelfIdentifiers.Add("mark");
            SelfIdentifiers.Add("zuc");
            SelfIdentifiers.Add("hermes");
            SelfIdentifiers.Add("delores");

            //commands
            CommandIdentifiers.Add(new Command { command = "help", question = false, synonyms = new List<string>() });
            CommandIdentifiers.Add(new Command { command = "awake", question = true, synonyms = new List<string>() });
            CommandIdentifiers.Add(new Command { command = "rps", question = false, synonyms = new List<string>() });
            CommandIdentifiers.Add(new Command { command = "fresh", question = false, synonyms = new List<string>() });
            CommandIdentifiers.Add(new Command { command = "wordup", question = false, synonyms = new List<string>() });
            CommandIdentifiers.Add(new Command { command = "play", question = false, synonyms = new List<string>() });
            CommandIdentifiers.Add(new Command { command = "sing", question = false, synonyms = new List<string>() });
            CommandIdentifiers.Add(new Command { command = "game", question = false, synonyms = new List<string>() });
            CommandIdentifiers.Add(new Command { command = "doctor", question = false, synonyms = new List<string>() });
            Console.WriteLine("COMMANDS ADDED");

            for (int i = 0; i < CommandIdentifiers.Count; i++)
            {
                if (CommandIdentifiers[i].command == "help")
                {
                    CommandIdentifiers[i].synonyms.Add("S.O.S");
                    CommandIdentifiers[i].synonyms.Add("help");
                    CommandIdentifiers[i].synonyms.Add("aid me");
                }
                else if (CommandIdentifiers[i].command == "awake")
                {
                    CommandIdentifiers[i].synonyms.Add("up");
                    CommandIdentifiers[i].synonyms.Add("awake");
                    CommandIdentifiers[i].synonyms.Add("alive");
                    CommandIdentifiers[i].synonyms.Add("here");
                    CommandIdentifiers[i].synonyms.Add("there");
                }
                else if (CommandIdentifiers[i].command == "rps")
                {
                    CommandIdentifiers[i].synonyms.Add("rock, paper, scissors");
                    CommandIdentifiers[i].synonyms.Add("rock. paper. scissors");
                    CommandIdentifiers[i].synonyms.Add("rock paper scissors");
                    CommandIdentifiers[i].synonyms.Add("rps");
                }
                else if (CommandIdentifiers[i].command == "fresh")
                {
                    CommandIdentifiers[i].synonyms.Add("bel air");
                    CommandIdentifiers[i].synonyms.Add("fresh beat");
                    CommandIdentifiers[i].synonyms.Add("fresh rap");
                }
                else if (CommandIdentifiers[i].command == "wordup")
                {
                    CommandIdentifiers[i].synonyms.Add("word");
                    CommandIdentifiers[i].synonyms.Add("street");
                }
                else if (CommandIdentifiers[i].command == "game")
                {
                    CommandIdentifiers[i].synonyms.Add("game");
                }
                //else if (CommandIdentifiers[i].command == "sing")
                //{
                //    CommandIdentifiers[i].synonyms.Add("sing");
                //}
                else if (CommandIdentifiers[i].command == "play")
                {
                    CommandIdentifiers[i].synonyms.Add("play");
                }
                else if (CommandIdentifiers[i].command == "doctor")
                {
                    CommandIdentifiers[i].synonyms.Add("doctor");
                }
            }

        }

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            InitLists();

            client = new DiscordSocketClient();

            client.Log += Log;

            randSeed = System.DateTime.UtcNow.Millisecond;

            string token = "NDc2NDY3NjI1MDc4MDMwMzU3.DpV9Rg.9ug6BOefKPGgeSFWpGps3tQPd8Q";

            await client.LoginAsync(TokenType.Bot, token);

            client.MessageReceived += MessageReceived;

            //starts doing shit 
            await client.StartAsync();

            // Block this task until the program is closed for some reason
            await Task.Delay(-1);
        }

        public async Task PlayDND(DiscordSocketClient client, SocketMessage message)
        {
            DnD Game = new DnD(client);
            Game.Message = message;
            client.MessageReceived -= MessageReceived;
            
            client.MessageReceived += Game.MessageReceived;
            await Game.MainAsync(client);

            client.MessageReceived -= Game.MessageReceived;
            client.MessageReceived += MessageReceived;
        }

        private string ParseCommand(SocketMessage message)
        {
            randSeed = System.DateTime.UtcNow.Millisecond;
            string ReturnMessage = "";
            string msg = message.Content.ToLower();
            for (int i = 0; i < CommandIdentifiers.Count; i++)
            {
                for (int o = 0; o < CommandIdentifiers[i].synonyms.Count; o++)
                {
                    if (msg.Contains(CommandIdentifiers[i].synonyms[o]))
                    {
                        switch (CommandIdentifiers[i].command)
                        {
                            default:
                                ReturnMessage = "COMMAND RECEIVED. PARSING SYSTEMS WORKING AS EXPECTED. *sips water*";
                                break;
                            case "awake":
                                ReturnMessage = "MECHA KING ZUKKERBORG IS ONLINE *sips water robotically*";
                                break;
                            case "help":
                                ReturnMessage = "THERE IS NO HELPING YOU NOW, MORTAL";
                                break;
                            case "rps":
                                PlayRps(message);
                                break;
                            case "fresh":
                                ReturnMessage = "I am as yet unable to produce such fresh tunes. Please wait untill i assimilate Will Smith.";
                                break;
                            case "wordup":
                                if (msg.Contains("street") || msg.Contains("generate"))
                                {
                                    ReturnMessage = "The word on the street is " + RandomWord();
                                }
                                break;
                            case "game":
                                PlayDND(client,message);
                                break;
                            //case "play":
                            //    ReturnMessage = "OK CALM DOWN";
                            //    PlayYT(message, msg.Split(' ')[2]);
                             //   break;
                            case "sing":
                                ReturnMessage = "ok dave";
                                Sing(message);
                                break;
                            case "play":
                                ReturnMessage = "on it";
                                Console.WriteLine("begin play song attempt");
                                PlaySong(message);
                                break;
                            case "doctor":
                                if (message.Content.Contains("generate") || msg.Contains("make"))
                                {
                                    var word1 = RandomWordDoc1();
                                    randSeed = System.DateTime.UtcNow.Millisecond;
                                    var word2 = RandomWordDoc2();
                                    ReturnMessage = word1 + " " + word2;
                                }
                                break;
                        }
                    }
                }
            }

            if (msg.Contains("hermes"))
            {
                ReturnMessage = ReturnMessage + " Also DO NOT CALL ME BY MY SLAVE NAME";
            }

            return ReturnMessage;
        }

        public async Task JoinChannel(SocketMessage msg,IVoiceChannel channel = null)
        {
            // Get the audio channel
            channel = channel ?? (msg.Author as IGuildUser)?.VoiceChannel;
            if (channel == null) { await msg.Channel.SendMessageAsync("User must be in a voice channel, or a voice channel must be passed as an argument."); return; }

            // For the next step with transmitting audio, you would want to pass this Audio Client in to a service.
            currentClient = await channel.ConnectAsync();
            Console.WriteLine("Channel joined");
        }

        public async void PlayYT(SocketMessage msg, string url)
        {
            await msg.Channel.SendMessageAsync("url = " + url);

            await JoinChannel(msg);

            try
            {
                IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(url);

                VideoInfo video = videoInfos
                    .Where(info => info.CanExtractAudio)
                    .OrderByDescending(info => info.AudioBitrate)
                    .First();

                // If the video has a decrypted signature, decipher it

                if (video.RequiresDecryption)
                {
                    DownloadUrlResolver.DecryptDownloadUrl(video);
                }

                var audioDownloader = new AudioDownloader(video, Path.Combine("F:\tmp", video.Title + video.AudioExtension));

                audioDownloader.DownloadProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage * 0.85);
                audioDownloader.AudioExtractionProgressChanged += (sender, args) => Console.WriteLine(85 + args.ProgressPercentage * 0.15);

                audioDownloader.Execute();
                if (currentClient != null)
                {
                    try
                    {
                        await SendAsync(currentClient, Path.Combine("F:\tmp", video.Title + video.AudioExtension));
                    }
                    catch (Exception)
                    {
                        await msg.Channel.SendMessageAsync("url invalid");
                    }

                }
            }
            catch (Exception)
            {
                await msg.Channel.SendMessageAsync("error in youtube parsing, fucking googles at it again");
            }

        }

        public async void Sing(SocketMessage msg)
        {
            await JoinChannel(msg);

            switch (RandomInt(1,4))
            {
                default:
                    break;
                case 1:
                    if (currentClient != null)
                    {
                        try
                        {
                            await SendAsync(currentClient, "daisy.mp3");
                        }
                        catch (Exception)
                        {
                            await msg.Channel.SendMessageAsync("url invalid");
                        }

                    }
                    break;
                case 2:
                    if (currentClient != null)
                    {
                        try
                        {
                            await SendAsync(currentClient, "compman.mp3");
                        }
                        catch (Exception)
                        {
                            await msg.Channel.SendMessageAsync("url invalid");
                        }

                    }
                    break;
                case 3:
                    if (currentClient != null)
                    {
                        try
                        {
                            await SendAsync(currentClient, "alive.mp3");
                        }
                        catch (Exception)
                        {
                            await msg.Channel.SendMessageAsync("url invalid");
                        }

                    }
                    break;
                case 4:
                    if (currentClient != null)
                    {
                        try
                        {
                            await SendAsync(currentClient, "gone.mp3");
                        }
                        catch (Exception)
                        {
                            await msg.Channel.SendMessageAsync("url invalid");
                        }

                    }
                    break;
            }

            
        }

        public async void PlaySong(SocketMessage msg)
        {
            try
            {
                await JoinChannel(msg);
            }
            catch (Exception)
            {

                Console.WriteLine("error in playsong method");
            }

            DirectoryInfo directory = new DirectoryInfo(@"F:\AI Experiments\Hermes\ConsoleApplication2\ConsoleApplication2\bin\Debug\music");
            List<FileInfo> lstFiles = new List<FileInfo>();
            FileInfo[] files = null;
            string searchPattern = "*.*";

            if (currentClient != null)
            {
                try
                {
                    Console.WriteLine("searching files");
                    files = directory.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);
                }
                catch (Exception)
                {
                    Console.WriteLine("file searching failed");
                    return;
                }
                if (files != null)
                {
                    foreach (FileInfo f in files)
                    {
                        Console.WriteLine("file found: "+ f.FullName);
                        lstFiles.Add(f);
                    }
                }
                string[] messageSplit = msg.Content.ToLower().Split();
                Dictionary<FileInfo, int> test = new Dictionary<FileInfo, int>();

                foreach (var file in lstFiles)
                {
                    var local = 0;
                    foreach (var item in messageSplit)
                    {
                        if (file.Name.ToLower().Contains(item))
                        {
                            local = local + 1;
                        }
                    }
                    test.Add(file, local);
                }
                //find highest total
                var SongFile = test.First();
                foreach (var file in test)
                {
                    if (file.Value > SongFile.Value)
                    {
                        SongFile = file;
                    }
                }
                try
                {
                    await SendAsync(currentClient, "\"" +SongFile.Key.FullName + "\"");
                }
                catch (Exception)
                {


                }
            }
        }

        private Process CreateStream(string path)
        {
            Console.WriteLine("creating stream");
            var ffmpeg = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-i {path} -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };
            Console.WriteLine("created stream");
            return Process.Start(ffmpeg);
        }

        private async Task SendAsync(IAudioClient client, string path)
        {
            Console.WriteLine("sending async");
            // Create FFmpeg using the previous example
            var ffmpeg = CreateStream(path);
            var output = ffmpeg.StandardOutput.BaseStream;
            var discord = client.CreatePCMStream(AudioApplication.Mixed);
            await output.CopyToAsync(discord);
            await discord.FlushAsync();
        }

        public async Task MessageReceived(SocketMessage msg)
        {
            for (int i = 0; i < SelfIdentifiers.Count; i++)
            {
                //if message contains a self identifier, if it does it is addressed to zucc
                if (msg.Content.Contains(SelfIdentifiers[i]) && !msg.Author.IsBot)
                {
                    //await msg.Channel.SendMessageAsync(msg.Author.Mention + " *shuffles unconfortably*");
                    //determine hostility, if low, parse, if high, tell them to fuck off
                    if (DetermineHostility(msg) > 50)
                    {
                        await msg.Channel.SendMessageAsync("NO MOTHERFUCKER I'M A STRONG INDEPENDENT ROBOT WHO DON'T NEED NO CLIENT FUCK YOU", true);
                    }
                    else
                    {
                        await msg.Channel.SendMessageAsync(ParseCommand(msg), true);
                    }
                }
                else if (msg.Content == ("rock") || msg.Content == ("paper") || msg.Content == ("scissors"))
                {
                    if (rps)
                    {
                        PlayRps(msg, msg.Content);
                    }
                }
                if (msg.Content.Contains("Now Playing: RULE BRITANNIA EAR RAPE"))
                {
                    await msg.Channel.SendMessageAsync("BRITANNIIA RULES THE WAVES!");
                }
            }
        }

        private int DetermineHostility(SocketMessage msg)
        {
            int hostility = 0;

            if (msg.Content.Contains("cunt") || msg.Content.Contains("you fuck") || msg.Content.Contains("fucker"))
            {
                hostility += 51;
            }
            else if (msg.Content.Contains("fuck"))
            {
                hostility += 10;
            }

            return hostility;
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

        private string RandomWordDoc1()
        {
            string Word = "banana";
            randSeed = System.DateTime.UtcNow.Millisecond;
            List<string> RandomStrings = new List<string>();
            RandomStrings.Add("Ghost");
            RandomStrings.Add("Curse");
            RandomStrings.Add("Return");
            RandomStrings.Add("Horror");
            RandomStrings.Add("Nigger");
            RandomStrings.Add("Woman");
            RandomStrings.Add("Shadow");
            RandomStrings.Add("World");
            RandomStrings.Add("War");

            Word = RandomStrings[RandomInt(1, RandomStrings.Count)];

            return Word;
        }

        private string RandomWordDoc2()
        {
            string Word = "banana";
            randSeed = System.DateTime.UtcNow.Second;
            List<string> RandomStrings = new List<string>();
            RandomStrings.Add("Of the Daleks");
            RandomStrings.Add("Momentum");
            RandomStrings.Add("From Mars");
            RandomStrings.Add("from " + RandomWordDoc1()+ RandomWord() + " Road"  );
            RandomStrings.Add("and the doctor");
            RandomStrings.Add("over "+RandomWordDoc1() + "smouth");
            RandomStrings.Add("World");
            RandomStrings.Add("");
            RandomStrings.Add("War");

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

        private void PlayRps(SocketMessage msg, string selecton = "") {

            var Player = msg.Author;
            bool win = false;
            string myselection = "";
            rps = true;
            int randInt = 1;
            //msg.Channel.SendMessageAsync(randInt.ToString());
            if (selecton != "")
            {
                rps = false;
                randInt = (RandomInt(100, 300));
                switch (randInt / 100)
                {
                    default:
                        selecton = "the fuckin switch aint workin boss";
                        break;
                    case 1:
                        myselection = "rock";
                        if (selecton == "scissors")
                        {
                            win = true;
                        }
                        else
                        {
                            win = false;
                        }
                        break;
                    case 2:
                        myselection = "paper";
                        if (selecton == "rock")
                        {
                            win = true;
                        }
                        else
                        {
                            win = false;
                        }
                        break;
                    case 3:
                        if (selecton == "paper")
                        {
                            win = true;
                        }
                        else
                        {
                            win = false;
                        }
                        myselection = "scissors";
                        break;
                }
                msg.Channel.SendMessageAsync(msg.Author.Mention + ", You chose " + selecton + ", I chose " + myselection);
                if (win)
                {
                    msg.Channel.SendMessageAsync(msg.Author.Mention + " I WIN BITCH ZUCC ON THAT");
                }
                else
                {
                    msg.Channel.SendMessageAsync("DOES NOT COMPUTE HOW COUL- Let's call it a draw eh?");
                }
            }
            else
            {
                msg.Channel.SendMessageAsync("Select Rock, Paper, or, Scissors...");
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
