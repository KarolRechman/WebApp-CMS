using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;

namespace Cit_eTrike.Models
{
    public class DataAccess
    {
        private SqlConnection con;

        /// <summary>
        /// Retrieves connection string from "appsettings.json" file
        /// </summary>
        /// <returns>SqlConnection object</returns>
        public SqlConnection GetCon()
        {
            var config = Configuration();
            //con = new SqlConnection(config.GetSection("ConnectionStrings").GetSection("eTrike").Value);
            con = new SqlConnection(config.GetSection("ConnectionStrings").GetSection("LocalDB_Test").Value);

            return con;
        }

        /// <summary>
        /// Builds configuration object, based on "appsettings.json" file
        /// </summary>
        /// <returns>Configuration object</returns>
        public IConfigurationRoot Configuration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }

        public async Task<List<eTrikeCarousel>> GeteTrikes(int id, string domainName = "")
        {
            List<eTrikeCarousel> eTrikes = new List<eTrikeCarousel>();

            using (GetCon())
            {
                SqlCommand cmd = new SqlCommand("usp_Get_eTrikes", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCategory", id);

                await con.OpenAsync();
                SqlDataReader rdr = await cmd.ExecuteReaderAsync();

                while (await rdr.ReadAsync())
                {
                    eTrikeCarousel eTrike = new eTrikeCarousel();

                    eTrike.IDeTrike = Convert.ToInt32(rdr["IDeTrike"]);
                    eTrike.Name = rdr["Name"].ToString();
                    //eTrike.CategoryName = rdr["Category"].ToString();

                    //if(id != 0)
                    //{
                    //    eTrike.CategoryDescription = rdr["Description"].ToString();
                    //}

                    eTrike.Price = rdr["Price"].ToString();
                    eTrike.PresentationTxt = rdr["PresentationTxt"].ToString();
                    eTrike.TaxTxt = rdr["TaxTxt"].ToString();
                    eTrike.ImgSrc = rdr["ImagesPath"].ToString();

                    eTrike.GetThumb(domainName);

                    eTrikes.Add(eTrike);
                }
                con.Close();
            }
            return eTrikes;
        }

        public async Task<Category> GeteTrikesCategory(int id)
        {
            Category category = new Category();

            using (GetCon())
            {
                SqlCommand cmd = new SqlCommand("usp_Get_eTrikes_Category", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCategory", id);

                await con.OpenAsync();
                SqlDataReader rdr = await cmd.ExecuteReaderAsync();

                while (await rdr.ReadAsync())
                {
                    category.CategoryName = rdr["Name"].ToString();
                    category.CategoryDescription = rdr["Description"].ToString();
                }
                con.Close();
            }
            return category;
        }

        public async Task<List<Category>> GetAlleTrikesCategories()
        {
            List<Category> categories = new List<Category>();

            using (GetCon())
            {
                SqlCommand cmd = new SqlCommand("usp_Get_eTrikes_Category", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCategory", 99);

                await con.OpenAsync();
                SqlDataReader rdr = await cmd.ExecuteReaderAsync();

                while (await rdr.ReadAsync())
                {
                    Category category = new Category();

                    category.IdCategory = Convert.ToInt32(rdr["IdCategory"]);
                    category.CategoryName = rdr["Name"].ToString();
                    category.CategoryDescription = rdr["Description"].ToString();

                    categories.Add(category);
                }
                rdr.Close();

                foreach (var cat in categories)
                {
                    SqlCommand cmd2 = new SqlCommand("usp_Get_eTrikes", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@IdCategory", cat.IdCategory);
                    cmd2.Parameters.AddWithValue("@Available", false);

                    SqlDataReader rdr2 = await cmd2.ExecuteReaderAsync();

                    List<eTrike> trikes = new List<eTrike>();

                    while (await rdr2.ReadAsync())
                    {
                        eTrike eTrike = new eTrike();

                        if (await rdr2.IsDBNullAsync(rdr2.GetOrdinal("IDeTrike")) == false)
                        {
                            eTrike.IDeTrike = Convert.ToInt32(rdr2["IDeTrike"]);
                            eTrike.Name = rdr2["Name"].ToString();
                            eTrike.ImgSrc = rdr2["Description"].ToString();
                            eTrike.Available = Convert.ToBoolean(rdr2["Available"]);
                        }

                        trikes.Add(eTrike);
                    }

                    rdr2.Close();

                    cat.eTrikes = trikes;
                }

                con.Close();
            }
            return categories;
        }

        public async Task<List<eTrikeCarousel>> GeteTrikesCategories(string domainName = "", int IdCategory = 0)
        {
            List<eTrikeCarousel> eTrikes = new List<eTrikeCarousel>();

            using (GetCon())
            {
                SqlCommand cmd = new SqlCommand("usp_Get_eTrikes_Category", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCategory", IdCategory);

                await con.OpenAsync();
                SqlDataReader rdr = await cmd.ExecuteReaderAsync();

                while (await rdr.ReadAsync())
                {
                    eTrikeCarousel eTrike = new eTrikeCarousel();

                    eTrike.IdCategory = Convert.ToInt32(rdr["IdCategory"]);
                    eTrike.CategoryName = rdr["Category"].ToString();
                    eTrike.Price = rdr["Price"].ToString();

                    eTrike.PresentationTxt = rdr["PresentationTxt"].ToString();
                    eTrike.TaxTxt = rdr["TaxTxt"].ToString();
                    eTrike.ImgSrc = rdr["ImagesPath"].ToString();

                    eTrike.GetThumb(domainName);

                    eTrikes.Add(eTrike);
                }
                con.Close();
            }
            return eTrikes;
        }

        public List<eTrikeType> GetAllTypes(int id=0)
        {
            List<eTrikeType> types = new List<eTrikeType>();

            using (GetCon())
            {
                SqlCommand cmd = new SqlCommand("usp_Get_All_Types", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDeTrike", id);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    eTrikeType eTrikeType = new eTrikeType();

                    eTrikeType.IdCategory = Convert.ToInt16(rdr["IdType"]);
                    eTrikeType.CategoryName = rdr["Name"].ToString();
                    eTrikeType.CategoryDescription = rdr["Description"].ToString();

                    types.Add(eTrikeType);

                }
                rdr.Close();
                con.Close();
            }
            return types;
        }

        public List<eTrikeBattery> GeteTrikesBatteries(int id)
        {
            List<eTrikeBattery> eTrikeBatteries = new List<eTrikeBattery>();

            using (GetCon())
            {
                SqlCommand cmd = new SqlCommand("usp_Get_eTrike_Batteries", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDeTrike", id);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    eTrikeBattery eTrikeBattery = new eTrikeBattery();

                    eTrikeBattery.IdBattery = Convert.ToInt16(rdr["IdBattery"]);
                    eTrikeBattery.Name = rdr["Name"].ToString();
                    eTrikeBattery.Power = rdr["Power"].ToString();
                    eTrikeBattery.Price = rdr["Price"].ToString();

                    eTrikeBatteries.Add(eTrikeBattery);

                }
                rdr.Close();
                con.Close();
            }
            return eTrikeBatteries;
        }

        public List<eTrikeBattery> GeteTrikeChargers(int id)
        {
            List<eTrikeBattery> eTrikeChargers = new List<eTrikeBattery>();

            using (GetCon())
            {
                SqlCommand cmd = new SqlCommand("usp_Get_eTrike_Chargers", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDeTrike", id);

                con.Open();
                SqlDataReader rdr3 = cmd.ExecuteReader();
                while (rdr3.Read())
                {
                    eTrikeBattery eTrikeCharger = new eTrikeBattery();

                    eTrikeCharger.IdBattery = Convert.ToInt16(rdr3["IdCharger"]);
                    eTrikeCharger.Name = rdr3["Name"].ToString();
                    eTrikeCharger.Power = rdr3["Power"].ToString();
                    eTrikeCharger.Price = rdr3["Price"].ToString();

                    eTrikeChargers.Add(eTrikeCharger);
                }
                rdr3.Close();
                con.Close();
            }
            return eTrikeChargers;
        }

        public List<eTrikeColor> GeteTrikeColors(int id)
        {
            List<eTrikeColor> eTrikeColors = new List<eTrikeColor>();

            using (GetCon())
            {
                SqlCommand cmd = new SqlCommand("usp_Get_eTrike_Colors", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDeTrike", id);

                con.Open();
                SqlDataReader rdr3 = cmd.ExecuteReader();
                while (rdr3.Read())
                {
                    eTrikeColor eTrikeColor = new eTrikeColor();

                    eTrikeColor.IdColor = Convert.ToInt16(rdr3["IdColor"]);
                    eTrikeColor.Name = rdr3["Name"].ToString();
                    eTrikeColor.RGBCode = rdr3["RGBCode"].ToString();

                    eTrikeColors.Add(eTrikeColor);
                }
                rdr3.Close();
                con.Close();
            }
            return eTrikeColors;
        }

        //public dynamic GetAllFields()
        //{

        //}

        public string AddeTrike(dynamic eTrike, bool Update, int Id = 0)
        {
            Dictionary<string, object> obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(Convert.ToString(eTrike));

            using (GetCon())
            {
                SqlCommand cmd = new SqlCommand("usp_Add_Edit_eTrike", con);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (var o in obj)
                {
                    if (o.Key.Contains("Array") && o.Value.ToString() != "[]")
                    {
                        DataTable dataTable = (DataTable)JsonConvert.DeserializeObject(o.Value.ToString(), (typeof(DataTable)));
                        cmd.Parameters.AddWithValue("@" + o.Key.Replace("Array", ""), dataTable);
                    }
                    else if (o.Value.ToString() != "[]")
                    {
                        cmd.Parameters.AddWithValue("@" + o.Key, o.Value);
                    }
                }

                cmd.Parameters.AddWithValue("@Update", Update);
                cmd.Parameters.AddWithValue("@IdeTrike", Id);

                IDbDataParameter status = cmd.CreateParameter();
                status.ParameterName = "@Status";
                status.Direction = System.Data.ParameterDirection.Output;
                status.DbType = System.Data.DbType.String;
                status.Size = 500;
                cmd.Parameters.Add(status);

                con.Open();
                cmd.ExecuteNonQuery();

                var result = status.Value.ToString();
                return result;
            }
        }

        public string DeleteeTrike(int id)
        {
            using (GetCon())
            {
                SqlCommand cmd = new SqlCommand("usp_Delete_eTrike", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdeTrike", id);

                IDbDataParameter status = cmd.CreateParameter();
                status.ParameterName = "@Status";
                status.Direction = System.Data.ParameterDirection.Output;
                status.DbType = System.Data.DbType.String;
                status.Size = 500;
                cmd.Parameters.Add(status);

                con.Open();
                cmd.ExecuteNonQuery();

                var result = status.Value.ToString();
                return result;
            }
        }

        public dynamic GeteTrikeDesc(int id)
        {
            dynamic eTrikeDesc = new ExpandoObject();
            var marksModel = eTrikeDesc as IDictionary<string, object>;
            string StoredProcedure = "";
            using (GetCon())
            {
                if (id != 0)
                {
                    StoredProcedure = "usp_Get_eTrike_Desc";
                }
                else
                {
                    StoredProcedure = "usp_Get_All_Fields";
                }

                SqlCommand cmd = new SqlCommand(StoredProcedure, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDeTrike", id);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        marksModel.Add(rdr.GetName(i), rdr.GetValue(i));
                    }
                }
                rdr.Close();
                con.Close();
            }
            List<eTrikeBattery> eTrikeBatteries = new List<eTrikeBattery>();
            eTrikeBatteries = GeteTrikesBatteries(id);         
            eTrikeDesc.Batteries = eTrikeBatteries;
            ///////////////////////////////////////////////////////
            List<eTrikeBattery> eTrikeCharges = new List<eTrikeBattery>();
            eTrikeCharges = GeteTrikeChargers(id);          
            eTrikeDesc.Chargers = eTrikeCharges;
            //////////////////////////////////////////////
            List<eTrikeColor> eTrikeColors = new List<eTrikeColor>();
            eTrikeColors = GeteTrikeColors(id);
            eTrikeDesc.Colors = eTrikeColors;

            return eTrikeDesc;
        }

        private class Polices : eTrikeCategory
        {
            public int IdCategory { get; set; }
            public string CategoryName { get; set; }
            public string CategoryDescription { get; set; }
            public List<eTrike> eTrikes { get; set; }
        }

        public eTrikeCategory GetDataPolicy(int id)
        {
            Polices polices = new Polices();
            using (GetCon())
            {
                SqlCommand cmd = new SqlCommand("usp_Get_eTrike_Policies", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    polices.IdCategory = id;
                    polices.CategoryName = rdr["data"].ToString();
                }
                con.Close();
            }
            return polices;
        }

        public async Task<Menu> GetMenu(int id = 0)
        {
            Menu menu = new Menu();
            List<Links> links = new List<Links>();

            using (GetCon())
            {
                if (id == 1)
                {
                    SqlCommand cmd = new SqlCommand("usp_Get_eTrike_Menu2", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", 0);

                    await con.OpenAsync();
                    SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                    while (await rdr.ReadAsync())
                    {
                        Links link = new Links();

                        link.IdLink = Convert.ToInt16(rdr["IdLink"]);
                        link.IdParent = Convert.ToInt16(rdr["IdParent"]);
                        link.Name = rdr["Name"].ToString();
                        link.HTMLidName = rdr["HTMLidName"].ToString();
                        link.HasChildren = Convert.ToBoolean(rdr["HasChildren"]);

                        links.Add(link);
                    }

                    rdr.Close();

                    foreach (var l in links)
                    {
                        if (l.HasChildren == true)
                        {
                            l.DropDowns = GetDropDowns(l.IdLink, 0);
                        }
                    }
                }
                else if (id == 2)
                {
                    SqlCommand cmd = new SqlCommand("usp_Get_Admin_Menu", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@Id", id);

                    await con.OpenAsync();
                    SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                    while (await rdr.ReadAsync())
                    {
                        Links link = new Links();

                        link.IdLink = Convert.ToInt16(rdr["IdMenu"]);
                        link.Name = rdr["Name"].ToString();
                        link.Icon = rdr["Icon"].ToString();
                        link.HTMLidName = rdr["AspPage"].ToString();

                        links.Add(link);
                    }
                }
                //SqlCommand cmd = new SqlCommand("usp_Get_eTrike_Menu2", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Id", id);

                //await con.OpenAsync();
                //SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                //while (await rdr.ReadAsync())
                //{
                //    Links link = new Links();

                //    link.IdLink = Convert.ToInt16(rdr["IdLink"]);
                //    link.IdParent = Convert.ToInt16(rdr["IdParent"]);
                //    link.Name = rdr["Name"].ToString();
                //    link.HTMLidName = rdr["HTMLidName"].ToString();
                //    link.HasChildren = Convert.ToBoolean(rdr["HasChildren"]);

                //    links.Add(link);
                //}

                //rdr.Close();

                //foreach (var l in links)
                //{
                //    if (l.HasChildren == true)
                //    {
                //        l.DropDowns = GetDropDowns(l.IdLink, 0);
                //    }
                //}
                con.Close();
            }

            menu.PageRoute = "null";
            menu.Links = links;
            return menu;
        }

        private List<Links> GetDropDowns(int IdParent = 0, int IdCategory = 0)
        {
            List<Links> DropDowns = new List<Links>();

            //List<Links> DropDowns = new List<Links>();

            SqlCommand cmd2 = new SqlCommand("usp_Get_eTrike_Menu2", con);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@Id", IdParent);
            cmd2.Parameters.AddWithValue("@IdCategory", IdCategory);

            SqlDataReader rdr2 = cmd2.ExecuteReader();
            while (rdr2.Read())
            {
                Links dropdown = new Links();

                dropdown.IdLink = Convert.ToInt16(rdr2["Id"]);
                dropdown.Name = rdr2["Name"].ToString();
                dropdown.HasChildren = Convert.ToBoolean(rdr2["HasChildren"]);

                DropDowns.Add(dropdown);
            }
            rdr2.Close();

            foreach (var l in DropDowns)
            {
                if (l.HasChildren == true)
                {
                    l.DropDowns = GetDropDowns(0, l.IdLink);
                }
            }

            return DropDowns;
        }

        public async Task<List<eTrikieLocation>> GetLocations()
        {
            var locations = new List<eTrikieLocation>();

            using (GetCon())
            {
                SqlCommand cmd = new SqlCommand("usp_Get_eTrikes_Locations", con);
                cmd.CommandType = CommandType.StoredProcedure;

                await con.OpenAsync();

                SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                while (await rdr.ReadAsync())
                {
                    eTrikieLocation location = new eTrikieLocation();

                    location.IdService = Convert.ToInt16(rdr["IdService"]);
                    location.IdServiceCenter = Convert.ToInt16(rdr["IdServiceCenter"]);
                    location.Name = rdr["Name"].ToString();
                    location.Name2 = rdr["Name2"].ToString();
                    location.Country = rdr["Country"].ToString();
                    location.Icon = rdr["Icon"].ToString();
                    location.StateName = rdr["StateName"].ToString();
                    location.Location = rdr["Location"].ToString();
                    location.PostCode = rdr["PostCode"].ToString();
                    location.Address = rdr["Address"].ToString();
                    location.Link = rdr["Link"].ToString();
                    location.Lat = Convert.ToDouble(rdr["Lat"]);
                    location.lng = Convert.ToDouble(rdr["lng"]);

                    locations.Add(location);
                }
                con.Close();
            }
            return locations;
        }
        public async Task<List<TxtContent>> GetContent(int IdSection)
        {
            var txt = new List<TxtContent>();

            using (GetCon())
            {
                SqlCommand cmd = new SqlCommand("usp_Get_Txt_Content", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdSection", IdSection);

                await con.OpenAsync();

                SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                while (await rdr.ReadAsync())
                {
                    var txtContent = new TxtContent();

                    txtContent.IdTextContent = Convert.ToInt16(rdr["IdTextContent"]);
                    txtContent.TextTitle = rdr["TextTitle"].ToString();
                    txtContent.Text = rdr["Text"].ToString();
                    txtContent.HTMLDecoration = rdr["HTMLDecoration"].ToString();

                    txt.Add(txtContent);
                }
                con.Close();
            }
            return txt;
        }
        public string GetEmail()
        {
            string email = "";
            using (GetCon())
            {
                SqlCommand cmd = new SqlCommand("usp_Get_Email", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    email = rdr["email"].ToString();
                }
                con.Close();
            }
            return email;
        }

        public void SetAvailability(int id, bool value)
        {
            using (GetCon())
            {
                SqlCommand cmd = new SqlCommand("usp_Set_Availability", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@value", value);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        //public List<eTrikeBattery> GetAllBatteries(int id = 0)
        //{
        //    List<eTrikeBattery> eTrikeBatteries = new List<eTrikeBattery>();

        //    SqlCommand cmd2 = new SqlCommand("usp_Get_eTrike_Batteries", con);
        //    cmd2.CommandType = CommandType.StoredProcedure;
        //    cmd2.Parameters.AddWithValue("@IDeTrike", id);

        //    SqlDataReader rdr2 = cmd2.ExecuteReader();
        //    while (rdr2.Read())
        //    {
        //        eTrikeBattery eTrikeBattery = new eTrikeBattery();

        //        eTrikeBattery.IdBattery = Convert.ToInt16(rdr2["IdBattery"]);
        //        eTrikeBattery.Name = rdr2["Name"].ToString();
        //        eTrikeBattery.Power = rdr2["Power"].ToString();
        //        eTrikeBattery.Price = rdr2["Price"].ToString();

        //        eTrikeBatteries.Add(eTrikeBattery);
        //    }
        //    rdr2.Close();
        //}

        //public List<eTrikeBattery> GetAllChargers()
        //{

        //}
    }
}

