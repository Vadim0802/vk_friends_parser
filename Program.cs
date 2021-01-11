using System;
using System.Threading.Tasks;

namespace VKParse
{
    internal static class Program
    {
        private static async Task Main()
        {
            await Parser.Run("213646078");
        }
    }
}