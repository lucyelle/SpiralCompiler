namespace SpiralTests;
public class StdInOutTest : TestBase
{
    [Fact]
    public void PascalTriangle()
    {
        var module = Compile("""
            public func choose(n: int, k: int) : int {
                var prod = 1;
                var i = 1;
                while (i <= k) {
                    prod = prod * (n - k + i) / i;
                    i++;
                }
                return prod;
            }

            public func main() {
                var height = 16;
                var y = 0;
                while (y < height) {
                    var pad = height - y - 1;
                    var i = 0;
                    while (i < pad) {
                        print(" ");
                        i++;
                    }

                    var x = 0;
                    while (x < y + 1) {
                        var value = choose(y, x);
                        if (value % 2 == 1) {
                            print("* ");
                        }
                        else {
                            print("  ");
                        }
                        x++;
                    }
                    printLine("");
                    y++;
                }
            }
            
            """);

        var result = TrimTrailingWhitespaces(RunAndCaptureStdOut(module));
        Assert.Equal("""
                           *
                          * *
                         *   *
                        * * * *
                       *       *
                      * *     * *
                     *   *   *   *
                    * * * * * * * *
                   *               *
                  * *             * *
                 *   *           *   *
                * * * *         * * * *
               *       *       *       *
              * *     * *     * *     * *
             *   *   *   *   *   *   *   *
            * * * * * * * * * * * * * * * *
            """,
            result, ignoreLineEndingDifferences: true);
    }
}
