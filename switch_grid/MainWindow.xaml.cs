using System;
using System.Collections;
using System.Collections.Generic; // TASK 8: Using professional generic collections
using System.IO;
using System.Media;
using System.Text;
using System.Windows;
using System.Drawing; // Processes pixel data maps

namespace switch_grid
{
    public partial class MainWindow : Window
    {
        // TASK 8: Replaced raw ArrayList models with safe strongly-typed modern Lists
        private List<string> reply = new List<string>();
        private List<string> ignore = new List<string>();

        private string stored_username = "";
        private string last_keyword = "";
        private string stored_topic = "";

        private SentimentDetector sentiment_detector = new SentimentDetector();

        public MainWindow()
        {
            InitializeComponent();

            // Temporary adapter mapping to pass generic lists to assignment architecture safely
            ArrayList legacyReplyAdapter = new ArrayList();
            ArrayList legacyIgnoreAdapter = new ArrayList();

            respond bot = new respond(legacyReplyAdapter, legacyIgnoreAdapter);

            // Cast data back into generic systems for type-safe processing optimization
            foreach (var item in legacyReplyAdapter) reply.Add(item.ToString());
            foreach (var item in legacyIgnoreAdapter) ignore.Add(item.ToString());

            play_greeting();
            generate_ascii_logo();
        }

        private void generate_ascii_logo()
        {
            try
            {
                string full_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo.jpg");

                if (!File.Exists(full_path))
                {
                    full_path = Path.Combine(Environment.CurrentDirectory, "logo.jpg");
                }
                if (!File.Exists(full_path))
                {
                    full_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "logo.jpg");
                }

                if (File.Exists(full_path))
                {
                    using (Bitmap image = new Bitmap(full_path))
                    {
                        // OPTIMIZATION: Height set to 140 to map perfectly to console proportions without vertical stretch
                        Bitmap resizedImage = new Bitmap(image, new System.Drawing.Size(210, 140));
                        StringBuilder asciiBuilder = new StringBuilder();

                        for (int height = 0; height < resizedImage.Height; height++)
                        {
                            for (int width = 0; width < resizedImage.Width; width++)
                            {
                                Color pixelColor = resizedImage.GetPixel(width, height);
                                int colorValue = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;

                                char ascii_design = colorValue > 200 ? '.' : colorValue > 150 ? '*' : colorValue > 100 ? 'O' : colorValue > 50 ? '#' : '@';
                                asciiBuilder.Append(ascii_design);
                            }
                            asciiBuilder.AppendLine();
                        }

                        ascii_logo_box.Text = asciiBuilder.ToString();
                    }
                }
                else
                {
                    ascii_logo_box.Text = "Error Matrix: 'logo.jpg' couldn't resolve from directory tree structure.";
                }
            }
            catch (Exception ex)
            {
                ascii_logo_box.Text = "System Fault processing image color frames: " + ex.Message;
            }
        }

        private void play_greeting()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Soundd.wav");
                if (File.Exists(path))
                {
                    SoundPlayer player = new SoundPlayer(path);
                    player.Play();
                }
            }
            catch { }
        }

        private void start_ai(object sender, RoutedEventArgs e)
        {
            logo_grid.Visibility = Visibility.Hidden;
            username_grid.Visibility = Visibility.Visible;
        }

        private void submit_name(object sender, RoutedEventArgs e)
        {
            string collect_username = user_name.Text.ToString().Trim();

            if (collect_username != "")
            {
                stored_username = collect_username;
                MessageBox.Show("Welcome back, agent " + stored_username + "!");
                username_grid.Visibility = Visibility.Hidden;
                chats_grid.Visibility = Visibility.Visible;
                chats_list.Items.Add("CyberGuard : Hello " + stored_username + "! Let's secure your perimeter.");
                chats_list.Items.Add("CyberGuard : Ask me about: passwords, phishing, firewall, vpn, fraud, malware.");
                chats_list.Items.Add("──────────────────────────────────────────");
            }
            else
            {
                MessageBox.Show("Please initialize user identity field.");
            }
        }

        private void send_question(object sender, RoutedEventArgs e)
        {
            string user_input = question.Text.ToString().Trim();

            if (user_input != "")
            {
                chats_list.Items.Add(stored_username + " : " + user_input);
                string response = get_response(user_input);
                chats_list.Items.Add("CyberGuard : " + response);
                chats_list.Items.Add("──────────────────────────────────────────");
                question.Clear();
                chats_list.ScrollIntoView(chats_list.Items[chats_list.Items.Count - 1]);
            }
        }

        private string get_response(string user_input)
        {
            string lower_input = user_input.ToLower();

            if (lower_input.Contains("i'm interested in") || lower_input.Contains("my favourite topic is"))
            {
                string topic = lower_input.Contains("i'm interested in") ?
                    lower_input.Replace("i'm interested in", "").Trim() :
                    lower_input.Replace("my favourite topic is", "").Trim();

                stored_topic = topic;
                return "Understood. Tracking interest matrix for: " + stored_topic + ".";
            }

            if (lower_input.Contains("what do you know about me") || lower_input.Contains("what do you remember"))
            {
                string recall = "Subject ID: " + stored_username + ".";
                if (stored_topic != "") recall += " Target Subject Interest: " + stored_topic + ".";
                return recall;
            }

            // TASK 6 OPTIMIZATION: Call your internal class orchestrator to cleanly pipe compound tips
            Sentiment sentiment = sentiment_detector.Detect(lower_input);
            if (sentiment != Sentiment.Neutral)
            {
                // Pull dynamic tip tracking logic straight from your automated class setup
                string compoundResponse = sentiment_detector.ProcessUserMessage(user_input);

                // Track keyword for subsequent "tell me more" follow-ups
                if (lower_input.Contains("scam") || lower_input.Contains("fraud")) last_keyword = "fraud";
                else if (lower_input.Contains("password")) last_keyword = "password";
                else if (lower_input.Contains("phishing")) last_keyword = "phishing";
                else if (lower_input.Contains("hack")) last_keyword = "hacked";
                else if (lower_input.Contains("malware") || lower_input.Contains("virus")) last_keyword = "malicious";
                else if (lower_input.Contains("vpn")) last_keyword = "vpn";

                return compoundResponse;
            }

            // BUG FIX: Added .ToLower() to the input split loop to handle capitalized topics smoothly
            string[] input_words = lower_input.Split(' ');
            foreach (string word in input_words)
            {
                string cleaned_word = word.Trim().ToLower(); // Fixes upper/lower keyword mismatches completely
                if (ignore.Contains(cleaned_word)) continue;

                foreach (string answer in reply)
                {
                    string keyword = answer.Split(' ')[0];
                    if (cleaned_word.Contains(keyword) || keyword.Contains(cleaned_word))
                    {
                        last_keyword = keyword;
                        return get_answer_by_keyword(keyword);
                    }
                }
            }

            return "Query context unrecognized. Try targeting active data streams: passwords, phishing, firewall, vpn, fraud, malware.";
        }

        private string get_answer_by_keyword(string keyword)
        {
            List<string> matching = new List<string>();
            foreach (string answer in reply)
            {
                string answer_keyword = answer.Split(' ')[0];
                if (answer_keyword == keyword)
                {
                    matching.Add(answer.Substring(keyword.Length).Trim());
                }
            }

            if (matching.Count > 0)
            {
                Random random = new Random();
                return matching[random.Next(matching.Count)].ToString();
            }
            return "No matching records found for context: " + keyword;
        }
    }
}