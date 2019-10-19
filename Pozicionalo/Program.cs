using InItClientProgram;
using System.Threading.Tasks;

namespace DocsShowClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //a shortcut managing elintézése
            ShortcutOperations.SetStartUp();

            //ha kell akkor az ablak elrejtése
            InitMainProgram.Hide();

            //a szükséges paraméterek betöltése
            InitMainProgram.SetUpParams();

            Task.Delay(-1).Wait();
        }
    }
}