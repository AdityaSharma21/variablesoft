using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VariableSoft.Models;
using WebMatrix.WebData;

namespace VariableSoft.Controllers
{
    public class userController : Controller
    {
        public ActionResult userlist()
        {
            List<UserProfile> UserProfileList = new List<UserProfile>();
            try
            {
                using (var context = new UsersContext())
                {

                    var data = (from tbl in context.UserProfiles select new { UserId = tbl.UserId, UserFullName = tbl.UserFullName, StateId = tbl.StateId, Pincode = tbl.Pincode, Mobile = tbl.Mobile, Logo = tbl.Logo, Email = tbl.Email, DistrictId = tbl.DistrictId, CityName = tbl.CityName }).ToList();
                    foreach (var item in data)
                    {
                        UserProfile userdata = new UserProfile();
                        userdata.UserId = item.UserId;
                        userdata.UserFullName = item.UserFullName;
                        userdata.StateId = item.StateId;
                        userdata.Pincode = item.Pincode;
                        userdata.Mobile = item.Mobile;
                        userdata.Logo = item.Logo;
                        userdata.Email = item.Email;
                        userdata.DistrictId = item.DistrictId;
                        userdata.CityName = item.CityName;
                        UserProfileList.Add(userdata);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View(UserProfileList);
        }

        [HttpGet]
        public ActionResult adduser(long? UserId)
        {
            UserProfile userprofile = new UserProfile();
            try
            {
                if (UserId != null && UserId != 0)
                {
                    
                    ViewBag.buttontext = "Save";
                    ViewBag.Title = "Edit User";
                    using (var context = new UsersContext())
                    {
                        userprofile = context.UserProfiles.SingleOrDefault(c => c.UserId == UserId);
                    }
                    ViewBag.districtddl = districtlist(userprofile.StateId.ToString());
                }
                else
                {
                    ViewBag.Title = "Add User";
                     
                    ViewBag.buttontext = "Create";
                    List<SelectListItem> districtddl = new List<SelectListItem>();
                    districtddl.Add(new SelectListItem {Text="Select",Value="" });
                    ViewBag.districtddl = districtddl;
                }
                ViewBag.stateddl = statelist();
            }
            catch (Exception ex)
            {

            }
            return View(userprofile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult adduser(UserProfile userprofile, HttpPostedFileBase file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Database.SetInitializer<UsersContext>(null);
                    string filename = "NoImage.png";
                    string ImagePath = Server.MapPath("~/uploads/");
                    if (file != null)
                    {
                        filename = System.IO.Path.GetFileName(file.FileName.Replace(" ", ""));
                        file.SaveAs(Server.MapPath("/uploads/" + filename));
                    }
                    else if (string.IsNullOrEmpty(userprofile.Logo))
                    {
                        
                        userprofile.Logo = "/uploads/" + filename; 
                    }
                    using (var context = new UsersContext())
                    {
                        if (!context.Database.Exists())
                        {
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                            WebSecurity.InitializeDatabaseConnection("UserContext", "UserProfile", "UserId", "UserFullName", autoCreateTables: true);
                        }
                        if(userprofile.UserId!=0)
                        {
                            UserProfile userdata = context.UserProfiles.SingleOrDefault(c => c.UserId == userprofile.UserId);
                            var data = context.UserProfiles.Any(c => c.UserFullName == userprofile.UserFullName&&c.UserId!=userprofile.UserId);
                            if (!data)
                            {
                                userdata.UserFullName = userprofile.UserFullName;
                                userdata.StateId = userprofile.StateId;
                                userdata.Pincode = userprofile.Pincode;
                                userdata.Mobile = userprofile.Mobile;
                                userdata.Logo = userprofile.Logo;
                                userdata.Email = userprofile.Email;
                                userdata.DistrictId = userprofile.DistrictId;
                                userdata.CityName = userprofile.CityName;
                                context.SaveChanges();
                                return RedirectToAction("userlist");
                            }
                            else
                            {
                                return RedirectToAction("userlist");
                            }
                        }
                        else
                        {
                            context.UserProfiles.Add(userprofile);
                            context.SaveChanges();
                            return RedirectToAction("userlist");
                        }
                    }
                }
                else
                {
                    return RedirectToAction("userlist", new { userprofile.UserId });
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("userlist", new { userprofile.UserId });
            }

        }

        

        public JsonResult getstatelist()
        {
            List<SelectListItem> statelist = new List<SelectListItem>();
            try
            {
                statelist.Add(new SelectListItem { Text = "Select",Value="" });
                statelist.Add(new SelectListItem { Text = "Andra Pradesh", Value = "1" });
                statelist.Add(new SelectListItem { Text = "Arunachal Pradesh", Value = "2" });
                statelist.Add(new SelectListItem { Text = "Assam", Value = "3" });
                statelist.Add(new SelectListItem { Text = "Bihar", Value = "4" });
                statelist.Add(new SelectListItem { Text = "Chandigarh", Value = "5" });
                statelist.Add(new SelectListItem { Text = "Chhatisgarh", Value = "6" });
                statelist.Add(new SelectListItem { Text = "Delhi", Value = "7" });
                statelist.Add(new SelectListItem { Text = "Goa", Value = "8" });
                statelist.Add(new SelectListItem { Text = "Gujrat", Value = "9" });
                statelist.Add(new SelectListItem { Text = "Haryana", Value = "10" });
                statelist.Add(new SelectListItem { Text = "Himachal Pradesh", Value = "11" });
                statelist.Add(new SelectListItem { Text = "J & k", Value = "12" });
                statelist.Add(new SelectListItem { Text = "Jharkhand", Value = "13" });
                statelist.Add(new SelectListItem { Text = "Karnatka", Value = "14" });
                statelist.Add(new SelectListItem { Text = "Kerela", Value = "15" });
                statelist.Add(new SelectListItem { Text = "Madhya Pradesh", Value = "16" });
                statelist.Add(new SelectListItem { Text = "Maharashtra", Value = "17" });
                statelist.Add(new SelectListItem { Text = "Manipur", Value = "18" });
                statelist.Add(new SelectListItem { Text = "Meghalaya", Value = "19" });
                statelist.Add(new SelectListItem { Text = "Mizoram", Value = "20" });
                statelist.Add(new SelectListItem { Text = "Nagaland", Value = "21" });
                statelist.Add(new SelectListItem { Text = "Odisha", Value = "22" });
                statelist.Add(new SelectListItem { Text = "Punjab", Value = "23" });
                statelist.Add(new SelectListItem { Text = "Rajasthan", Value = "24" });
                statelist.Add(new SelectListItem { Text = "Sikkim", Value = "25" });
                statelist.Add(new SelectListItem { Text = "Tamil Nadu", Value = "26" });
                statelist.Add(new SelectListItem { Text = "Telangana", Value = "27" });
                statelist.Add(new SelectListItem { Text = "Tripura", Value = "28" });
                statelist.Add(new SelectListItem { Text = "Uttar Pradesh", Value = "29" });
                statelist.Add(new SelectListItem { Text = "Uttarakhand", Value = "30" });
                statelist.Add(new SelectListItem { Text = "WestBangal", Value = "31" });

            }
            catch(Exception ex)
            {
                statelist.Add(new SelectListItem { Text = "Select", Value = "" });
            }
            return Json(statelist,JsonRequestBehavior.AllowGet);
        }


        public JsonResult getdistrictlist( string value)
        {
            return Json(districtlist(value), JsonRequestBehavior.AllowGet);
        }

        private List<SelectListItem> statelist()
        {
            List<SelectListItem> statelist = new List<SelectListItem>();
            statelist.Add(new SelectListItem { Text = "Select", Value = "" });
            statelist.Add(new SelectListItem { Text = "Andra Pradesh", Value = "1" });//Anantapur,Chittoor,EastGodawri,Guntur
            statelist.Add(new SelectListItem { Text = "Arunachal Pradesh", Value = "2" });//Anjaw,Changlang,Debang Valley
            statelist.Add(new SelectListItem { Text = "Assam", Value = "3" });//Baksa,Barpeta,Dispur
            statelist.Add(new SelectListItem { Text = "Bihar", Value = "4" });//Araria,Arwal,Orangabad
            statelist.Add(new SelectListItem { Text = "Chandigarh", Value = "5" });//Chandigarh
            statelist.Add(new SelectListItem { Text = "Chhatisgarh", Value = "6" });//Balod,Baloda Bazar,Balrampur
            statelist.Add(new SelectListItem { Text = "Delhi", Value = "7" });//Central Delhi,North Delhi,East Delhi,New Delhi
            statelist.Add(new SelectListItem { Text = "Goa", Value = "8" });//North Goa,South Goa
            statelist.Add(new SelectListItem { Text = "Gujrat", Value = "9" });//Ahemdabad,GandhiNagar,Jamnagar
            statelist.Add(new SelectListItem { Text = "Haryana", Value = "10" });//Gurugram,Ambala,Biwani
            statelist.Add(new SelectListItem { Text = "Himachal Pradesh", Value = "11" });//Chamba,Bilaspur,Hameerpur
            statelist.Add(new SelectListItem { Text = "J & k", Value = "12" });//Shrinagar,anantnag,udhampur
            statelist.Add(new SelectListItem { Text = "Jharkhand", Value = "13" });//Bokaro,chatra,dhanbad
            statelist.Add(new SelectListItem { Text = "Karnatka", Value = "14" });//bagalkot,ballari,belagavi
            statelist.Add(new SelectListItem { Text = "Kerela", Value = "15" });//trivendrum,kannoor,kollam
            statelist.Add(new SelectListItem { Text = "Madhya Pradesh", Value = "16" });//Bhopal,Indore,Ujjain
            statelist.Add(new SelectListItem { Text = "Maharashtra", Value = "17" });//Bombay,Pune,Khandala
            statelist.Add(new SelectListItem { Text = "Manipur", Value = "18" });//Vishnupur,Chandel,Senapati
            statelist.Add(new SelectListItem { Text = "Meghalaya", Value = "19" });//Eastgarohills,eastkhasihills,northgarohills
            statelist.Add(new SelectListItem { Text = "Mizoram", Value = "20" });//Aizwal,champhai,Kolasir
            statelist.Add(new SelectListItem { Text = "Nagaland", Value = "21" });//Dimapur,kohima,Mon
            statelist.Add(new SelectListItem { Text = "Odisha", Value = "22" });//Angul,Bhubneshwar,Cuttak
            statelist.Add(new SelectListItem { Text = "Punjab", Value = "23" });//Amritsar,Bhatinda,Firozpur
            statelist.Add(new SelectListItem { Text = "Rajasthan", Value = "24" });//Jaipur,Kota,Jhalawar
            statelist.Add(new SelectListItem { Text = "Sikkim", Value = "25" });//East Sikkim,North Sikkim,South Sikkim
            statelist.Add(new SelectListItem { Text = "Tamil Nadu", Value = "26" });//Chennai,Dharmapuri,Madurai
            statelist.Add(new SelectListItem { Text = "Telangana", Value = "27" });//Adilabad,Hyedrabad,Jagtiag
            statelist.Add(new SelectListItem { Text = "Tripura", Value = "28" });//Dhalai,Gomati,Khowai
            statelist.Add(new SelectListItem { Text = "Uttar Pradesh", Value = "29" });//Agra,Aligarh,Allahbad
            statelist.Add(new SelectListItem { Text = "Uttarakhand", Value = "30" });//Almora,Chamoli,Haridwar
            statelist.Add(new SelectListItem { Text = "WestBangal", Value = "31" });//Alipur,Duar,Bankura
            return statelist;
        }


        private List<SelectListItem> districtlist(string value)
        {
            List<SelectListItem> districtlist = new List<SelectListItem>();
            try
            {
                districtlist.Add(new SelectListItem { Text = "Select", Value = "" });
                switch (value)
                {
                    case "1":
                        districtlist.Add(new SelectListItem { Text = "EastGodawri", Value = "1" });
                        districtlist.Add(new SelectListItem { Text = "Chittoor", Value = "2" });
                        districtlist.Add(new SelectListItem { Text = "Guntur", Value = "3" });
                        break;
                    case "2":
                        districtlist.Add(new SelectListItem { Text = "Anjaw", Value = "4" });
                        districtlist.Add(new SelectListItem { Text = "Changlang", Value = "5" });
                        districtlist.Add(new SelectListItem { Text = "Debang", Value = "6" });
                        break;

                    case "3":
                        districtlist.Add(new SelectListItem { Text = "Baksa", Value = "7" });
                        districtlist.Add(new SelectListItem { Text = "Barpeta", Value = "8" });
                        districtlist.Add(new SelectListItem { Text = "Dispur", Value = "9" });
                        break;

                    case "4":
                        districtlist.Add(new SelectListItem { Text = "Araria", Value = "10" });
                        districtlist.Add(new SelectListItem { Text = "Arwal", Value = "11" });
                        districtlist.Add(new SelectListItem { Text = "Orangabad", Value = "12" });
                        break;
                    case "5":
                        districtlist.Add(new SelectListItem { Text = "Chandigarh", Value = "13" });
                        break;
                    case "6":
                        districtlist.Add(new SelectListItem { Text = "Balod", Value = "14" });
                        districtlist.Add(new SelectListItem { Text = "Baloda Bazar", Value = "15" });
                        districtlist.Add(new SelectListItem { Text = "Balrampur", Value = "16" });
                        break;
                    case "7":
                        districtlist.Add(new SelectListItem { Text = "Central Delhi", Value = "17" });
                        districtlist.Add(new SelectListItem { Text = "North Delhi", Value = "18" });
                        districtlist.Add(new SelectListItem { Text = "East Delhi", Value = "19" });
                        break;

                    case "8":
                        districtlist.Add(new SelectListItem { Text = "Central Goa", Value = "20" });
                        districtlist.Add(new SelectListItem { Text = "North Goa", Value = "21" });
                        districtlist.Add(new SelectListItem { Text = "East Goa", Value = "22" });
                        break;

                    case "9":
                        districtlist.Add(new SelectListItem { Text = "Ahemdabad", Value = "23" });
                        districtlist.Add(new SelectListItem { Text = "GandhiNagar", Value = "24" });
                        districtlist.Add(new SelectListItem { Text = "Jamnagar", Value = "25" });
                        break;
                    case "10":
                        districtlist.Add(new SelectListItem { Text = "Gurugram", Value = "26" });
                        districtlist.Add(new SelectListItem { Text = "Ambala", Value = "27" });
                        districtlist.Add(new SelectListItem { Text = "Biwani", Value = "28" });
                        break;

                    case "11":
                        districtlist.Add(new SelectListItem { Text = "Chamba", Value = "29" });
                        districtlist.Add(new SelectListItem { Text = "Bilaspur", Value = "30" });
                        districtlist.Add(new SelectListItem { Text = "Hameerpur", Value = "31" });
                        break;

                    case "12":
                        districtlist.Add(new SelectListItem { Text = "Shrinagar", Value = "32" });
                        districtlist.Add(new SelectListItem { Text = "anantnag", Value = "33" });
                        districtlist.Add(new SelectListItem { Text = "udhampur", Value = "34" });
                        break;
                    case "13":
                        districtlist.Add(new SelectListItem { Text = "Bokaro", Value = "35" });
                        districtlist.Add(new SelectListItem { Text = "chatra", Value = "36" });
                        districtlist.Add(new SelectListItem { Text = "dhanbad", Value = "37" });
                        break;
                    case "14":
                        districtlist.Add(new SelectListItem { Text = "bagalkot", Value = "38" });
                        districtlist.Add(new SelectListItem { Text = "ballari", Value = "39" });
                        districtlist.Add(new SelectListItem { Text = "belagavi", Value = "40" });
                        break;
                    case "15":
                        districtlist.Add(new SelectListItem { Text = "trivendrum", Value = "41" });
                        districtlist.Add(new SelectListItem { Text = "kannoor", Value = "42" });
                        districtlist.Add(new SelectListItem { Text = "kollam", Value = "43" });
                        break;
                    case "16":
                        districtlist.Add(new SelectListItem { Text = "Bhopal", Value = "44" });
                        districtlist.Add(new SelectListItem { Text = "Indore", Value = "45" });
                        districtlist.Add(new SelectListItem { Text = "Ujjain", Value = "46" });
                        break;
                    case "17":
                        districtlist.Add(new SelectListItem { Text = "Bombay", Value = "47" });
                        districtlist.Add(new SelectListItem { Text = "Pune", Value = "48" });
                        districtlist.Add(new SelectListItem { Text = "Khandala", Value = "49" });
                        break;
                    case "18":
                        districtlist.Add(new SelectListItem { Text = "Vishnupur", Value = "50" });
                        districtlist.Add(new SelectListItem { Text = "Chandel", Value = "51" });
                        districtlist.Add(new SelectListItem { Text = "Senapati", Value = "52" });
                        break;
                    case "19":
                        districtlist.Add(new SelectListItem { Text = "Eastgarohills", Value = "53" });
                        districtlist.Add(new SelectListItem { Text = "eastkhasihills", Value = "54" });
                        districtlist.Add(new SelectListItem { Text = "northgarohills", Value = "55" });
                        break;
                    case "20":
                        districtlist.Add(new SelectListItem { Text = "Aizwal", Value = "56" });
                        districtlist.Add(new SelectListItem { Text = "champhai", Value = "57" });
                        districtlist.Add(new SelectListItem { Text = "Kolasir", Value = "58" });
                        break;

                    case "21":
                        districtlist.Add(new SelectListItem { Text = "Dimapur", Value = "59" });
                        districtlist.Add(new SelectListItem { Text = "kohima", Value = "60" });
                        districtlist.Add(new SelectListItem { Text = "Mon", Value = "61" });
                        break;
                    //,,,
                    case "22":
                        districtlist.Add(new SelectListItem { Text = "Angul", Value = "62" });
                        districtlist.Add(new SelectListItem { Text = "Bhubneshwar", Value = "62" });
                        districtlist.Add(new SelectListItem { Text = "Cuttak", Value = "63" });
                        break;

                    case "23":
                        districtlist.Add(new SelectListItem { Text = "Amritsar", Value = "64" });
                        districtlist.Add(new SelectListItem { Text = "Bhatinda", Value = "65" });
                        districtlist.Add(new SelectListItem { Text = "Firozpur", Value = "66" });
                        break;

                    case "24":
                        districtlist.Add(new SelectListItem { Text = "Jaipur", Value = "67" });
                        districtlist.Add(new SelectListItem { Text = "Kota", Value = "68" });
                        districtlist.Add(new SelectListItem { Text = "Jhalawar", Value = "69" });
                        break;
                    case "25":
                        districtlist.Add(new SelectListItem { Text = "East Sikkim", Value = "70" });
                        districtlist.Add(new SelectListItem { Text = "North Sikkim", Value = "71" });
                        districtlist.Add(new SelectListItem { Text = "South Sikkim", Value = "72" });
                        break;
                    case "26":
                        districtlist.Add(new SelectListItem { Text = "Chennai", Value = "73" });
                        districtlist.Add(new SelectListItem { Text = "Dharmapuri", Value = "74" });
                        districtlist.Add(new SelectListItem { Text = "Madurai", Value = "75" });
                        break;
                    case "27":
                        districtlist.Add(new SelectListItem { Text = "Adilabad", Value = "76" });
                        districtlist.Add(new SelectListItem { Text = "Hyedrabad", Value = "77" });
                        districtlist.Add(new SelectListItem { Text = "Jagtiag", Value = "78" });
                        break;

                    case "28":
                        districtlist.Add(new SelectListItem { Text = "Dhalai", Value = "79" });
                        districtlist.Add(new SelectListItem { Text = "Gomati", Value = "80" });
                        districtlist.Add(new SelectListItem { Text = "Khowai", Value = "81" });
                        break;

                    case "29":
                        districtlist.Add(new SelectListItem { Text = "Agra", Value = "82" });
                        districtlist.Add(new SelectListItem { Text = "Aligarh", Value = "83" });
                        districtlist.Add(new SelectListItem { Text = "Allahbad", Value = "84" });
                        break;
                    case "30":
                        districtlist.Add(new SelectListItem { Text = "Almora", Value = "85" });
                        districtlist.Add(new SelectListItem { Text = "Chamoli", Value = "86" });
                        districtlist.Add(new SelectListItem { Text = "Haridwar", Value = "87" });
                        break;

                    case "31":
                        districtlist.Add(new SelectListItem { Text = "Alipur", Value = "88" });
                        districtlist.Add(new SelectListItem { Text = "Duar", Value = "89" });
                        districtlist.Add(new SelectListItem { Text = "Bankura", Value = "90" });
                        break;
                }

            }
            catch (Exception ex)
            {
                districtlist.Add(new SelectListItem { Text = "Select", Value = "" });
            }
            return districtlist;
        }
    }
}
