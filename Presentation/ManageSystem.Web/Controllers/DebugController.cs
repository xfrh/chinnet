using System;
using System.Web.Mvc;

namespace ManageSystem.Web.Controllers
{
    /// <summary>
    /// Debug controller for Visual Studio debugging
    /// </summary>
    public class DebugController : Controller
    {
        /// <summary>
        /// Simple test endpoint that doesn't use any services
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Test()
        {
            try
            {
                var debugInfo = new
                {
                    Message = "Debug Controller Working!",
                    Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    MachineName = Environment.MachineName,
                    ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id
                };

                ViewBag.DebugInfo = debugInfo;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.ToString();
                return View("Error");
            }
        }

        /// <summary>
        /// JSON endpoint for AJAX testing
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public JsonResult Status()
        {
            try
            {
                return Json(new
                {
                    success = true,
                    message = "CHINET Application is running in Visual Studio!",
                    timestamp = DateTime.Now,
                    environment = "Development"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Database connection test
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult TestDb()
        {
            try
            {
                // Basic database connection test without using services
                using (var connection = new System.Data.SqlClient.SqlConnection(
                    "Data Source=.;Initial Catalog=ChinetsDB;User ID=sa;Password=123456"))
                {
                    connection.Open();
                    var command = new System.Data.SqlClient.SqlCommand("SELECT @@VERSION", connection);
                    var version = command.ExecuteScalar()?.ToString();

                    ViewBag.DatabaseStatus = "Connected successfully!";
                    ViewBag.SqlVersion = version;
                    connection.Close();
                }

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.DatabaseStatus = "Connection failed!";
                ViewBag.Error = ex.ToString();
                return View();
            }
        }
    }
}
