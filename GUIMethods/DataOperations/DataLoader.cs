using IOs;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace DocsShowServer
{
    public class DataLoader
    { 
        Tables MyTable { get; set; }

        public DataLoader(Tables table)
        {
            MyTable = table;
        }

        void LoadKilokok(List<string> machines)
        {
            //egy új lista machine darab kilőkővel
            CommonDatas.Kilokok = new KilokoModel[machines.Count];

            //kezdje el feltölteni a kilőkőket
            for (int i = 0; i < machines.Count; i++)
            {
                //ha nem comment, tehát nem //-el kezdődik
                if (!machines[i].StartsWith("//"))
                {
                    CommonDatas.Kilokok[i] = new KilokoModel(machines[i].Split(';')[0]);
                }
            }
        }

        public void SetUpDefaultLoads()
        {
            //a machine adatok betöltése
            List<string> machines = FileOperations.Read(CommonDatas.GepInfoPath);

            //ha hibás akkor addig csinálja amíg jó nem lesz
            if(machines == null)
            {
                DataChecking.CheckTheExcelIsOpened(CommonDatas.GepInfoPath);

                machines = FileOperations.Read(CommonDatas.GepInfoPath);
            }

            //töltse be a GUI adatokat
            LoadMachines(machines);
            //inicialízálja a Kilőkőlistát
            LoadKilokok(machines);
        }

        void LoadMachines(List<string> machines)
        {
            //a GUI feltöltése
            foreach (var item in machines) {
                string[] Splitter = item.Split(';');

                MyTable.dgw_Asztalok.Rows.Add();

                int index = MyTable.dgw_Asztalok.Rows.GetLastRow(DataGridViewElementStates.Visible);

                MyTable.dgw_Asztalok.Rows[index].Cells["col_asztal"].Value = Splitter[0];
                MyTable.dgw_Asztalok.Rows[index].Cells["col_Cikk"].Value = "";
                MyTable.dgw_Asztalok.Rows[index].Cells["col_standBal"].Value = "";
                MyTable.dgw_Asztalok.Rows[index].Cells["col_standJobb"].Value = "";
                MyTable.dgw_Asztalok.Rows[index].Cells["col_dbSzam"].Value = "Fejlesztés alatt";
                MyTable.dgw_Asztalok.Rows[index].Cells["col_monSzam"].Value = int.Parse(Splitter[2])+1;
            }
            MyTable.dgw_Asztalok.Sort(MyTable.dgw_Asztalok.Columns["col_asztal"], ListSortDirection.Ascending);
        }
    }
}
