using System.Text.RegularExpressions;

namespace JwtWebApi.Helpers
{
    public static class LocalizationHelper
    {
        /// <summary>
        ///  Remove placeholders como {0}, {1:...} de mensagens 
        ///    visando facilitar a comparação de mensagens localizadas com valores dinâmicos (ex: datas, logins).
        /// </summary>
        /// 
        public static string RemovePlaceholders(string input)
        {
            return Regex.Replace(input, @"\{\d+(:[^}]*)?\}", "").Trim();
        }
    }
}