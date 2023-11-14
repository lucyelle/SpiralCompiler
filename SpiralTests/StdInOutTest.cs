namespace SpiralTests;
public class StdInOutTest : TestBase
{
    [Fact]
    public void PascalTriangle()
    {
        var module = Compile("""
            func choose(n: int, k: int) : int {
                var prod = 1;
                var i = 1;
                while (i <= k) {
                    prod = prod * (n - k + i) / i;
                    i = i + 1;
                }
                return prod;
            }

            func main() {
                var height = 16;
                var y = 0;
                while (y < height) {
                    var pad = height - y - 1;
                    var i = 0;
                    while (i < pad) {
                        print(" ");
                        i = i + 1;
                    }

                    var x = 0;
                    while (x < y + 1) {
                        var value = choose(y, x);
                        if (value % 2 == 1) {
                            print("* ");
                        }
                        else {
                            print("+ ");
                        }
                        x = x + 1;
                    }
                    println("");
                    y = y + 1;
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
