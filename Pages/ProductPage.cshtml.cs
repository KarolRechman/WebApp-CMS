using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cit_eTrike.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cit_eTrike.Pages
{
    public class ProductPageModel : PageModel
    {
        public DataAccess dataAccess = new DataAccess();
        private Helper helper = new Helper();

        public dynamic eTrikeDesc { get; set; }    

        public void OnGet(int id)
        {
            eTrikeDesc = dataAccess.GeteTrikeDesc(id);
            if (helper.IsPropertyExist(eTrikeDesc, "ImagesPath") == true)
            {

                string url = "/images/eTrikes" + eTrikeDesc.ImagesPath.ToString();

                string path = eTrikeDesc.ImagesPath.ToString();
                path = path.Replace("/", "");

                //var file = Path.Combine(Directory.GetCurrentDirectory(),
                //         "MyStaticFiles", "images", "banner1.svg");

                var directoryFiles = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "eTrikes", path));
                directoryFiles = directoryFiles.Where(df => !df.Contains("Thumbs")).ToArray();
                string domainName = HttpContext.Request.PathBase.Value.ToString();

                List<eTrikeImg> eTrikeImgs = new List<eTrikeImg>();
                int number = 1;
                foreach (string filePath in directoryFiles)
                {
                    string fileName = Path.GetFileName(filePath);

                    eTrikeImgs.Add(new eTrikeImg
                    {
                        IdImg = number,
                        Name = fileName.Split('.')[0].ToString(),
                        Src = domainName + url + fileName.ToString(),
                    }); ;
                    number += 1;
                }

                var dirThumbs = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "eTrikes", path, "Thumbs"));
                dirThumbs = dirThumbs.Where(dt => !dt.Contains("Carousel")).ToArray();
                url = url + "/Thumbs/";

                List<eTrikeImg> eTrikeImgsThumbs = new List<eTrikeImg>();
                number = 1;
                foreach (string filePath in dirThumbs)
                {
                    string fileName = Path.GetFileName(filePath);

                    eTrikeImgsThumbs.Add(new eTrikeImg
                    {
                        IdImg = number,
                        Name = fileName.Split('.')[0].ToString(),
                        Src = domainName + url + fileName.ToString(),
                    }); ;
                    number += 1;
                }

                eTrikeDesc.Img = eTrikeImgs;
                eTrikeDesc.ImgThumbs = eTrikeImgsThumbs;
            }
        }
    }
}
