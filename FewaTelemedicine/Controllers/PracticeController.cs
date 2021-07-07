

#region Hospital Controller Description 
/* This file contains Definition of  Methods for Login Patient, Get and Add Waiting Room,Doctor Cabin,Updated Doctor,
 * ProfileUpdate and Update Parameter.
 */
#endregion
#region Log History
/* #39 6/9/2020 - Bhavana => Added Email Template Changes in Get Updated Doctor.
*  #40 6/9/2020 - Bhavana => Updated  Added Upload Option to update for Doctor Logo.
 */
#endregion 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FewaTelemedicine.Common;
using FewaTelemedicine.Domain;
using FewaTelemedicine.Domain.Models;
using FewaTelemedicine.Domain.Repositories;
using FewaTelemedicine.Domain.Services;
using FewaTelemedicine.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace FewaTelemedicine.Controllers
{
    [EnableCors("MyPolicy")]
    [Authorize]
    public class PracticeController : Controller
    {
        private readonly ILogger<PracticeController> _logger;
        private readonly IProviderRepository _providerRepository;
        List<ProviderCabin> _providerCabins = null;
        private readonly IHttpContextAccessor accessor;
        WaitingRoom _waitingroom = null;
        private readonly IPatientRepository _patientRepository;
        List<Provider> _providers = null;
        private int idletime = 0;
        private readonly IHubContext<NotificationHub,
            INotificationHub> _notify;
        private readonly IConfiguration _config;
        private IWebHostEnvironment _hostingEnvironment;
        private FewaDbContext FewaDbContext = null;
        public IConfiguration Configuration { get; }

        public PracticeController(
            ILogger<PracticeController> logger,
            List<ProviderCabin> providerCabins,
            WaitingRoom waitingroom,
            IConfiguration configuration,
            List<Provider> providers,
            IHubContext<NotificationHub,
            INotificationHub> notify,
            IConfiguration config,
            IPatientRepository patientRepository,
            IProviderRepository providerRepository,
            IWebHostEnvironment hostEnvironment,
            FewaDbContext fewaDbContext,
            IHttpContextAccessor HttpContextAccessor)
        {
            FewaDbContext = fewaDbContext;
            _patientRepository = patientRepository;
            _providers = providers;
            Configuration = configuration;
            _logger = logger;
            _providerCabins = providerCabins;
            _waitingroom = waitingroom;
            _providerRepository = providerRepository;
            idletime = Convert.ToInt32(configuration["IdleTime"]);
            _notify = notify;
            _config = config;
            accessor = HttpContextAccessor;
            _hostingEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult GetPracticeConfiguration(string practice, string key)
        {
            try
            {
                if (key == "73l3M3D")
                {
                    if (!string.IsNullOrEmpty(practice))
                {
                    return Ok(FewaDbContext.practices.Where(a => a.url.ToLower().Trim() == practice.ToLower().Trim()).FirstOrDefault());
                }
                List<Practice> result = FewaDbContext.practices.ToList();
                return Ok(result);
                }
                else
                {
                    return Ok("wrongKey");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }

        }
        [AllowAnonymous]
        public IActionResult LoginPatient([FromBody] Patient obj)
        {
            if (!(GetPatientbyName(obj.name) is null))
            {
                return StatusCode(500, "Patient already logged in");
            }
            HttpContext.Session.SetString("practice",obj.practice);
            var provider = (from temp in FewaDbContext.providers
                            where temp.url == obj.providerNameAttending 
                               && temp.practice==obj.practice
                            select temp).FirstOrDefault();
            obj.lastUpdated = DateTime.Now;
            obj.providerId = provider.providerId;
            obj.practiceId = provider.practiceId;
            string userAgent = HttpContext.GetServerVariable("HTTP_USER_AGENT");
            Regex OS = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex device = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string device_info = string.Empty;
            if (OS.IsMatch(userAgent))
            {
                device_info = OS.Match(userAgent).Groups[0].Value;
            }
            if (device.IsMatch(userAgent.Substring(0, 4)))
            {
                device_info += device.Match(userAgent).Groups[0].Value;
            }
            if (!string.IsNullOrEmpty(device_info))
            {
                obj.isMobile = true;
            }
            _waitingroom.patients.Add(obj);
            SecurityController securityController = new SecurityController(null, null, _config, null, null);
            var token = securityController.GenerateJSONWebToken(obj.name, "Patient",obj.providerId,obj.practiceId);
            var result = new
            {
                User = obj,
                Token = token
            };
            return Ok(result);
            // return Ok(Json(obj));
        }

        public List<Patient> GetPatientsAttended([FromBody] Provider obj,[Optional] string searchString)
        {
            var attendedPatients = new List<Patient>();
            //Add Optional paramter if value is in parameter then filter.
            // Else no filter.
            // if no search text then today's all records and if no today's records
            // then top 10 records.
            // if value in search text then  return values matching with search text.
            if (string.IsNullOrEmpty(searchString)|| obj==null)
            {
                DateTime startDateTime = DateTime.Today; //Today at 00:00:00
                DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
                /* Display Today's Records */
                attendedPatients = (from temp in FewaDbContext.patients
                                    where (temp.appointmentDate >=
                                    startDateTime && temp.appointmentDate <= endDateTime)&&(temp.providerId == obj.providerId&& 
                                   temp.practiceId==obj.practiceId)
                                    orderby temp.startTime descending
                                    select temp
                                   ).ToList<Patient>();
                /*Display  Previous Records  if no today's records */
                if (attendedPatients.Count <= 0)
                {
                    attendedPatients = (from temp in FewaDbContext.patients
                                        where(temp.providerId== obj.providerId&& temp.practiceId == obj.practiceId)
                                        orderby temp.startTime, temp.appointmentDate descending
                                        select temp
                                  ).OrderByDescending(a => a.startTime).Take(10).ToList<Patient>();

                }
            }
            else if (!string.IsNullOrEmpty(searchString)|| obj!=null)
            {
                /* Display Records Matching With SearchString */
                attendedPatients = (from temp in FewaDbContext.patients
                                    where
                                    (
                                    temp.appointmentDate.Month.ToString().Contains(searchString) ||
                                    temp.appointmentDate.Date.ToString().Contains(searchString) ||
                                    temp.appointmentDate.Year.ToString().Contains(searchString))&&
                                    (temp.providerId == obj.providerId && temp.practiceId == obj.practiceId)
                                    orderby temp.appointmentDate descending
                                    select temp).Take(10).AsEnumerable().ToList<Patient>();
            }

            return attendedPatients;
        }

        public IActionResult GetUpdatedProvider(string username, string practiceName)
        {
            if (string.IsNullOrEmpty(username) || username == "undefined")
            { return BadRequest(); }

            var configuration = FewaDbContext.practices.Where(x => x.url.ToLower().Trim() == practiceName.ToLower().Trim()).FirstOrDefault();
            var provider = (from temp in FewaDbContext.providers
                            where temp.userName.ToLower().Trim() == username.ToLower().Trim() && temp.practice.ToLower().Trim() == practiceName.ToLower().Trim()
                            select temp).FirstOrDefault();
            provider.roomName = provider.roomName.Replace("name", provider.userName);
            var data = new
            {
                User = provider,
                Configuration = configuration
            };
            return Ok(data);
        }
        public IActionResult GetProviderCabin()
        {
            return Json(GetCurrentProviderCabin());
        }
        private ProviderCabin GetCurrentProviderCabin()
        {
            foreach (var item in _providerCabins)
            {
                if (item.provider.userName == HttpContext.Session.GetString("name"))
                {
                    return item;
                }

            }
            return null;
        }
        public IActionResult CallPatient([FromBody] Patient obj)
        {
            Patient p = GetPatientbyName(obj.name);
            if (p is null)
            {
                return StatusCode(500);
            }
            else
            {
                p.status = (int)TeleConstants.PatientCalled;
                //p.DoctorNameAttending = HttpContext.Session.GetString("Name");
                p.lastUpdated = DateTime.Now;
                GetCurrentProviderCabin().patient = p;
                //var dd = JsonSerializer.Serialize(_waitingroom.PatientsAttendedModels);
                //_notify.Clients.All.BroadcastMessage("PatientLoggedIn", dd);
                //var patient = JsonSerializer.Serialize(p);
                //_notify.Clients.All.BroadcastMessage("CallPatient", patient);
                return Ok(p);
            }
        }
        private Patient GetPatientbyName(string PatName)
        {
            foreach (var t in _waitingroom.patients)
            {
                if (PatName == t.name)
                {
                    return t;
                }
            }
            return null;
        }
        public IActionResult CurrentPatients()
        {

            this.RemoveIdle();
            //var dd = JsonSerializer.Serialize(_waitingroom.PatientsAttendedModels);
            //_notify.Clients.All.BroadcastMessage("PatientLoggedIn", dd);
            return Json(_waitingroom.patients);
        }
        public IActionResult WriteMedication([FromBody] Patient obj)
        {
            Patient p = GetPatientbyName(GetCurrentProviderCabin().patient.name);
            if (p.status == (int)TeleConstants.PatientCalled)
            {
                p.status = (int)TeleConstants.PatientCompleted;
                p.labOrdersSent = obj.labOrdersSent;
                p.newPrescriptionsSentToYourPharmacy = obj.newPrescriptionsSentToYourPharmacy;
                p.newPrescriptionsMailedToYou = obj.newPrescriptionsMailedToYou;
                p.medication = obj.medication;
                p.followUpNumber = obj.followUpNumber;
                p.followUpMeasure = obj.followUpMeasure;
                p.status = (int)TeleConstants.PatientCompleted;
                return Ok(true);
            }
            else
            {
                return Ok(true);
            }
        }
        public IActionResult TakeFinalReport([FromBody] Patient p1)
        {
            Patient p = GetPatientbyName(p1.name);
            if (p is null) { return Ok(null); }
            if (p.status == (int)TeleConstants.PatientCompleted)
            {
                var patient = JsonSerializer.Serialize(p);

                //_notify.Clients.All.BroadcastMessage("PatientCompleted", patient);
                //_waitingroom.PatientsAttendedModels.Remove(p);

                return Ok(p);
            }
            else
            {
                return Ok(null);
            }
        }
        public IActionResult PatientAttended([FromBody] Patient obj)
        {
            Patient p = GetPatientbyName(obj.name);
            if (p is null)
            {
                return StatusCode(500);
            }
            else
            {
                GetCurrentProviderCabin().patient = new Patient();
                p.status = (int)TeleConstants.PatientCompleted;
                p.labOrdersSent = obj.labOrdersSent;
                p.newPrescriptionsSentToYourPharmacy = obj.newPrescriptionsSentToYourPharmacy;
                p.newPrescriptionsMailedToYou = obj.newPrescriptionsMailedToYou;
                p.medication = obj.medication;
                p.followUpNumber = obj.followUpNumber;
                p.followUpMeasure = obj.followUpMeasure;
                p.endTime = DateTime.Now;
                p.medication = obj.medication;
                p.followUpNumber = obj.followUpNumber;
                p.followUpMeasure = obj.followUpMeasure;
                p.url = obj.url;
                p.advice = obj.advice;
                p.practice = obj.practice;
                FewaDbContext.patients.Add(p);
                FewaDbContext.SaveChanges();
                return Ok(p);
            }
        }
        private void RemoveIdle()
        {
            var removepats = new List<Patient>();

            foreach (var t in _waitingroom.patients)
            {
                var diffInSeconds = DateTime.Now.Subtract(t.lastUpdated)
                    .TotalSeconds;
                t.totalCheckupTime = diffInSeconds;
                if (diffInSeconds > idletime)
                {
                    removepats.Add(t);
                }
            }
            foreach (var t in removepats)
            {
                _waitingroom.patients.Remove(t);
            }
        }
        public IActionResult UpdateProfile([FromForm] Provider obj)
        {
            if (obj == null)
            {
                return BadRequest();
            }
            var files = Request.Form.Files;
            if (files.Count > 0)
            {
                string folderName = "img";
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, folderName);
                //logoPath = Path.Combine(folderName, file.FileName);
                obj.image = '/' + folderName + '/' + files[0].FileName;
                string filePath = Path.Combine(uploadsFolder, files[0].FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
            }
            var provider = _providerRepository.getProviderByUserName(obj.practice,obj.userName,obj.email);
            if (provider is null)
            {
                return StatusCode(500);
            }
            else
            {
                provider.nameTitle = obj.nameTitle;
                provider.name = obj.name;
                provider.email = obj.email;
                provider.mobileNumber = obj.mobileNumber;
                provider.designation = obj.designation;
                provider.medicalDegree = obj.medicalDegree;
                if (obj.image != null)
                    provider.image = obj.image;
                provider.password = Cipher.Encrypt(obj.newPassword, obj.userName);
                provider.newPassword = Cipher.Decrypt(provider.password, obj.userName);
            }
            FewaDbContext.providers.Update(provider);
            FewaDbContext.SaveChanges();
            return Ok(provider);
        }
        public IActionResult UpdatePracticeConfiguration([FromBody] Practice obj)
        {
            if (obj is null)
            {
                return StatusCode(500);
            }
            FewaDbContext.practices.Update(obj);
            FewaDbContext.SaveChanges();
            return Ok(obj);
        }
        public IActionResult UploadPracticeLogo([FromForm] Practice obj)
        {
            if (obj is null)
            {
                return StatusCode(500);
            }
            var files = Request.Form.Files;
            if (files.Count > 0)
            {
                string folderName = "img";
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, folderName);
                //logoPath = Path.Combine(folderName, file.FileName);
                obj.logoPath = '/' + folderName + '/' + files[0].FileName;
                string filePath = Path.Combine(uploadsFolder, files[0].FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
            }
            Practice practice = FewaDbContext.practices.Where(a => a.practiceId == obj.practiceId).FirstOrDefault();
            if (practice == null)
            {
                return Unauthorized(new { Message = "practice not found" });
            }
            else
            {
                practice.name = obj.name;
                practice.email = obj.email;
                practice.contactNumber = obj.contactNumber;
                practice.logoPath = obj.logoPath;
                practice.description = obj.description;
            }
            FewaDbContext.practices.Update(practice);
            FewaDbContext.SaveChanges();
            return Ok(practice);
        }

        public IActionResult PreviewEmailTemplate([FromBody] Practice list)
        {
            if (ModelState.IsValid)
            {
                if (list == null)
                {
                    return BadRequest();
                }
                // var oldEmail = "";
                var username = accessor.HttpContext.Session.GetString("name");
                var provider = _providerRepository.getProviderByUserName(list.name,username);
                var newEmailContent = list.emailAdditionalContent;
                var oldEmailContent = FewaDbContext.practices.Select(a => a.emailAdditionalContent).FirstOrDefault();
                var htmlContent = FewaDbContext.practices.Select(a => a.emailHtmlBody).FirstOrDefault();
                htmlContent = htmlContent.Replace("{imageUrl}", list.serverName + list.logoPath);
                htmlContent = htmlContent.Replace("{join}", list.serverName + "/" + provider.practice + "/" + provider.url + "/#/patient/intro");
                htmlContent = htmlContent.Replace("{serverName}", list.serverName);
                htmlContent = htmlContent.Replace("providerNameTitle", provider.nameTitle);
                if (!(string.IsNullOrEmpty(provider.name)))
                    htmlContent = htmlContent.Replace("providerName", provider.name);
                if (string.IsNullOrEmpty(provider.name))
                    htmlContent = htmlContent.Replace("providerName", provider.userName);
                htmlContent = htmlContent.Replace("practiceName", list.name);
                /* if (string.IsNullOrEmpty(oldEmailContent))
                 {
                     oldEmail = oldEmailContent;
                     oldEmailContent = "old";
                 } */
                if (!string.IsNullOrEmpty(oldEmailContent) && htmlContent.Contains(oldEmailContent) && !string.IsNullOrEmpty(newEmailContent))
                {
                    htmlContent = htmlContent.Replace(oldEmailContent, newEmailContent);
                }
                else if (htmlContent.Contains("EmailAdditionalContent") && !string.IsNullOrEmpty(newEmailContent))
                {
                    htmlContent = htmlContent.Replace("EmailAdditionalContent", newEmailContent);
                }

                var data = new
                {
                    EmailHTMLBody = htmlContent,
                    PreviewEmailContent = newEmailContent
                };

                return Ok(data);
            }
            else
            {
                return Ok("Cannot Load Preview.");
            }
        }

        public IActionResult GetAllAdvice([FromBody] Provider obj)
        {
            try
            {
                List<ProviderAdvice> getAllAdvice = FewaDbContext.advices.Where(a => a.practiceId == obj.practiceId 
                                                    && a.providerId == obj.providerId).ToList(); 
                if (getAllAdvice.Count == 0 )
                {
                    return NoContent();
                }
                return Ok(getAllAdvice);
            }
        
            catch (Exception ex)
            {
                return Ok("Error In Retrieving Records" + ex.Message);
            }
        }
        public IActionResult AddAdvice([FromBody] ProviderAdvice obj)
        {
            try
            {
                if (obj is null)
                {
                    return StatusCode(500);
                }
                Provider provider = FewaDbContext.providers.Where(a => a.providerId == obj.providerId && a.practiceId == obj.practiceId).FirstOrDefault();
                if (provider == null)
                {
                    return Ok(new { message = "Provider doesn't exists" });
                }
                ProviderAdvice newAdvice = new ProviderAdvice();
                obj.adviceId = FewaDbContext.advices.Max(a => a.adviceId) + 1;    
                newAdvice.adviceId = obj.adviceId;
                newAdvice.advice = obj.advice;
                newAdvice.practiceId = obj.practiceId;
                newAdvice.providerId = obj.providerId;
                FewaDbContext.advices.Add(newAdvice);
                FewaDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
            return Ok(FewaDbContext.advices.Where(a => a.providerId == obj.providerId).ToList());
        }

        public IActionResult EditAdvice([FromBody] ProviderAdvice obj)
        {
            try
            {
                if (obj is null)
                {
                    return StatusCode(500);
                }
                ProviderAdvice providerAdvice = FewaDbContext.advices.Where(a => a.adviceId == obj.adviceId && a.providerId == obj.providerId).FirstOrDefault();
                if (providerAdvice == null)
                {
                    return Ok(new { message = "Provider doesn't exists" });
                }
                providerAdvice.advice = obj.advice;
                FewaDbContext.advices.Update(providerAdvice);
                FewaDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
            return Ok(FewaDbContext.advices.Where(a => a.providerId == obj.providerId).ToList());
        }

        public IActionResult DeleteAdvice( int id)
        {

            ProviderAdvice removeAdvice = FewaDbContext.advices.Find(id);
            if (removeAdvice == null)
            {
                return NotFound();
            } 
            FewaDbContext.advices.Remove(removeAdvice);
            FewaDbContext.SaveChanges();
            return Ok(FewaDbContext.advices.Where(a => a.providerId == removeAdvice.providerId).ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult GetAllProvider(string practice)
        {
            try
            {
                List<Provider> getAllProvider = FewaDbContext.providers.Where(a => a.practice == practice).ToList();
                foreach (var item in getAllProvider)
                {
                    item.password = Cipher.Decrypt(item.password, item.userName);
                }
                if (getAllProvider.Count > 0)
                {
                    return Ok(getAllProvider);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return Ok("Error In Retrieving Records" + ex.Message);
            }
        }

        public IActionResult DeleteProvider(string practice, string username)
        {
            Provider removeProvider = FewaDbContext.providers.Where(a => a.userName == username && a.practice == practice).FirstOrDefault();
            if (removeProvider == null)
            {
                return NotFound();
            }
            FewaDbContext.providers.Remove(removeProvider);
            FewaDbContext.SaveChanges();
            List<ProviderAdvice> getAllAdvice = FewaDbContext.advices.Where(a => a.providerId == removeProvider.providerId).ToList();
            foreach (var item in getAllAdvice)
            {
                FewaDbContext.advices.Remove(item);
                FewaDbContext.SaveChanges();

            }
            _providers.Clear();
            foreach (var a in FewaDbContext.providers.ToList<Provider>()) //fetch new provider 
            {
                _providers.Add(a);

            }
            return Ok(_providers.Where(a => a.practiceId == removeProvider.practiceId).ToList());

        }

        [AllowAnonymous]
        public IActionResult GetAllPractices(string key)
        {
            try
            {
                if (key == "73l3M3D")
                {
                    return Ok(FewaDbContext.practices.Select(a => new Practice{url = a.url,email = a.email}).ToList());
                }
                else
                {
                    return Ok("wrongKey");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }

        }
        [AllowAnonymous]
        public IActionResult GetPracticeAndProvider(string practice, string provider)
        {
            try
            {
                List<Provider> getProvider = FewaDbContext.providers.Where(a => a.practice == practice.ToLower().Trim() && a.userName == provider.ToLower().Trim()).ToList();
                if (getProvider.Count > 0)
                {
                    var result = new
                    {
                        User = getProvider,
                    };
                    return Ok(result);

                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }

        }
    }
}