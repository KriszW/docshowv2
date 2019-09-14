using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsShowServer
{
    public class OrderUpdateLabelSetter
    {
        Tables MyTable { get; set; }

        public OrderUpdateLabelSetter(Tables table)
        {
            MyTable = table;
        }

        public void SetOrdUpdateTime()
        {
            //ha külső szálból lépne be akkor ez lekezeli
            if (MyTable.LBL_LastUpdate.InvokeRequired)
            {
                MyTable.dgw_Asztalok.BeginInvoke((Action)(() =>
                {
                    SetOrdUpdateTime();
                }));

                return;
            }

            MyTable.LBL_LastUpdate.Text = DateTime.Now.ToString();
        }

        public void SetOrdFileDatas(FileInfo ordInfo)
        {
            //ha külső szálból lépne be akkor ez lekezeli
            if (MyTable.LBL_OrdFileInfos.InvokeRequired)
            {
                MyTable.dgw_Asztalok.BeginInvoke((Action)(() =>
                {
                    SetOrdFileDatas(ordInfo);
                }));

                return;
            }

            MyTable.LBL_OrdFileInfos.Text = $"Név: {ordInfo.Name}\nLétrehozás dátuma: {ordInfo.CreationTime.ToString()}";
        }
    }
}
