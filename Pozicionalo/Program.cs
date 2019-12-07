namespace DocsShowClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //a shortcut managing elintézése
            InItClientProgram.ShortcutOperations.SetStartUp();

            //ha kell akkor az ablak elrejtése
            InItClientProgram.InitMainProgram.Hide();

            //a szükséges paraméterek betöltése
            InItClientProgram.InitMainProgram.SetUpParams();

            System.Console.ReadLine();
        }
    }
}