using System.Text;

namespace PaymentGateway.Application.Utilities
{
    public static class BcdConverter
    {
        public static byte[] StringToBcd(string input)
        {
            if (input.Length % 2 != 0)
            {
                input = "0" + input;
            }

            byte[] bcd = new byte[input.Length / 2];
            for (int i = 0; i < bcd.Length; i++)
            {
                bcd[i] = byte.Parse(input.Substring(2 * i, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return bcd;
        }

        public static string BcdToString(byte[] bcd)
        {
            StringBuilder sb = new StringBuilder(bcd.Length * 2);
            foreach (byte b in bcd)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString().TrimStart('0'); // Remove leading zeros
        }
    }
}
