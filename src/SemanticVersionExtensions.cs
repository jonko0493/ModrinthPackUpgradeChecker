using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModrinthPackUpgradeChecker
{
    public static class SemanticVersionExtensions
    {
        public static SemanticVersion ParseLoose(string versionString)
        {
            if (SemanticVersion.TryParse(versionString, out SemanticVersion? version))
            {
                return version ?? new(0, 0, 0);
            }
            string[] components = versionString.Split('.');
            if (components.Length == 2)
            {
                if (SemanticVersion.TryParse($"{versionString}.0", out SemanticVersion? versionWith0))
                {
                    return versionWith0 ?? new(0, 0, 0);
                }
            }

            return new(0, 0, 0);
        }
    }
}
