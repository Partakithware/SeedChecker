using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

class Program
{
    private static readonly byte[] _salt = Convert.FromBase64String("fT2JQQzNMJl2NRoMbo9RjA==");
    
    private static readonly Dictionary<string, string> AllSeeds = new Dictionary<string, string>
    {
        {"2htOIVagY/7JFx7acMpyUR6D3qJDr/u+", "paintEverythingGray"},
        {"YJayFFSdWEl66+rlFoWJRNvBHJi8gHnx", "paintEverythingNegative"},
        {"5Czr2vSNyB9hJd1yob+TYo0qqH/5U2P9", "coatEverythingEcho"},
        {"5YXhKErRZovhjJkrP9fptrVHbNc1oSSn", "coatEverythingIlluminant"},
        {"cptECrPRxYeNTULJULs4gVoKdRsf3c3n", "noSurface"},
        {"QQN1FbxlHeUCXPZc51GYvn8G5GXOJcny", "extraLivingTrees"},
        {"0ebq4RCzI3PVaUPOT0f6/+vkXEaoLz2U", "extraFloatingIslands"},
        {"GkviuS3QN0pyESRJdjIs6oC8s8hOhUXw", "errorWorld"},
        {"N8G20sWOkIa7ZP0rS/jopLpe9180N6Tx", "graveyardBloodmoonStart"},
        {"io2s6kMi4L7ZCDYZGP1Hc8nEWuYW4gp5", "surfaceIsInSpace"},
        {"xYBNU5Soje9VhQHNQXETDKbwlc+7XZau", "rainsForAYear"},
        {"vWb/t7nNF+tnjgr5VgY2hi0HcT1j3kvC", "biggerAbandonedHouses"},
        {"zSwnCH9E121+S6VQdB0k20E7IPdtobls", "randomSpawn"},
        {"+URq9gxzcyHxAXVqdwl1fz8wgPYYu0Wx", "addTeleporters"},
        {"6kX2PJe0FWt3i0fp0tVBh5jt84ozLXBo", "startInHardmode"},
        {"m1gQVuUnIRW083pnfFdnN3DPsg1qFYHZ", "noInfection"},
        {"KYvKIk2LK0oyNY86m+uPhKQ7QbzFmDsR", "hallowOnTheSurface"},
        {"kbxnychxHNDcoyFHhxM9OJHRxis6mFF/", "worldIsInfected"},
        {"e48+tRi5DqzRkBPk3yq9udBG/kaYOQaB", "surfaceIsMushrooms"},
        {"eyGmBQhQ9QnE7UsIib1QmnNRVBNmQtMi", "surfaceIsDesert"},
        {"Iubz1XcBvsfPjSZucIJ3hCDFFEpjG57w", "pooEverywhere"},
        {"SPlOdka0fv8wUovao6u3VB7ZS+IbcPDu", "noSpiderCaves"},
        {"AoEz0g1XX0V/nJwcaN2RWwUf/6ghr9pT", "actuallyNoTraps"},
        {"6lK0Tn4t2UlklesGiJ94617yKvk01ICB", "rainbowStuff"},
        {"MucLvCERZix3rfcwUH68HDtuFYukiTv9", "digExtraHoles"},
        {"VSN8nV180t6PgabWDl4Uf55I1vu97JRD", "roundLandmasses"},
        {"ZYO3rUjSeCaaBrCE8Bv0FBtkjigLMz90", "extraLiquid"},
        {"ALdQZ+bxQA4VdfjVfdhO/sm9q3sZD9dJ", "portalGunInChests"},
        {"eH2IYQwQyOud0hyoTPaeVsqYlAP7MvbS", "worldIsFrozen"},
        {"Z4Odmvd5lScy/KGXHUO2nvqA9l3KRvm8", "halloweenGen"},
        {"KNSxbK83ZXH41aUhWLti9OFMxoMrCV1s", "endlessHalloween"},
        {"gkN386qfe3u1qqQDpGsUu3DsRkEBpD1R", "endlessChristmas"},
        {"4eijvDtfcSl66CDifYSVP3WBZm9OLBoW", "vampirism"},
        {"HnTdmrZ5OT1ldA3r0w3dCgrdLnJBtBSD", "teamBasedSpawns"},
        {"ypBuvKpqKay//OvhG2COriSpGT7f4YY3", "dualDungeons"}
    };

    public static string ToSecret(string plainInput)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(plainInput);
        bytes = new BCrypt().CryptRaw(bytes, _salt, 4);
        
        for (int i = 0; i < 1000; i++)
        {
            int num = i % bytes.Length;
            int num2 = (int)bytes[num] % bytes.Length;
            byte temp = bytes[num];
            bytes[num] = bytes[num2];
            bytes[num2] = temp;
        }
        
        bytes = new BCrypt().CryptRaw(bytes, _salt, 4);
        return Convert.ToBase64String(bytes);
    }
    
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Terraria Secret Seed Cracker");
            Console.WriteLine("Usage:");
            Console.WriteLine("  dotnet run test <seed>");
            Console.WriteLine("  dotnet run wordlist <file>");
            return;
        }
        
        string mode = args[0].ToLower();
        
        if (mode == "test" && args.Length >= 2)
        {
            string input = args[1];
            string cleaned = Regex.Replace(input.ToLower(), "[^a-z0-9]+", "");
            string hash = ToSecret(cleaned);
            
            Console.WriteLine($"Input: {input}");
            Console.WriteLine($"Cleaned: {cleaned}");
            Console.WriteLine($"Hash: {hash}");
            
            if (AllSeeds.ContainsKey(hash))
            {
                Console.WriteLine($"✓ MATCH: {AllSeeds[hash]}");
            }
            else
            {
                Console.WriteLine("✗ No match");
            }
        }
        else if (mode == "wordlist" && args.Length >= 2)
        {
            string file = args[1];
            if (!System.IO.File.Exists(file))
            {
                Console.WriteLine($"Error: File '{file}' not found");
                return;
            }
            
            Console.WriteLine($"Brute forcing with {file}...\n");
            int found = 0;
            
            foreach (string line in System.IO.File.ReadLines(file))
            {
                string word = line.Trim();
                if (string.IsNullOrWhiteSpace(word)) continue;
                
                string cleaned = Regex.Replace(word.ToLower(), "[^a-z0-9]+", "");
                if (string.IsNullOrWhiteSpace(cleaned)) continue;
                
                string hash = ToSecret(cleaned);
                
                if (AllSeeds.ContainsKey(hash))
                {
                    Console.WriteLine($"✓ FOUND: '{word}' -> {AllSeeds[hash]}");
                    found++;
                }
            }
            
            Console.WriteLine($"\nFound {found} seeds!");
        }
        else if (mode == "wordlistcsv" && args.Length >= 2)
        {
            string file = args[1];
            if (!System.IO.File.Exists(file))
            {
                Console.WriteLine($"Error: File '{file}' not found");
                return;
            }
            
            Console.WriteLine($"Brute forcing with {file} (CSV mode)...\n");
            int found = 0;
            
            foreach (string line in System.IO.File.ReadLines(file))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                // 1. Extract the phrase (the first column of the CSV)
                // If it's a standard CSV, the phrase is before the first comma.
                // If the line has no comma, it treats the whole line as the word.
                int commaIndex = line.IndexOf(',');
                string originalPhrase = commaIndex == -1 ? line.Trim() : line.Substring(0, commaIndex).Trim();
                
                if (string.IsNullOrWhiteSpace(originalPhrase)) continue;
                
                // 2. Your existing cleaning logic (IMPORTANT: removes spaces between words)
                // This converts "bigger and boulder" to "biggerandboulder"
                string cleaned = Regex.Replace(originalPhrase.ToLower(), "[^a-z0-9]+", "");
                if (string.IsNullOrWhiteSpace(cleaned)) continue;
                
                // 3. Your existing hashing logic
                string hash = ToSecret(cleaned);
                
                if (AllSeeds.ContainsKey(hash))
                {
                    Console.WriteLine($"✓ FOUND: '{originalPhrase}' -> {AllSeeds[hash]}");
                    found++;
                }
            }
            
            Console.WriteLine($"\nFound {found} seeds!");
        }
    }
}