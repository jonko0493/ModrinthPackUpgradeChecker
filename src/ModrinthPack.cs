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
}
