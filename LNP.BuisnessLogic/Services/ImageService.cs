using System.IO;
using System.Web;

namespace LNP.BuisnessLogic.Services
{
    public class ImageService
    {
        public string SaveImage(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0) 
                return string.Empty;

            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images"), fileName);
            file.SaveAs(path);
            return "/Content/Images/" + fileName;
        }
    }
}