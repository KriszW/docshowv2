using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DocsShowServer
{
    public class DataGridViewManager
    {
        Tables MyTable { get; set; }

        public DataGridViewManager(Tables table)
        {
            MyTable = table;
        }

        #region manage datas in datagridview

        void LoadDataGridView(string asztal, string stand1, string stand2, string cikk)
        {
            //az adatok bevezetése a GUIba
            foreach (var item in MyTable.dgw_Asztalok.Rows)
            {
                //a row kikényszerítése
                DataGridViewRow row = (DataGridViewRow)item;

                //ha az asztal megegyezik akkor inicialízálja az adatokat
                if (asztal == row.Cells["col_asztal"].Value.ToString())
                {
                    row.Cells["col_Cikk"].Value = cikk;

                    row.Cells["col_standBal"].Value = stand1;
                    row.Cells["col_standJobb"].Value = stand2;

                    //ha végzet akkor úgyse lesz több találat
                    break;
                }
            }
        }

        void ManageDGV()
        {
            //refreshelje és állítsa sorrendbe
            try
            {
                MyTable.dgw_Asztalok.Sort(MyTable.dgw_Asztalok.Columns[0], ListSortDirection.Ascending);
                MyTable.dgw_Asztalok.Refresh();
            }
            catch (Exception)
            {

            }
        }

        public void ReplaceInfo(string asztal, string stand1, string stand2, string cikk)
        {
            //ha külső szálból lépne be a frissitésbe akkor nem fogja lefagyasztani a GUI szálat
            if (MyTable.dgw_Asztalok.InvokeRequired)
            {
                MyTable.dgw_Asztalok.BeginInvoke((Action)(() => 
                {
                    ReplaceInfo(asztal, stand1, stand2, cikk);
                }));

                return;
            }

            //a megfelelő kilőkőmodel megszerzése
            IKilokoModel kiloko = DataOperations.GetKilokoModel(asztal);

            if (kiloko==null)
            {
                return;
            }

            //ha az egyik standard üres akkor állítsa be a "Nincs PDF" szöveget
            if (stand1 == "" || stand1.Contains("Nincs semmi") || stand1.Contains("/"))
            {
                stand1 = "Nincs PDF";
            }
            if (stand2 == "" || stand2.Contains("Nincs semmi") || stand2.Contains("/"))
            {
                stand2 = "Nincs PDF";
            }

            stand1 = stand1.Split('#')[0];
            stand2 = stand2.Split('#')[0];

            //inicialízálja az adatokat
            LoadDataGridView(asztal, stand1, stand2, kiloko.GetGUICikk(cikk));

            //frissítse a datagridviewt
            ManageDGV();
        }

        #endregion

    }
}
