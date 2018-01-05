using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HKClassLibrary
{
    public static class HKValidations
    {

        public static string HKCapitalize(string capitalize)
        {
            if (capitalize==null)
            {
                return capitalize;
            }
            else
            {
                capitalize = capitalize.ToLower().Trim();
                string[] firstLetterCapitalize = capitalize.Split(' ');
                int count = 0;
                capitalize = "";

                foreach (string tempS in firstLetterCapitalize)
                {
                    capitalize += char.ToUpper(tempS[0]) + tempS.Substring(1);
                    if (count != firstLetterCapitalize.Length - 1)
                    {
                        capitalize += ' ';
                    }
                    count++;
                }
                return capitalize;
            }
        }
        public static bool HKPostalCodeValidation(ref string postalCode)
        {

            string pattern = @"^[ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ] ?\d[ABCEGHJKLMNPRSTVWXYZ]\d$";

            Regex reg = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            if (String.IsNullOrEmpty(postalCode))
            {
                return true;
            }
            else
            {
                postalCode = postalCode.Replace(" ",string.Empty);
                if (reg.IsMatch(postalCode))
                {
                    postalCode = postalCode.ToUpper();
                    string temp="";
                    for(int i = 0; i < postalCode.Length; i++)
                    {
                        if (i == 3)
                            temp += " ";
                        temp += postalCode[i];
                    }
                    postalCode = temp;
                    return true;
                }
                else
                    return false;
            }
        }
        public static bool HKZipCodeValidation(ref string postalCode)
        {
            //Zip code : \d{5}(-\d{4})?
            string pattern = @"^\d{5}(-\d{4})?$";

            Regex reg = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            if (!reg.IsMatch(postalCode))
                return false;
            else
            {
                postalCode = postalCode.Trim();
                if (postalCode.Length == 5)
                {
                    postalCode = Regex.Replace(postalCode, @"(\s+|@|&|'|\(|\)|<|>|#)", "");
                }
                else if (postalCode.Length == 9)
                {
                    postalCode = postalCode.Substring(0, 5) + "-" + postalCode.Substring(5);
                }
                return true;
            }
        }
        public static bool HKPhoneNumberValidation(ref string number)
        {
            number = number.Trim();
            number = Regex.Replace(number, "[^a-zA-Z0-9_]+", "");

            if (number.Length != 10)
                return false;

            number = number.Substring(0, 3) +"-"+ number.Substring(3, 3) + "-"+ number.Substring(6);
            return true;
        }
    }
}
