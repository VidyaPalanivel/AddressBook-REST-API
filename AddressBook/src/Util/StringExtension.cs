using System.Text.RegularExpressions;

namespace AddressBook.Util
{
    /// <summary>
    /// Class <c>StringExtensions</c> an extension class consists of string manipulation
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// This method converts the string to snake case.
        /// </summary>
        /// <param name="input">The string input to be converted as snake case.</param>
        /// <returns>A string convered to snake case.</returns>
        /// <example>
        /// Use this method with any string to be converted as snake case
        /// Usage: string.ConvertToSnakeCase()
        /// For e.g. The given input string is PersonName will be converted to 
        /// person_name.
        /// </example>
        public static string ConvertToSnakeCase(this string input)
        {
            if (System.String.IsNullOrEmpty(input)) { return input; }

            Match startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        public static string Encode(string Password)
        {
                try{
                    byte[] EncDataByte = new byte[Password.Length];
                    EncDataByte=System.Text.Encoding.UTF8.GetBytes(Password);
                    string EncryptedData = Convert.ToBase64String(EncDataByte);
                    return EncryptedData;
                }
                catch( Exception ex)
                {
                    throw new Exception("Error in Encode: "+ex.Message);
                }
        }
        public static string DecodeFrom64(string Password) 
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding(); 
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(Password); 
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length); 
                char[] decoded_char = new char[charCount]; 
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0); 
                string result = new String(decoded_char); 
                return result;
            }
    }
}