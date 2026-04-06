namespace Cleverens.task1;

using System.Text;

public static class StringCompressor
{
    /// <summary>
    /// Сжимает строку: "aaabb" -> "a3b2". Одиночные символы остаются без цифр.
    /// </summary>
    public static string Compress(string input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;

        var sb = new StringBuilder();
        int n = input.Length;

        for (int i = 0; i < n; i++)
        {
            int count = 1;
            // Считаем количество одинаковых символов подряд
            while (i + 1 < n && input[i] == input[i + 1])
            {
                count++;
                i++;
            }

            sb.Append(input[i]);
            if (count > 1)
            {
                sb.Append(count);
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// Декомпрессия строки: "a3b2" -> "aaabb".
    /// </summary>
    public static string Decompress(string compressed)
    {
        if (string.IsNullOrEmpty(compressed)) return string.Empty;

        var sb = new StringBuilder();
        int n = compressed.Length;

        for (int i = 0; i < n; i++)
        {
            char symbol = compressed[i];
            int start = i + 1;
            
            // Собираем число (может быть больше 9)
            while (i + 1 < n && char.IsDigit(compressed[i + 1]))
            {
                i++;
            }

            int count = 1;
            if (i >= start)
            {
                // Используем Span для парсинга без создания лишних подстрок
                count = int.Parse(compressed.AsSpan(start, i - start + 1));
            }

            sb.Append(symbol, count);
        }

        return sb.ToString();
    }
}
