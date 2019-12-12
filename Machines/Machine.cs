using KilokoModelLibrary;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Machines
{
    public class Machine
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static List<Machine> Machines { get; set; }

        public static IEnumerable<Machine> Configure(string path)
        {
            if (path.EndsWith(".json"))
            {
                try
                {
                    var text = System.IO.File.ReadAllText(path);
                    return new JavaScriptSerializer().Deserialize<List<Machine>>(text);
                }
                catch (System.IO.FileNotFoundException ex)
                {
                    _logger.Error($"A {path} fájl nem létezik", ex);
                }
                catch (System.IO.IOException ex)
                {
                    _logger.Error($"A {path} fájl olvasása közben hiba lépett fel", ex);
                }
                catch (Exception ex)
                {
                    _logger.Error($"A {path} fájl gép adatokra fordítása közben ismeretlen hiba lépett fel",ex);   
                }

                return default;
            }
            else
            {
                var ex = new ApplicationException("A konfigurációs fájlnak JSON-nek kell lennie");
                _logger.Error($"A {path} fájl nem megfelelő formátumú", ex);
                throw ex;
            }
        }
        public Machine(string iD, int monitorID, string kilokoNum)
        {
            IP = iD ?? throw new ArgumentNullException(nameof(iD));
            MonitorIndex = monitorID;
            KilokoNum = int.Parse(kilokoNum);
        }

        public string IP { get; private set; }
        public int MonitorIndex { get; private set; }
        public int KilokoNum { get; private set; }
        public List<IKilokoModel> Kiloko { get; set; }

        //a discint függvény meghivása miatt kell
        public override bool Equals(object obj) => ((Machine)obj).IP == this.IP;

        public bool IsSame(Machine other)
        {
            return other.MonitorIndex == this.MonitorIndex && other.IP == this.IP;
        }
    }
}