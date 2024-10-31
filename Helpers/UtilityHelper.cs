using System.Globalization;
using System.Text.RegularExpressions;

namespace BackEndRecursosHumanosFilasur.Helpers
{
    public class UtilityHelper
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                                     @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                                     RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }


        public static async Task<bool> UploadFormFileAsync(string path, string _filename, IFormFile _file)
        {

            bool status = false;
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (var stream = new FileStream(Path.Combine(path, _filename), FileMode.Create))
                {
                    await _file.CopyToAsync(stream);
                }
                status = true;
            }
            catch (System.Exception)
            {
                status = false;
            }

            return status;
        }
    }
}
