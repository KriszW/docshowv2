using System;

namespace EndMethodsLibrary
{
    public static class EndMethods
    {
        public static void End(string msg = "", int exitCode = 100)
        {
            if (msg != "")
            {
                Console.WriteLine($"{DateTime.Now.ToString()}:{msg}");
            }

            Console.WriteLine("Nyomjon entert a kilépéshez...");

            Console.ReadLine();
            Environment.Exit(exitCode);
        }

        public static void UnexpectedEnd()
        {
            //ha valami nem menni jól, akkor a folyamat megállítása
            Console.WriteLine("Valami megszakította a folyamatot");

            End();
        }
    }
}