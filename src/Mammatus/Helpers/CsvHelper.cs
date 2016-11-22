using System.Data;
using System.IO;

namespace Mammatus.Helpers
{
    public static class CsvHelper
    {
        public static bool ToCsv(DataTable dt, string strFilePath, string tableheader, string columname)
        {
            try
            {
                StreamWriter strmWriterObj = new StreamWriter(strFilePath, false, System.Text.Encoding.UTF8);
                strmWriterObj.WriteLine(tableheader);
                strmWriterObj.WriteLine(columname);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var strBufferLine = "";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j > 0)
                            strBufferLine += ",";
                        strBufferLine += dt.Rows[i][j];
                    }
                    strmWriterObj.WriteLine(strBufferLine);
                }
                strmWriterObj.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static DataTable FromCsv(string filePath, int n, DataTable dt)
        {
            using (var reader = new StreamReader(filePath, System.Text.Encoding.UTF8, false))
            {
                int m = 0;
                reader.Peek();
                while (reader.Peek() > 0)
                {
                    m = m + 1;
                    string str = reader.ReadLine();
                    if (m >= n + 1)
                    {
                        string[] split = str.Split(',');

                        System.Data.DataRow dr = dt.NewRow();
                        int i;
                        for (i = 0; i < split.Length; i++)
                        {
                            dr[i] = split[i];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                return dt;
            }
        }
    }
}
