using System;
using System.Windows.Forms;

namespace DocsShowServer
{
    public class KilokoSetter
    {
        Tables MyTable { get; set; }

        public KilokoSetter(Tables table)
        {
            MyTable = table;
        }

        #region kiloko state changer

        //állítsa a kilőkőt offlinera azon a kilőkőszámon ami meg van addva
        public void SetKilokoOffline(string kilokoszam)
        {
            for (int i = 0; i < MyTable.dgw_Asztalok.Rows.Count; i++)
            {
                DataGridViewRow row = MyTable.dgw_Asztalok.Rows[i];

                if (row.Cells["col_asztal"].Value.ToString() == kilokoszam)
                {
                    if (MyTable.dgw_Asztalok.InvokeRequired)
                    {
                        MyTable.dgw_Asztalok.BeginInvoke((Action)(() =>
                        {
                            row.Cells["Col_Active"].Value = false;
                        }));
                    }

                    break;
                }
            }
        }

        //állítsa a kilőkőt onlinera azon a kilőkőszámon ami meg van addva
        public void SetKilokoOnline(string kilokoszam)
        {
            for (int i = 0; i < MyTable.dgw_Asztalok.Rows.Count; i++)
            {
                DataGridViewRow row = MyTable.dgw_Asztalok.Rows[i];

                if (row.Cells["col_asztal"].Value.ToString() == kilokoszam)
                {
                    if (MyTable.dgw_Asztalok.InvokeRequired) {
                        MyTable.dgw_Asztalok.BeginInvoke((Action)(() => 
                        {
                            row.Cells["Col_Active"].Value = true;
                        }));
                    }
                    break;
                }
            }
        }

        #endregion
    }
}
