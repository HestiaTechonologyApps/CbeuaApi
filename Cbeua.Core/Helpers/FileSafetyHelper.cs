using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cbeua.Core.Helpers
{
    /// <summary>
    /// Guards for file paths that come from requests or from stored entity fields.
    /// </summary>
    public static class FileSafetyHelper
    {
        // Extensions that a browser or IIS would execute or render as active content
        // when served from wwwroot. Uploads with these extensions are refused.
        private static readonly string[] BlockedExtensions =
        {
            ".html", ".htm", ".xhtml", ".shtml", ".svg", ".svgz", ".xml", ".xsl", ".js", ".mjs",
            ".css", ".swf", ".exe", ".dll", ".bat", ".cmd", ".com", ".ps1", ".vbs", ".msi",
            ".asp", ".aspx", ".ashx", ".asmx", ".axd", ".cshtml", ".config", ".php", ".jsp", ".sh"
        };

        private static readonly Regex SafeSegmentRegex = new("^[A-Za-z0-9_-]{1,64}$", RegexOptions.Compiled);

        /// <summary>True when <paramref name="path"/> resolves to a location inside <paramref name="root"/>.</summary>
        public static bool IsUnderRoot(string root, string? path)
        {
            if (string.IsNullOrWhiteSpace(root) || string.IsNullOrWhiteSpace(path))
                return false;

            try
            {
                var fullRoot = Path.GetFullPath(root).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                               + Path.DirectorySeparatorChar;
                var fullPath = Path.GetFullPath(path);
                return fullPath.StartsWith(fullRoot, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Resolves a web-relative path such as "/profilepics/x.jpg" under <paramref name="folderRoot"/>
        /// (e.g. wwwroot/profilepics). Returns null when the result would escape that folder.
        /// </summary>
        public static string? ResolveUnder(string webRoot, string folderRoot, string? relativeWebPath)
        {
            if (string.IsNullOrWhiteSpace(relativeWebPath))
                return null;

            var candidate = Path.Combine(webRoot,
                relativeWebPath.TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar));
            return IsUnderRoot(folderRoot, candidate) ? Path.GetFullPath(candidate) : null;
        }

        /// <summary>True for a single folder-name segment made of letters, digits, '_' or '-'.</summary>
        public static bool IsSafeSegment(string? segment) =>
            !string.IsNullOrEmpty(segment) && SafeSegmentRegex.IsMatch(segment);

        /// <summary>Strips any directory parts and invalid characters from a client-supplied file name.</summary>
        public static string SanitizeFileName(string? fileName)
        {
            var name = Path.GetFileName((fileName ?? string.Empty).Replace('\\', '/').Split('/').Last());
            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            name = name.Trim().Trim('.');
            return string.IsNullOrEmpty(name) ? "file" : name;
        }

        public static bool IsBlockedExtension(string? fileName)
        {
            var ext = Path.GetExtension(fileName ?? string.Empty);
            return string.IsNullOrEmpty(ext) || BlockedExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase);
        }
    }
}
