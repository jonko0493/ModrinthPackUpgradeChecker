using Modrinth;
using Modrinth.Models;
using Mono.Options;
using System.IO.Compression;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ModrinthPackUpgradeChecker
{
    internal partial class Program
    {
        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        public async static Task MainAsync(string[] args)
        {
            string mrpackPath = "", mcVersion = "";
            bool inclusive = false;
            OptionSet options = new()
            {
                { "p|m|pack|mrpack=", "Modrinth modpack to analyze", p => mrpackPath = p },
                { "v|version=", "Minecraft version to check to upgrade to; blank to get the latest available version of each mod", v => mcVersion = v },
                { "i|inclusive", "If set, will show mods that have the specified version rather than mods that don't", i => inclusive = true },
            };
            options.Parse(args);

            Console.WriteLine($"Parsing pack '{mrpackPath}' and looking for {(string.IsNullOrEmpty(mcVersion) ? "latest version" : mcVersion)}...");

            ZipArchive zip = ZipFile.OpenRead(mrpackPath);
            string indexJson = new StreamReader(zip.Entries.First(f => f.Name == "modrinth.index.json").Open()).ReadToEnd();
            ModrinthPack mrpack = JsonSerializer.Deserialize<ModrinthPack>(indexJson) ?? new();

            Console.WriteLine($"Found {mrpack.Files.Length} mods. Fetching details from modrinth...");

            ModrinthClient client = new();
            Regex slugRegex = SlugRegex();

            async Task<(string, string, string[])> GetModSupportedVersions(string cdnUri)
            {
                Project mod = await client.Project.GetAsync(slugRegex.Match(cdnUri).Groups["slug"].Value);
                return (mod.Title, mod.Slug, mod.GameVersions);
            }

            IEnumerable<Task<(string, string, string[])>> gameVersionFetches = mrpack.Files.Select(m => GetModSupportedVersions(m.Downloads.First()));
            (string, string, string[])[] modGameVersions = await Task.WhenAll(gameVersionFetches);

            int compatibleMods = 0;
            Console.WriteLine();

            foreach ((string title, string slug, string[] gameVersions) in modGameVersions.OrderBy(m => m))
            {
                if (string.IsNullOrEmpty(mcVersion))
                {
                    Console.WriteLine($"{title} ({slug}): {gameVersions.OrderByDescending(v => v).First()}");
                }
                else
                {
                    if (gameVersions.Contains(mcVersion) && inclusive)
                    {
                        Console.WriteLine($"{title} ({slug})");
                    }
                    else if (!gameVersions.Contains(mcVersion) && !inclusive)
                    {
                        Console.WriteLine($"{title} ({slug}): {gameVersions.OrderByDescending(v => v).First()}");
                    }
                    
                    if (gameVersions.Contains(mcVersion))
                    {
                        compatibleMods++;
                    }
                }
            }

            Console.WriteLine();

            if (!string.IsNullOrEmpty(mcVersion))
            {
                if (inclusive)
                {
                    Console.WriteLine($"{compatibleMods} compatible mods found ({mrpack.Files.Length - compatibleMods} mods incompatible)");
                }
                else
                {
                    Console.WriteLine($"{mrpack.Files.Length - compatibleMods} incompatible mods found ({compatibleMods} mods compatible)");
                }
            }
        }

        [GeneratedRegex(@"https:\/\/cdn\.modrinth\.com\/data\/(?<slug>[\w\d]+)\/")]
        private static partial Regex SlugRegex();
    }
}
