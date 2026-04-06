namespace Test;

using Xunit;
using Cleverence.task1;

public class StringCompressorTests
{
    [Theory]
    [InlineData("aaabbcccdde", "a3b2c3d2e")]
    [InlineData("abc", "abc")]
    [InlineData("aaaaa", "a5")]
    [InlineData("aabb", "a2b2")]
    [InlineData("", "")]
    [InlineData("a", "a")]
    public void Compress_ShouldReturnExpectedString(string input, string expected)
    {
        string result = StringCompressor.Compress(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("a3b2c3d2e", "aaabbcccdde")]
    [InlineData("abc", "abc")]
    [InlineData("a5", "aaaaa")]
    [InlineData("a10", "aaaaaaaaaa")] // Проверка многозначных чисел
    [InlineData("", "")]
    public void Decompress_ShouldReturnOriginalString(string compressed, string expected)
    {
        string result = StringCompressor.Decompress(compressed);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void RoundTrip_ShouldRestoreOriginalString()
    {
        // Проверка: сжатие + декомпрессия = исходная строка
        string original = "aaabbbcccdddeeefffg";
        string compressed = StringCompressor.Compress(original);
        string decompressed = StringCompressor.Decompress(compressed);

        Assert.Equal(original, decompressed);
    }
}
