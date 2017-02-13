using ConvertFileNegocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcConvertFile.Controllers
{
    public class HomeController : Controller
    {
        private Negocio negocio = new Negocio();

        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult UploadArquivo(HttpPostedFileBase arquivo)
        {
            try
            {
                if (arquivo == null)
                {
                    ViewBag.Message = "Informe o arquivo para converter.";
                    return View("Aviso");
                }

                if (arquivo.ContentLength > 0)
                {
                    string extensaoArquivo = Path.GetExtension(arquivo.FileName);

                    if (!negocio.ValidarAquivoValido(extensaoArquivo))
                    {
                        ViewBag.Message = "Formato do arquivo é inválido.";
                        return View("Error");
                    }

                    string nomeArquivo = Path.GetFileName(arquivo.FileName);
                    string caminho = Path.Combine(Server.MapPath("~/ArquivosOriginais"), nomeArquivo);
                    arquivo.SaveAs(caminho);

                    if (negocio.Processar(caminho, nomeArquivo, Server.MapPath("~/Convertidos")))
                    {
                        var nome = nomeArquivo.Split('.');
                        return DownloadArquivo(Server.MapPath("~/Convertidos") + "\\" + nome[0] + ".csv", nome[0] + ".csv");
                    }
                }
               
               return View("Sucess");
            }
            catch
            {
                ViewBag.Message = "Erro ao converter arquivo";
                return View("Error");
            }
        }

        public FileResult DownloadArquivo(string nomeArquivo, string fileName)
        {
            string contentType = "text/csv";
            return File(nomeArquivo, contentType, fileName);
        }
    }
}