namespace GoldenBook.Helpers
{
    public static class Crypto
    {
        public static string EncodeBase64(string value)
        {
            if (value == null) return null;

            var bytes = System.Text.Encoding.UTF8.GetBytes(value);
            return System.Convert.ToBase64String(bytes);
        }

        public static bool IsPasswordCorrect(string password)
        {
            var passwordEncoded = EncodeBase64(password);
            return passwordEncoded == SecretPasswordEncoded;
        }

        private static string SecretPasswordEncoded = "cm9ja2Fjcm8yMDE3";
    }
}
