using System.Text.RegularExpressions;

namespace ShootingClub.Application.Utils
{
    public static class CpfUtils
    {
        public static string Format(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return cpf;

            if (!ValidCPF(cpf))
                return cpf;

            if (Regex.IsMatch(cpf, @"^\d{3}\.\d{3}\.\d{3}-\d{2}$"))
                return cpf;

            cpf = new string(cpf.Where(char.IsDigit).ToArray());
            if (cpf.Length != 11)
                return cpf;

            return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
        }

        public static bool ValidCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            var digits = new string(cpf.Where(char.IsDigit).ToArray());

            if (digits.Length != 11)
                return false;

            if (digits.Distinct().Count() == 1)
                return false;

            var nums = digits.Select(c => c - '0').ToArray();

            int soma1 = 0;
            for (int i = 0; i < 9; i++)
                soma1 += nums[i] * (10 - i);

            int resto1 = (soma1 * 10) % 11;
            if (resto1 == 10) resto1 = 0;
            if (nums[9] != resto1)
                return false;

            int soma2 = 0;
            for (int i = 0; i < 10; i++)
                soma2 += nums[i] * (11 - i);

            int resto2 = (soma2 * 10) % 11;
            if (resto2 == 10) resto2 = 0;
            if (nums[10] != resto2)
                return false;

            return true;
        }
    }
}
