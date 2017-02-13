using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertFileNegocio
{
    public class Negocio
    {
        DataSet resultado = new DataSet();
        public  bool ValidarAquivoValido(string extensao)
        {
            if (extensao == ".xls" || extensao == ".xlsx")
            {
                return true;
            }

            return false;
        }

        public bool Processar(string caminhoArquivo, string nomeArquivo, string caminhoArquivoSaida)
        {
            getExcelData(caminhoArquivo);
            var sucesso = converToCSV(nomeArquivo, caminhoArquivoSaida);
            return sucesso; 
        }

        private void getExcelData(string caminhoArquivo)
        {

            if (caminhoArquivo.EndsWith(".xlsx"))
            {
                // Reading from a binary Excel file (format; *.xlsx)
                FileStream stream = File.Open(caminhoArquivo, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                resultado = excelReader.AsDataSet();
                excelReader.Close();
            }

            if (caminhoArquivo.EndsWith(".xls"))
            {
                // Reading from a binary Excel file ('97-2003 format; *.xls)
                FileStream stream = File.Open(caminhoArquivo, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                resultado = excelReader.AsDataSet();
                excelReader.Close();
            }
        }

        private bool converToCSV(string nomeArquivo, string caminhoArquivoSaida)
        {
            try
            {
                int ind = 0;

                string a = "";
                int row_no = 0;

                while (row_no < resultado.Tables[ind].Rows.Count)
                {
                    for (int i = 0; i < resultado.Tables[ind].Columns.Count; i++)
                    {
                        a += resultado.Tables[ind].Rows[row_no][i].ToString() + ",";
                    }
                    row_no++;
                    a += "\n";
                }

                var nome = nomeArquivo.Split('.');
                string output = caminhoArquivoSaida + "\\" + nome[0] + ".csv";
                StreamWriter csv = new StreamWriter(@output, false);
                csv.Write(a);
                csv.Close();
            }
            catch (Exception)
            {
                return false;
            }
           
            return true;
        }
    }
}
