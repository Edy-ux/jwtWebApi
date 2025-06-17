using System.Text.RegularExpressions;

namespace JwtWebApi.Helpers
{
    public static class LocalizationHelper
    {
        /// <summary>
        /// Remove placeholders como {0}, {1:...} de mensagens localizadas,
        /// para facilitar comparações com valores dinâmicos (ex: datas, logins).
        /// </summary>
        /// 
        public static string RemovePlaceholders(string input)
        {
            return Regex.Replace(input, @"\{\d+(:[^}]*)?\}", "").Trim();
        }
    }
}