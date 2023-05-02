namespace SpiralTests;
public class FunctionTest : TestBase
{
    [Theory]
    [InlineData(-2, 2)]
    [InlineData(2, 2)]
    [InlineData(0, 0)]
    [InlineData(-13, 13)]
    [InlineData(40, 40)]
    public void Abs(int input, int expectedOutput)
    {
        var module = Compile("""
            public func abs(x: int) : int {
                if (x >= 0) {
                    return x;
                }
                return -x;
            }
            """);

        var output = CallFunction(module, "abs", input);
        Assert.Equal(expectedOutput, output);
    }
}
// sum, max, fact ...
