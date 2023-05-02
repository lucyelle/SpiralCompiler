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

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(3, 3, 3)]
    [InlineData(-2, -2, -2)]
    [InlineData(1, 2, 2)]
    [InlineData(4, 3, 4)]
    [InlineData(-3, 3, 3)]
    [InlineData(0, 54, 54)]
    [InlineData(5, -2, 5)]
    [InlineData(5, 0, 5)]
    [InlineData(-4, 0, 0)]
    public void Max(int a, int b, int expectedOutput)
    {
        var module = Compile("""
            public func max(a: int, b: int) : int {
                if (a > b) {
                    return a;
                }
                return b;
            }
            """);

        var output = CallFunction(module, "max", a, b);
        Assert.Equal(expectedOutput, output);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 1, 0)]
    [InlineData(1, 2, 1)]
    [InlineData(3, 5, 7)]
    [InlineData(1, 10, 45)]
    [InlineData(10, 1, 0)]
    public void Sum(int a, int b, int expectedOutput)
    {
        var module = Compile("""
            public func sum(a: int, b: int) : int {
                if (a > b) {
                    return 0;
                }
                var i = a;
                var sum = 0;
                while (i < b) {
                    sum = sum + i;
                    i++;
                }
                return sum;
            }
            """);

        var output = CallFunction(module, "sum", a, b);
        Assert.Equal(expectedOutput, output);
    }


    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 6)]
    [InlineData(5, 120)]
    public void Fact(int n, int expectedOutput)
    {
        var module = Compile("""
            public func fact(x: int): int {
                if (x < 3) {
                    return x;
                }
                return x * fact(x - 1);
            }
            """);

        var output = CallFunction(module, "fact", n);
        Assert.Equal(expectedOutput, output);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    [InlineData(4, 5)]
    [InlineData(5, 8)]
    [InlineData(6, 13)]
    public void Fib(int n, int expectedOutput)
    {
        var module = Compile("""
            public func fib(x: int): int {
                if (x < 2) {
                    return 1;
                }
                return fib(x - 1) + fib(x - 2);
            }
            """);

        var output = CallFunction(module, "fib", n);
        Assert.Equal(expectedOutput, output);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(2, true)]
    [InlineData(3, true)]
    [InlineData(4, false)]
    [InlineData(41, true)]
    [InlineData(99, false)]
    [InlineData(49, false)]
    [InlineData(48, false)]
    [InlineData(101, true)]
    public void IsPrime(int n, bool expectedOutput)
    {
        var module = Compile("""
            public func isPrime(x: int): bool {
                if (x <= 1) {
                    return false;
                }
                var i = 2;
                while (i * i <= x) {
                    if (x % i == 0) {
                        return false;
                    }
                    i++;
                }
                return true;
            }
            """);

        var output = CallFunction(module, "isPrime", n);
        Assert.Equal(expectedOutput, output);
    }
}
