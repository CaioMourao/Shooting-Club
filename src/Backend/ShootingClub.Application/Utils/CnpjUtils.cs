using System.Text.RegularExpressions;

namespace ShootingClub.Application.Utils
{
    public static class CnpjUtils
    {
        public static string Format(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return cnpj;

            if (Regex.IsMatch(cnpj, @"^\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}$"))
                return cnpj;

            if (ValidCNPJ(cnpj))
                return cnpj;

            var digits = new string(cnpj.Where(char.IsDigit).ToArray());
            if (digits.Length != 14)
                return cnpj;

            return $"{digits.Substring(0, 2)}.{digits.Substring(2, 3)}.{digits.Substring(5, 3)}/{digits.Substring(8, 4)}-{digits.Substring(12, 2)}";
        }

        public static bool ValidCNPJ(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return false;

            var digits = new string(cnpj.Where(char.IsDigit).ToArray());
            if (digits.Length != 14)
                return false;

            if (digits.Distinct().Count() == 1)
                return false;

            var nums = digits.Select(c => c - '0').ToArray();

            int[] pesos1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma1 = 0;
            for (int i = 0; i < 12; i++)
                soma1 += nums[i] * pesos1[i];
            int resto1 = soma1 % 11;
            int dig1 = resto1 < 2 ? 0 : 11 - resto1;
            if (nums[12] != dig1)
                return false;

            int[] pesos2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma2 = 0;
            for (int i = 0; i < 13; i++)
                soma2 += nums[i] * pesos2[i];
            int resto2 = soma2 % 11;
            int dig2 = resto2 < 2 ? 0 : 11 - resto2;
            if (nums[13] != dig2)
                return false;

            return true;
        }
    }
}
