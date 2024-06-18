using System.Text.Json.Serialization;

namespace ModrinthPackUpgradeChecker
{
    public class ModrinthPack
    {
        [JsonPropertyName("game")]
        public string Game { get; set; }
        [JsonPropertyName("formatVersion")]
        public int FormatVersion { get; set; }
        [JsonPropertyName("versionId")]
        public string VersionId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("summary")]
        public string Summary { get; set; }
        [JsonPropertyName("files")]
        public ModrinthFile[] Files { get; set; }
        [JsonPropertyName("dependencies")]
        public ModrinthDependencies Dependencies { get; set; }
    }

    public class ModrinthFile
    {
        [JsonPropertyName("path")]
        public string Path { get; set; }
        [JsonPropertyName("hashes")]
        public ModrinthHashes Hashes { get; set; }
        [JsonPropertyName("env")]
        public ModrinthEnv Env { get; set; }
        [JsonPropertyName("downloads")]
        public string[] Downloads { get; set; }
        [JsonPropertyName("fileSize")]
        public int FileSize { get; set; }
    }

    public struct ModrinthHashes
    {
        [JsonPropertyName("sha512")]
        public string Sha512 { get; set; }
        [JsonPropertyName("sha1")]
        public string Sha1 { get; set; }
    }

    public struct ModrinthEnv
    {
        [JsonPropertyName("client")]
        public string Client { get; set; }
        [JsonPropertyName("server")]
        public string Server { get; set; }
    }

    public struct ModrinthDependencies
    {
        [JsonPropertyName("fabric-loader")]
        public string FabricLoaderVersion { get; set; }
        [JsonPropertyName("minecraft")]
        public string MinecraftVersion { get; set; }
    }

    public struct SemanticVersion : IComparable
    {
        public int MajorVersion { get; set; }
        public int MinorVersion { get; set; }
        public int? BuildVersion { get; set; }

        public static SemanticVersion Parse(string version)
        {
            string[] versions = version.Split('.');
            SemanticVersion semVer = new();
            try
            {
                semVer.MajorVersion = int.Parse(versions[0]);
                semVer.MinorVersion = int.Parse(versions[1]);
                if (versions.Length > 2)
                {
                    semVer.BuildVersion = int.Parse(versions[2]);
                }
                return semVer;
            }
            catch
            {
                return new() { MajorVersion = 0, MinorVersion = 0, BuildVersion = 0 };
            }
        }

        public override string ToString()
        {
            return BuildVersion is not null ? $"{MajorVersion}.{MinorVersion}.{BuildVersion}" : $"{MajorVersion}.{MinorVersion}";
        }

        public int CompareTo(object? obj)
        {
            if (obj is not null)
            {
                SemanticVersion otherVersion = (SemanticVersion)obj;
                if (MajorVersion == otherVersion.MajorVersion)
                {
                    if (MinorVersion == otherVersion.MinorVersion)
                    {
                        return (BuildVersion ?? 0) - (otherVersion.BuildVersion ?? 0);
                    }
                    return MinorVersion - otherVersion.MinorVersion;
                }
                return MajorVersion - otherVersion.MajorVersion;
            }
            return -1;
        }
    }
}
