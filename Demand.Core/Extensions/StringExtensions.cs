using System.Text;
using System.Text.RegularExpressions;

namespace Kep.Helpers.Extensions;

public static class StringExtensions
{
    public static string UppercaseFirstLetter(this string value)
    {
        if (value.Length > 0)
        {
            var array = value.ToCharArray();
            array[0] = char.ToUpper(array[0]);
            for (var i = 1; i < array.Length; i++)
            {
                array[i] = char.ToLower(array[i]);
            }
            return new(array);
        }
        return value;
    }
    public static string GetSpecialCharacter(this string value)
    {
        var length = 1;
        Random random = new();
        const string chars = ".*!-_";
        IEnumerable<string> rndStr = Enumerable.Repeat(chars, length);
        value += new string(rndStr.Select(s => s[random.Next(s.Length)]).ToArray());
        return value;
    }

    public static bool IsInt(this string value)
    {
        if (string.IsNullOrEmpty(value)) return false;

        Int32 tmp;
        return Int32.TryParse(value, out tmp);
    }
    public static bool IsInt64(this string value)
    {
        if (string.IsNullOrEmpty(value)) return false;

        Int64 tmp;
        return Int64.TryParse(value, out tmp);
    }
    public static bool IsDateTime(this string value)
    {
        if (string.IsNullOrEmpty(value)) return false;

        DateTime tmp;
        return DateTime.TryParse(value, out tmp);
    }

    public static bool IsBoolean(this string value)
    {
        var val = value.ToLower().Trim();

        if (val == "1" || val == "0") return true;

        if (val == "true" || val == "false")
            return true;

        return false;
    }

    public static string RemoveRepeatedWhiteSpace(this string value)
    {
        if (value.IsNull()) return string.Empty;

        string[] tmp = value.ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        StringBuilder sb = new();
        foreach (var word in tmp)
        {
            sb.Append(word.Replace("\r", "").Replace("\n", "").Replace("\t", "")/*.Replace("\\" , "")*/ + " ");
        }

        return sb.ToString().TrimEnd();
    }

    public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> value)
    {
        return !value.IsNullOrEmpty();
    }
    public static bool IsNullOrEmpty(this string text)
    {
        return text == null || text.Trim().Length == 0;
    }

    public static bool ValidateIdentityNumber(this string identityNo)
    {
        var regex = new Regex(@"^[1-9]{1}[0-9]{9}[02468]{1}$");

        return identityNo.IsNotNullOrEmpty() && regex.IsMatch(identityNo);
    }

    public static bool ValidateBirthYear(this string birthYear)
    {
        var regex = new Regex(@"^[1-2]{1}[0-9]{3}$");

        return birthYear.IsNotNullOrEmpty() && regex.IsMatch(birthYear);
    }


    public static bool IsNotNullOrEmpty(this string text)
    {
        return !IsNullOrEmpty(text);
    }

    public static bool IsNotNullOrWhiteSpace(this string text)
    {
        return string.IsNullOrWhiteSpace(text);
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> value)
    {
        return value.IsNull() || !value.Any();
    }

    public static bool IsNull(this object objectToCall)
    {
        return objectToCall == null || Convert.IsDBNull(objectToCall);
    }

    public static bool IsNotNull(this object objectToCall)
    {
        return !(objectToCall == null || Convert.IsDBNull(objectToCall));
    }
    public static bool IsNumeric(this string value)
    {
        if (string.IsNullOrEmpty(value)) { return false; }

        decimal convert;
        return decimal.TryParse(value, out convert);
    }

    public static string HexString2B64String(this string input)
    {
        return System.Convert.ToBase64String(input.HexStringToHex());
    }

    public static byte[] HexStringToHex(this string inputHex)
    {
        try
        {
            var resultantArray = new byte[inputHex.Length / 2];
            for (var i = 0; i < resultantArray.Length; i++)
            {
                resultantArray[i] = System.Convert.ToByte(inputHex.Substring(i * 2, 2), 16);
            }
            return resultantArray;
        }
        catch (Exception ex)
        {

            throw new($"Şifreli atılan istek doğru formatta değildir. -> {ex.Message}");
        }
    }

    public static string Masking(this string value)
    {
        if (string.IsNullOrEmpty(value) || value.Length < 2)
            return value;
        else if (value.Length < 3)
            return value.Substring(0, 1) + new string('*', value.Length - 1);
        else
            return value.Substring(0, 1) + new string('*', value.Length - 2) + value.Substring(value.Length - 1);
    }

    public static string EditCityCode(this string cityCode)
    {
        if (cityCode.Length == 1)
        {
            cityCode = "00" + cityCode;
        }
        else if (cityCode.Length == 2)
        {
            cityCode = "0" + cityCode;
        }

        return cityCode;
    }

    public static string ReplaceTurkishCharacters(this string value)
    {
        value = value.Replace("ğ", "g")
            .Replace("Ğ", "G")
            .Replace("ş", "s")
            .Replace("Ş", "S")
            .Replace("ı", "i")
            .Replace("İ", "I")
            .Replace("ü", "u")
            .Replace("Ü", "U")
            .Replace("ö", "o")
            .Replace("Ö", "O")
            .Replace("ç", "c")
            .Replace("Ç", "C");

        return value;
    }

    public static bool ToBoolean(this string value)
    {
        return Convert.ToBoolean(value);
    }

    public static byte ToByte(this string value)
    {
        return Convert.ToByte(value);
    }

    public static Int16 ToInt16(this string value) 
    {
        return Convert.ToInt16(value);
    }
       
    public static Int32 ToInt32(this string value)
    {
        return Convert.ToInt32((value) );
    }

    public static Int64 ToInt64(this string value)
    {
        return Convert.ToInt64((value));
    }

    public static decimal ToDecimal(this string value)
    {
        return Convert.ToDecimal((value));
    }

    public static double ToDouble(this string value)
    {
        return Convert.ToDouble((value));
    }

    public static bool IsValidEmail(this string email)
    {
        var emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

        Regex regex = new(emailPattern);
        return regex.IsMatch(email);
    }
}