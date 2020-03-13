using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TestingCoe800.Models;
using System.Data.OleDb;


namespace TestingCoe800.Controllers
{
    [Authorize(Roles = "Manager")]
    public class StoreItemsController : Controller
    {
      
        private UsersDBEntities db = new UsersDBEntities();

        
        // GET: Store__Items
        public ActionResult Index()
        {
            string Manager_Id = User.Identity.GetUserId().ToString();

            var storeItems = db.StoreItems.Include(s => s.Store).Where(s=> s.Store.ManagerIDFk == Manager_Id);

            return View(storeItems.ToList());
        }

        // GET: Store__Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreItem storeItem = db.StoreItems.Find(id);
            if (storeItem == null)
            {
                return HttpNotFound();
            }
            return View(storeItem);
        }

        // GET: Store__Items/Create
        public ActionResult Create()
        {
         string Manager_Id = User.Identity.GetUserId().ToString();
        ViewBag.StoreIDFk = new SelectList(db.Stores.Where(g => g.ManagerIDFk.ToString() == Manager_Id ) , "Id", "StoreName");
            return View();
        }

        // POST: Store__Items/Create      
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ItemName,StoreIDFk,Price")] StoreItem storeItem)
        {
            if (ModelState.IsValid)
            {
                db.StoreItems.Add(storeItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.StoreIDFk = new SelectList(db.Stores, "Id", "StoreName", storeItem.StoreIDFk);
            return View(storeItem);
        }

        // GET: Store__Items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreItem storeItem = db.StoreItems.Find(id);
            if (storeItem == null)
            {
                return HttpNotFound();
            }
            string Manager_Id = User.Identity.GetUserId().ToString();
            ViewBag.StoreIDFk = new SelectList(db.Stores.Where(g => g.ManagerIDFk.ToString() == Manager_Id), storeItem.StoreIDFk);
            return View(storeItem);
        }

        // POST: Store__Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ItemName,Price")] StoreItem storeItem)
        {
            if (ModelState.IsValid)
            {
                storeItem.StoreIDFk = db.StoreItems.Where(d => d.Id  == storeItem.Id).Select(d => d.StoreIDFk).SingleOrDefault();
                db.Entry(storeItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StoreIDFk = new SelectList(db.Stores, "Id", "StoreName", storeItem.StoreIDFk);
            return View(storeItem);
        }

        // GET: Store__Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreItem storeItem = db.StoreItems.Find(id);
            if (storeItem == null)
            {
                return HttpNotFound();
            }
            return View(storeItem);
        }

        // POST: Store__Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StoreItem storeItem = db.StoreItems.Find(id);
            db.StoreItems.Remove(storeItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Import()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Import (HttpPostedFileBase postedFile)
        {

            if (postedFile != null)
            {
                try
                {
                    string fileExtension = Path.GetExtension(postedFile.FileName);

                    //Validate uploaded file and return error.
                    if (fileExtension != ".xls" && fileExtension != ".xlsx")
                    {
                        ViewBag.Message = "Please select the excel file with .xls or .xlsx extension";
                        return View();
                    }

                    string folderPath = Server.MapPath("~/UploadedFiles/");
                    //Check Directory exists else create one
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    //Save file to folder
                    var filePath = folderPath + Path.GetFileName(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    //Get file extension

                    string excelConString = "";

                    //Get connection string using extension 
                    switch (fileExtension)
                    {
                        //If uploaded file is Excel 1997-2007.
                        case ".xls":
                            excelConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                            break;
                        //If uploaded file is Excel 2007 and above
                        case ".xlsx":
                            excelConString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                            break;
                    }

                    //Read data from first sheet of excel into datatable
                    DataTable dt = new DataTable();
                    excelConString = string.Format(excelConString, filePath);

                    using (OleDbConnection excelOledbConnection = new OleDbConnection(excelConString))
                    {
                        using (OleDbCommand excelDbCommand = new OleDbCommand())
                        {
                            using (OleDbDataAdapter excelDataAdapter = new OleDbDataAdapter())
                            {
                                excelDbCommand.Connection = excelOledbConnection;

                                excelOledbConnection.Open();
                                //Get schema from excel sheet
                                DataTable excelSchema = GetSchemaFromExcel(excelOledbConnection);
                                //Get sheet name
                                string sheetName = excelSchema.Rows[0]["TABLE_NAME"].ToString();
                                excelOledbConnection.Close();

                                //Read Data from First Sheet.
                                excelOledbConnection.Open();
                                excelDbCommand.CommandText = "SELECT * From [" + sheetName + "]";
                                excelDataAdapter.SelectCommand = excelDbCommand;
                                //Fill datatable from adapter
                                excelDataAdapter.Fill(dt);
                                excelOledbConnection.Close();
                            }
                        }
                    }

                    //Insert records to Employee table.
                    using (var context = new UsersDBEntities());
                    {
                        //Loop through datatable and add employee data to employee table. 
                        foreach (DataRow row in dt.Rows)
                        {

                            db.StoreItems.Add(GetItem(row));
                        }
                        db.SaveChanges();
                    }
                    ViewBag.Message = "Data Imported Successfully.";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                }
            }
            else
            {
                ViewBag.Message = "Please select the file first to upload.";
            }
            return View();
        }

        private static DataTable GetSchemaFromExcel(OleDbConnection excelOledbConnection)
        {
            return excelOledbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        }

        //Convert each datarow into employee object
        private StoreItem GetItem(DataRow row)
        {
            return new StoreItem
            {
                Id = int.Parse(row[0].ToString()),
                StoreIDFk = int.Parse(row[1].ToString()),
                ItemName = row[2].ToString(),
                Price = (row[3].ToString())
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
