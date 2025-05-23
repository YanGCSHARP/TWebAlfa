using System.Threading.Tasks;
using System.Web;

namespace LNP.Core.Interfaces.Services
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(HttpPostedFileBase file);
    }
}