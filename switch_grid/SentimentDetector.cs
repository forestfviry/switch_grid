using System;

namespace switch_grid
{
    public enum Sentiment
    {
        Positive,
        Negative,
        Neutral
    }

    public class SentimentDetector
    {
        public Sentiment Detect(string input)
        {
            string lowerInput = input.ToLower();

            if (lowerInput.Contains("worried") || lowerInput.Contains("scared") ||
                lowerInput.Contains("afraid") || lowerInput.Contains("frustrated") ||
                lowerInput.Contains("anxious") || lowerInput.Contains("overwhelmed"))
            {
                return Sentiment.Negative;
            }
            else if (lowerInput.Contains("curious") || lowerInput.Contains("interested") ||
                     lowerInput.Contains("happy") || lowerInput.Contains("excited") ||
                     lowerInput.Contains("good"))
            {
                return Sentiment.Positive;
            }

            return Sentiment.Neutral;
        }

        public string GetSentimentResponse(Sentiment sentiment)
        {
            if (sentiment == Sentiment.Negative)
            {
                return "It's completely understandable to feel that way. Cyber threats can be overwhelming. Let me share some tips to help you stay safe:";
            }
            else if (sentiment == Sentiment.Positive)
            {
                return "Great energy! Staying proactive and positive is key to solid security defenses. Here's a quick tip:";
            }
            return "";
        }

        public string GetCybersecurityTip(string input)
        {
            string lowerInput = input.ToLower();

            if (lowerInput.Contains("scam") || lowerInput.Contains("fraud"))
            {
                return "Tip: Always double-check urgent requests for money or personal information. Real companies won't force immediate wire payments.";
            }
            if (lowerInput.Contains("phishing") || lowerInput.Contains("email"))
            {
                return "Tip: Check the sender's full domain name before clicking links. Phishing attacks thrive on lookalike domains.";
            }
            if (lowerInput.Contains("password") || lowerInput.Contains("login"))
            {
                return "Tip: Use long passphrases with mixed characters, and avoid reusing passwords across multiple personal profiles.";
            }
            if (lowerInput.Contains("malware") || lowerInput.Contains("virus"))
            {
                return "Tip: Run an active anti-virus solution and avoid opening external attachments sent from unverified sources.";
            }
            if (lowerInput.Contains("vpn") || lowerInput.Contains("wifi"))
            {
                return "Tip: Avoid conducting banking tasks over public Wi-Fi access links without an encrypted VPN tunnel active.";
            }

            // Universal fallback tip if no matching keywords are found
            return "Tip: Ensure your desktop operating updates and browser patches are installed immediately to shield against active system vulnerabilities.";
        }

        // Task 6 Compliance: Orchestrates and merges the prefix statement with a cybersecurity tip automatically
        public string ProcessUserMessage(string userInput)
        {
            Sentiment detectedSentiment = Detect(userInput);
            string empathyPrefix = GetSentimentResponse(detectedSentiment);
            string concreteTip = GetCybersecurityTip(userInput);

            return $"{empathyPrefix}\n\n{concreteTip}";
        }
    }
}