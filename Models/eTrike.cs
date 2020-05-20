using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Cit_eTrike.Models
{
    /// <summary>
    /// Interface with basic attributes for eTrike object and derived objects
    /// </summary>
    public interface IeTrike
    {
        public int IDeTrike { get; set; }
        public string ProductNumber { get; set; }
        public string Name { get; set; }
        public string ImgSrc { get; set; }
        public string Price { get; set; }
    }

    /// <summary>
    /// Class that defines eTrike object
    /// </summary>
    public class eTrike : IeTrike
    {
        public int IDeTrike { get; set; }
        public string ProductNumber { get; set; }
        public string Name { get; set; }
        public string ImgSrc { get; set; }
        public string Price { get; set; }
        public bool Available { get; set; }
    }
    //public class eTrikeDesc : eTrike
    //{

    //}
    /// <summary>
    /// Class that defines eTrikeCarousel object, it's used wherever we need an eTrike object with additional category informations and images used in Carousel
    /// </summary>
    public class eTrikeCarousel : IeTrike, eTrikeCategory
    {
        public string PresentationTxt { get; set; }
        public string TaxTxt { get; set; }

        public int IDeTrike { get; set; }
        public string ProductNumber { get; set; }
        public string Name { get; set; }
        public string ImgSrc { get; set; }
        public string Price { get; set; }

        public int IdCategory { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }

        public List<eTrike> eTrikes { get; set; }
        /// <summary>
        /// Method that sets 'ImgSrc' - image path for thumbnails, from
        /// </summary>
        /// <param name="domainName"></param>
        public void GetThumb(string domainName)
        {
            string path;
            string name = ImgSrc.Substring(0, ImgSrc.LastIndexOf("/"));
            name = ImgSrc.Substring(name.LastIndexOf("/")).Replace("/", "");
            path = ImgSrc.Trim() + "Thumbs/" + "Carousel_" + name + ".webp";

            ImgSrc = domainName + path;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class eTrikeBattery
    {
        public int IdBattery { get; set; }
        public string ProductNumber { get; set; }
        public string Name { get; set; }
        public string Power { get; set; }
        public string Price { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class eTrikeColor
    {
        public int IdColor { get; set; }
        public string Name { get; set; }
        public string RGBCode { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public interface eTrikeCategory
    {
        public int IdCategory { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }

        public List<eTrike> eTrikes { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Category : eTrikeCategory
    {
        public int IdCategory { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }

        public List<eTrike> eTrikes { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class eTrikeImg
    {
        public int IdImg { get; set; }
        public string Src { get; set; }
        public string Name { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Menu
    {
        public string PageRoute { get; set; }
        public List<Links> Links { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Links
    {
        public int IdLink { get; set; }
        public int IdParent { get; set; }
        public string Name { get; set; }
        public string HTMLidName { get; set; }
        public string Icon { get; set; }
        public bool HasChildren { get; set; }
        public List<Links> DropDowns { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class eTrikieLocation
    {
        public int IdService { get; set; }
        public int IdServiceCenter { get; set; }
        public string Country { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public string StateName { get; set; }
        public string Location { get; set; }
        public string PostCode { get; set; }
        public string Address { get; set; }
        public string Link { get; set; }
        public double Lat { get; set; }
        public double lng { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class eTrikeType : Category
    {

    }
    /// <summary>
    /// 
    /// </summary>
    public class TxtContent
    {
        public int IdTextContent { get; set; }
        public string TextTitle { get; set; }
        public string Text { get; set; }
        public string HTMLDecoration { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EmailMsg
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}

