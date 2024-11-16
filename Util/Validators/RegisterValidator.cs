using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace BackendLaboratory.Util.Validators
{
    public static class RegisterValidator
    {
        public static bool IsEmailValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }
            try
            {
                MailAddress m = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsBirthDateValid(DateTime? birthday)
        {
            if (birthday == null) { return true; }

            DateTime minDate = new DateTime(1915, 1, 1, 0, 0, 0);
            DateTime maxDate = new DateTime(2050, 12, 31, 0, 0, 0);

            return birthday >= minDate && birthday <= maxDate && birthday <= DateTime.Now;
        }

        public static bool IsFullnameValid(string name)
        {
            return Regex.IsMatch(name, @"^[А-ЯЁ][а-яёА-ЯЁа-яё-]*(?:-[А-ЯЁ][а-яё]*)?\s[А-ЯЁ][а-яёА-ЯЁа-яё-]*(?:-[А-ЯЁ][а-яё]*)?(?:\s[А-ЯЁ][а-яёА-ЯЁа-яё-]*(?:-[А-ЯЁ][а-яё]*)?)?$");
        }

        public static bool IsPasswordStrong(string? password)
        {
            if (string.IsNullOrWhiteSpace(password)) { return true; }

            bool hasUpperCase = Regex.IsMatch(password, @"[A-ZА-Я]");
            bool hasLowerCase = Regex.IsMatch(password, @"[a-zа-я]");
            bool hasDigit = Regex.IsMatch(password, @"\d");
            bool hasSpecialChar = Regex.IsMatch(password, @"[!@#$%^&*(),.?""{}|<>]");

            return hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar;
        }
        
        public static bool IsPhoneValid(string name)
        {
            return Regex.IsMatch(name, @"^(\+7)\s?\(?\d{3}\)?\s?\d{3}[-\s]?\d{2}[-\s]?\d{2}$");
        }
    }
}
