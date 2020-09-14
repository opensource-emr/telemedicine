﻿#region  FewaDbContext Description
/* This File contains Description about tables and Columns Values in Fewa Telemedicine.
 */
#endregion
#region Log History 
/* #38 14/9/2020 - Bhavana => Added Parameter "EmailAdditionalContent" in EmailAPI.
*  #38 14/9/2020 - Bhavana => Updated Email HTML Body Parameter.
 */
#endregion
using FewaTelemedicine.Domain.Models;
using FewaTelemedicine.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace FewaTelemedicine.Domain
{
    public class FewaContextFactory : IDesignTimeDbContextFactory<FewaDbContext>
    {
        // private readonly IHttpContextAccessor accessor;

        public FewaDbContext CreateDbContext(string[] args)
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Build config
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<FewaDbContext>();
            optionsBuilder.UseNpgsql(connectionString);
            return new FewaDbContext(optionsBuilder.Options);
        }

    }

    public class FewaDbContext : DbContext
    {
        //private readonly IHttpContextAccessor accessor;

        public FewaDbContext(DbContextOptions<FewaDbContext> options)
            : base(options)
        {
            //this.accessor = httpContextAccessor;
        }

        public DbSet<DoctorsModel> DoctorsModels { get; set; }
        public DbSet<PatientsAttendedModel> PatientsAttendedModels { get; set; }
        public DbSet<ParametersModel> ParametersModels { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DoctorsModel>().ToTable("mst_Doctors");
            modelBuilder.Entity<PatientsAttendedModel>().ToTable("txn_Patients_Attended");
            modelBuilder.Entity<PatientsAttendedModel>().Property(et => et.Id)
                    .ValueGeneratedOnAdd();
            modelBuilder.Entity<ParametersModel>().ToTable("mst_Parameters");
            // Default user name and password
            modelBuilder.Entity<DoctorsModel>().HasData(new DoctorsModel { Id = 1, UserName = "doctor", Password = "ajNJkHEqM5bu0szpIIhwzw==", DoctorRoomName = Guid.NewGuid().ToString() + "-" + "DoctorName", DoctorId = "DefaultDoctor", NameTitle = "Dr", DoctorName = "DefaultDoctor" },
                                                        new DoctorsModel { Id = 2, UserName = "doctor1", Password = "n5J6FQcRx5weMX9aHi5VMA==", DoctorRoomName = Guid.NewGuid().ToString() + "-" + "DoctorName", DoctorId = "doc1", NameTitle = "Dr", DoctorName = "Doctor1" },
                                                        new DoctorsModel { Id = 3, UserName = "doctor2", Password = "l1F7gwOSJWmin7N+0ECzrw==", DoctorRoomName = Guid.NewGuid().ToString() + "-" + "DoctorName", DoctorId = "doc2", NameTitle = "Dr", DoctorName = "Doctor2" }
            );

            modelBuilder.Entity<DoctorsModel>()
                 .HasKey(c => new { c.Id, c.DoctorId });
            modelBuilder.Entity<ParametersModel>()
                 .HasKey(c => new { c.Id, c.DoctorId });
            modelBuilder.Entity<PatientsAttendedModel>()
                 .HasKey(c => new { c.Id, c.DoctorId });

            // Filling the parameter table with default names
            modelBuilder.Entity<ParametersModel>().HasData(
                       new
                       {
                           Id = 1,
                           ParameterGroupName = "Hospital",
                           ParameterName = "Name",
                           ParameterValue = "Fewa Telemedicine",
                           ValueDataType = "string",
                           DoctorId = "DefaultDoctor"
                       },
                       new
                       {
                           Id = 2,
                           ParameterGroupName = "Hospital",
                           ParameterName = "Description",
                           ParameterValue = "Welcome to the demo of Fewa. This is the place where you can put your hospital description. Fewa is a application which helps to connect doctors and patient using video. Patient can print advice , share documents with doctor and doctor also knows how much time he has given to attend the patient. To start using the demo login as username-doctor,password doctor , send a invitation to the patient and then both can communicate. ",
                           ValueDataType = "string",
                           DoctorId = "DefaultDoctor"
                       },
                       new
                       {
                           Id = 3,
                           ParameterGroupName = "Hospital",
                           ParameterName = "ContactNumber",
                           ParameterValue = "98465175",
                           ValueDataType = "string",
                           DoctorId = "DefaultDoctor"
                       },
                       new
                       {
                           Id = 4,
                           ParameterGroupName = "Hospital",
                           ParameterName = "Email",
                           ParameterValue = "dummy@email.com",
                           ValueDataType = "string",
                           DoctorId = "DefaultDoctor"
                       },
                       new
                       {
                           Id = 5,
                           ParameterGroupName = "Hospital",
                           ParameterName = "VideoCallPlatform",
                           ParameterValue = "jitsi",
                           ValueDataType = "string",
                           DoctorId = "DefaultDoctor"
                       },
                       new
                       {
                           Id = 6,
                           ParameterGroupName = "Hospital",
                           ParameterName = "LogoPath",
                           ParameterValue = "img/logo.png",
                           ValueDataType = "string",
                           DoctorId = "DefaultDoctor"
                       },
                       new
                       {
                           Id = 7,
                           ParameterGroupName = "EmailAPI",
                           ParameterName = "ApiKey",
                           ParameterValue = "****",
                           ValueDataType = "string",
                           DoctorId = "DefaultDoctor"
                       },
                       new
                       {
                           Id = 8,
                           ParameterGroupName = "EmailAPI",
                           ParameterName = "Email",
                           ParameterValue = "dummy@email.com",
                           ValueDataType = "string",
                           DoctorId = "DefaultDoctor"
                       },
                       new
                       {
                           Id = 9,
                           ParameterGroupName = "EmailAPI",
                           ParameterName = "Name",
                           ParameterValue = "Fewa",
                           ValueDataType = "string",
                           DoctorId = "DefaultDoctor"
                       },
                       new
                       {
                           Id = 10,
                           ParameterGroupName = "EmailAPI",
                           ParameterName = "EmailPlainBody",
                           ParameterValue = "Please attend the doctor.",
                           ValueDataType = "string",
                           DoctorId = "DefaultDoctor"
                       },
                       new
                       {
                           Id = 11,
                           ParameterGroupName = "EmailAPI",
                           ParameterName = "EmailSubject",
                           ParameterValue = "fewa Telemedicine call today schedule.",
                           ValueDataType = "string",
                           DoctorId = "DefaultDoctor"
                       },
                        new
                        {
                            Id = 12,
                            ParameterGroupName = "EmailAPI",
                            ParameterName = "EmailAdditionalContent",
                            ParameterValue = "",
                            ValueDataType = "string",
                            DoctorId = "DefaultDoctor"
                        },
                       new
                       {
                           Id = 13,
                           ParameterGroupName = "EmailAPI",
                           ParameterName = "EmailHTMLBody",
                           ParameterValue = "   <html>  " +
                                                 "      <head>  " +
                                                 "         <style>  " +
                                                 "            .banner-color {  " +
                                                 "            background-color: #eb681f!important;  " +
                                                 "            }  " +
                                                 "            .title-color {  " +
                                                 "            color: #0066cc!important;  " +
                                                 "            }  " +
                                                 "            .button-color {  " +
                                                 "            background-color: #0066cc!important;  " +
                                                 "            }  " +
                                                 "            @media screen and (min-width: 500px) {  " +
                                                 "            .banner-color {  " +
                                                 "            background-color: #eb681f!important;  " +
                                                 "            }  " +
                                                 "            .title-color {  " +
                                                 "            color: black;  " +
                                                 "            }  " +
                                                 "            .button-color {  " +
                                                 "            background-color: #0066cc!important;  " +
                                                 "            }  " +
                                                 "            }  " +
                                                 "         </style>  " +
                                                 "      </head>  " +
                                                 "      <body>  " +
                                                 "         <div style='background-color:#ececec;padding:0;margin:0 auto;font-weight:200;width:100%!important;height:100%!important'>  " +
                                                 "            <table align='center' border='0' cellspacing='0' cellpadding='0' style='table-layout:fixed;font-weight:200;font-family:Helvetica,Arial,sans-serif' width='100%'>  " +
                                                 "               <tbody>  " +
                                                 "                  <tr>  " +
                                                 "                     <td align='center'>  " +
                                                 "                        <center style='width:100%'>  " +
                                                 "                           <table bgcolor='#FFFFFF' border='0' cellspacing='0' cellpadding='0' style='margin:0 auto;max-width:512px;font-weight:200;width:inherit;font-family:Helvetica,Arial,sans-serif' width='512'>  " +
                                                 "                              <tbody style='position:absolute'>  " +
                                                 "                                 <tr>  " +
                                                 "                                    <td bgcolor='#F3F3F3' width='100%' style='background-color:#f3f3f3;padding:12px;border-bottom:1px solid #ececec'>  " +
                                                 "                                       <table border='0' cellspacing='0' cellpadding='0' style='font-weight:200;width:100%!important;font-family:Helvetica,Arial,sans-serif;min-width:100%!important' width='100%'>  " +
                                                 "                                          <tbody>  " +
                                                 "                                             <tr>  " +
                                                 "                                                <td align='left' valign='middle' width='50%'><span style='margin:0;color:#4c4c4c;white-space:normal;display:inline-block;text-decoration:none;font-size:12px;line-height:20px'></span></td>  " +
                                                 "                                                <td valign='middle' width='50%' align='right' style='padding:0 0 0 10px'><span style='margin:0;color:#4c4c4c;white-space:normal;display:inline-block;text-decoration:none;font-size:12px;line-height:20px'>TodaysDate</span></td>  " +
                                                 "                                                <td width='1'>&nbsp;</td>  " +
                                                 "                                             </tr>  " +
                                                 "                                          </tbody>  " +
                                                 "                                       </table>  " +
                                                 "                                    </td>  " +
                                                 "                                 </tr>  " +
                                                 "                                 <tr>  " +
                                                 "                                    <td align='left' style='background-color:white'>  " +
                                                 "                                       <table border='0' cellspacing='0' cellpadding='0' style='font-weight:200;font-family:Helvetica,Arial,sans-serif' width='100%'>  " +
                                                 "                                          <tbody>  " +
                                                 "                                             <tr>  " +
                                                 "                                                <td width='100%'>  " +
                                                 "                                                   <table border='0' cellspacing='0' cellpadding='0' style='font-weight:200;font-family:Helvetica,Arial,sans-serif' width='100%'>  " +
                                                 "                                                      <tbody>  " +
                                                 "                                                         <tr>  " +
                                                 "                                                            <td align='center'  style='padding:20px 48px;color:#ffffff' class='banner-color'>  " +
                                                 "                                                               <table border='0' cellspacing='0' cellpadding='0' style='font-weight:200;font-family:Helvetica,Arial,sans-serif' width='100%'>  " +
                                                 "                                                                  <tbody>  " +
                                                 "                                                                     <tr>  " +
                                                 "                                                                        <td align='center' width='100%'>  " +
                                                 "   																		<h1 style='padding:0;margin:0;color:#ffffff;font-weight:500;font-size:20px;line-height:24px'>Invitation from HospitalName</h1>  " +
                                                 "                                                                        </td>  " +
                                                 "                                                                     </tr>  " +
                                                 "                                                                  </tbody>  " +
                                                 "                                                               </table>  " +
                                                 "                                                            </td>  " +
                                                 "                                                         </tr>  " +
                                                 "                                                         <tr>  " +
                                                 "                                                            <td align='center' style='padding:20px 0 10px 0'>  " +
                                                 "                                                               <table border='0' cellspacing='0' cellpadding='0' style='font-weight:200;font-family:Helvetica,Arial,sans-serif' width='100%'>  " +
                                                 "                                                                  <tbody>  " +
                                                 "                                                                     <tr>  " +
                                                 "   																		<td align='center' width='100%' style='padding: 0 15px;text-align: justify;color: rgb(76, 76, 76);font-size: 12px;line-height: 18px;'>  " +
                                                 "   																		<tr _ngcontent-kfm-c50<td _ngcontent-kfm-c50=''><img _ngcontent-kfm-c50='' src='{ImageUrl}' style='height: 80px;'></td></tr>  " +
                                                 "   																		<h3 style='font-weight: 600; padding: 0px; margin: 0px; font-size: 16px; line-height: 24px; text-align: center;' class='title-color'>Hi, This is <strong>DoctorNameTitle &nbsp;&nbsp;DoctorName</strong></h3>  " +
                                                 "                                                                           <p style='margin: 20px 0 30px 0;font-size: 15px;text-align: center;'>EmailAdditionalContent &nbsp;</p>" +
                                                 "                                                                           <p style='margin: 20px 0 30px 0;font-size: 15px;text-align: center;'>Please click this following button to join me for a secure video meeting <b>Join now</b>!</p>  " +
                                                 "                                                                           <div style='font-weight: 200; text-align: center; margin: 25px;'><a style='padding:0.6em 1em;border-radius:600px;color:#ffffff;font-size:14px;text-decoration:none;font-weight:bold' class='button-color' href='{Join}'>Join the conference</a></div>  " +
                                                 "                                                                        </td>  " +
                                                 "                                                                     </tr>  " +
                                                 "                                                                  </tbody>  " +
                                                 "                                                               </table>  " +
                                                 "   															<table border='0' cellspacing='0' cellpadding='0' style='font-weight:200;font-family:Helvetica,Arial,sans-serif' width='100%'>  " +
                                                 "   																<tbody>  " +
                                                 "   																	<tr>  " +
                                                 "   																		<td align='center' valign='middle' width='100%' style='border-top:1px solid #d9d9d9;padding:12px 0px 20px 0px;text-align:center;color:#4c4c4c;font-weight:200;font-size:12px;line-height:18px'>Regards,  " +
                                                 "   																			<br><b>Powered by © Fewa Telehealth 2020</b>  " +
                                                 "   																		</td>  " +
                                                 "   																	</tr>  " +
                                                 "   																</tbody>  " +
                                                 "   															</table>  " +
                                                 "                                                            </td>  " +
                                                 "                                                         </tr>  " +
                                                 "                                                      </tbody>  " +
                                                 "                                                   </table>  " +
                                                 "                                                </td>  " +
                                                 "                                             </tr>  " +
                                                 "                                          </tbody>  " +
                                                 "                                       </table>  " +
                                                 "                                    </td>  " +
                                                 "                                 </tr>  " +
                                                 "                                 <tr>  " +
                                                 "                                  " +
                                                 "                                 </tr>  " +
                                                 "   							   <tr>  " +
                                                 "                                    <td bgcolor='#F3F3F3' width='100%' style='background-color:#f3f3f3;padding:12px;border-bottom:1px solid #ececec'>  " +
                                                 "                                       <table border='0' cellspacing='0' cellpadding='0' style='font-weight:200;width:100%!important;font-family:Helvetica,Arial,sans-serif;min-width:100%!important' width='100%'>  " +
                                                 "                                          <tbody>  " +
                                                 "                                             <tr>  " +
                                                 "   												<td align='left' valign='middle' width='50%'><span style='margin:0;color:#4c4c4c;white-space:normal;display:inline-block;text-decoration:none;font-size:12px;line-height:20px'></span></td>  " +
                                                 "                                                <td valign='middle' width='50%' align='right' style='padding:0 0 0 10px'><span style='margin:0;color:#4c4c4c;white-space:normal;display:inline-block;text-decoration:none;font-size:12px;line-height:20px'></span></td>  " +
                                                 "                                                <td width='1'>&nbsp;</td>  " +
                                                 "                                             </tr>  " +
                                                 "                                          </tbody>  " +
                                                 "                                       </table>  " +
                                                 "                                    </td>  " +
                                                 "                                 </tr>  " +
                                                 "                              </tbody>  " +
                                                 "                           </table>  " +
                                                 "                        </center>  " +
                                                 "                     </td>  " +
                                                 "                  </tr>  " +
                                                 "               </tbody>  " +
                                                 "            </table>  " +
                                                 "         </div>  " +
                                                 "      </body>  " +
                                                 "  </html>  ",
                           ValueDataType = "string",
                           DoctorId = "DefaultDoctor"
                       },
                       new
                       {
                           Id = 14,
                           ParameterGroupName = "SMSAPI",
                           ParameterName = "AccountSID",
                           ParameterValue = "****",
                           ValueDataType = "string",
                           DoctorId = "DefaultDoctor"
                       },
                       new
                       {
                           Id = 15,
                           ParameterGroupName = "SMSAPI",
                           ParameterName = "AuthToken",
                           ParameterValue = "****",
                           ValueDataType = "string",
                           DoctorId = "DefaultDoctor"
                       },
                       new
                       {
                           Id = 16,
                           ParameterGroupName = "SMSAPI",
                           ParameterName = "PhoneNumber",
                           ParameterValue = "****",
                           ValueDataType = "string",
                           DoctorId = "DefaultDoctor"
                       },
                       new
                       {
                           Id = 17,
                           ParameterGroupName = "Doctor",
                           ParameterName = "DoctorRoomName",
                           ParameterValue = "doctor",
                           ValueDataType = "string",
                           DoctorId = "DefaultDoctor"
                       },
                       new
                       {
                           Id = 18,
                           ParameterGroupName = "Server",
                           ParameterName = "ServerName",
                           ParameterValue = "https://localhost:44304/",
                           ValueDataType = "string",
                           DoctorId = "DefaultDoctor"
                       },
                      new
                      {
                          Id = 19,
                          ParameterGroupName = "Hospital",
                          ParameterName = "Name",
                          ParameterValue = "Fewa Telemedicine",
                          ValueDataType = "string",
                          DoctorId = "doc1"
                      },
                       new
                       {
                           Id = 20,
                           ParameterGroupName = "Hospital",
                           ParameterName = "Description",
                           ParameterValue = "Welcome to the demo of Fewa. This is the place where you can put your hospital description. Fewa is a application which helps to connect doctors and patient using video. Patient can print advice , share documents with doctor and doctor also knows how much time he has given to attend the patient. To start using the demo login as username-doctor1,password doctor1 , send a invitation to the patient and then both can communicate. ",
                           ValueDataType = "string",
                           DoctorId = "doc1"
                       },
                       new
                       {
                           Id = 21,
                           ParameterGroupName = "Hospital",
                           ParameterName = "ContactNumber",
                           ParameterValue = "98465175",
                           ValueDataType = "string",
                           DoctorId = "doc1"
                       },
                       new
                       {
                           Id = 22,
                           ParameterGroupName = "Hospital",
                           ParameterName = "Email",
                           ParameterValue = "dummy@email.com",
                           ValueDataType = "string",
                           DoctorId = "doc1"
                       },
                       new
                       {
                           Id = 23,
                           ParameterGroupName = "Hospital",
                           ParameterName = "VideoCallPlatform",
                           ParameterValue = "jitsi",
                           ValueDataType = "string",
                           DoctorId = "doc1"
                       },
                       new
                       {
                           Id = 24,
                           ParameterGroupName = "Hospital",
                           ParameterName = "LogoPath",
                           ParameterValue = "img/logo.png",
                           ValueDataType = "string",
                           DoctorId = "doc1"
                       },
                       new
                       {
                           Id = 25,
                           ParameterGroupName = "EmailAPI",
                           ParameterName = "ApiKey",
                           ParameterValue = "****",
                           ValueDataType = "string",
                           DoctorId = "doc1"
                       },
                       new
                       {
                           Id = 26,
                           ParameterGroupName = "EmailAPI",
                           ParameterName = "Email",
                           ParameterValue = "dummy@email.com",
                           ValueDataType = "string",
                           DoctorId = "doc1"
                       },
                       new
                       {
                           Id = 27,
                           ParameterGroupName = "EmailAPI",
                           ParameterName = "Name",
                           ParameterValue = "Fewa",
                           ValueDataType = "string",
                           DoctorId = "doc1"
                       },
                       new
                       {
                           Id = 28,
                           ParameterGroupName = "EmailAPI",
                           ParameterName = "EmailPlainBody",
                           ParameterValue = "Please attend the doctor.",
                           ValueDataType = "string",
                           DoctorId = "doc1"
                       },
                       new
                       {
                           Id = 29,
                           ParameterGroupName = "EmailAPI",
                           ParameterName = "EmailSubject",
                           ParameterValue = "fewa Telemedicine call today schedule.",
                           ValueDataType = "string",
                           DoctorId = "doc1"
                       },
                        new
                        {
                            Id = 30,
                            ParameterGroupName = "EmailAPI",
                            ParameterName = "EmailAdditionalContent",
                            ParameterValue = "",
                            ValueDataType = "string",
                            DoctorId = "doc1"
                        },
                       new
                       {
                           Id = 31,
                           ParameterGroupName = "EmailAPI",
                           ParameterName = "EmailHTMLBody",
                           ParameterValue = "   <html>  " +
                                                 "      <head>  " +
                                                 "         <style>  " +
                                                 "            .banner-color {  " +
                                                 "            background-color: #eb681f!important;  " +
                                                 "            }  " +
                                                 "            .title-color {  " +
                                                 "            color: #0066cc!important;  " +
                                                 "            }  " +
                                                 "            .button-color {  " +
                                                 "            background-color: #0066cc!important;  " +
                                                 "            }  " +
                                                 "            @media screen and (min-width: 500px) {  " +
                                                 "            .banner-color {  " +
                                                 "            background-color: #eb681f!important;  " +
                                                 "            }  " +
                                                 "            .title-color {  " +
                                                 "            color: black;  " +
                                                 "            }  " +
                                                 "            .button-color {  " +
                                                 "            background-color: #0066cc!important;  " +
                                                 "            }  " +
                                                 "            }  " +
                                                 "         </style>  " +
                                                 "      </head>  " +
                                                 "      <body>  " +
                                                 "         <div style='background-color:#ececec;padding:0;margin:0 auto;font-weight:200;width:100%!important;height:100%!important'>  " +
                                                 "            <table align='center' border='0' cellspacing='0' cellpadding='0' style='table-layout:fixed;font-weight:200;font-family:Helvetica,Arial,sans-serif' width='100%'>  " +
                                                 "               <tbody>  " +
                                                 "                  <tr>  " +
                                                 "                     <td align='center'>  " +
                                                 "                        <center style='width:100%'>  " +
                                                 "                           <table bgcolor='#FFFFFF' border='0' cellspacing='0' cellpadding='0' style='margin:0 auto;max-width:512px;font-weight:200;width:inherit;font-family:Helvetica,Arial,sans-serif' width='512'>  " +
                                                 "                              <tbody style='top:21%;position:absolute'>  " +
                                                 "                                 <tr>  " +
                                                 "                                    <td bgcolor='#F3F3F3' width='100%' style='background-color:#f3f3f3;padding:12px;border-bottom:1px solid #ececec'>  " +
                                                 "                                       <table border='0' cellspacing='0' cellpadding='0' style='font-weight:200;width:100%!important;font-family:Helvetica,Arial,sans-serif;min-width:100%!important' width='100%'>  " +
                                                 "                                          <tbody>  " +
                                                 "                                             <tr>  " +
                                                 "                                                <td align='left' valign='middle' width='50%'><span style='margin:0;color:#4c4c4c;white-space:normal;display:inline-block;text-decoration:none;font-size:12px;line-height:20px'></span></td>  " +
                                                 "                                                <td valign='middle' width='50%' align='right' style='padding:0 0 0 10px'><span style='margin:0;color:#4c4c4c;white-space:normal;display:inline-block;text-decoration:none;font-size:12px;line-height:20px'>TodaysDate</span></td>  " +
                                                 "                                                <td width='1'>&nbsp;</td>  " +
                                                 "                                             </tr>  " +
                                                 "                                          </tbody>  " +
                                                 "                                       </table>  " +
                                                 "                                    </td>  " +
                                                 "                                 </tr>  " +
                                                 "                                 <tr>  " +
                                                 "                                    <td align='left' style='background-color:white'>  " +
                                                 "                                       <table border='0' cellspacing='0' cellpadding='0' style='font-weight:200;font-family:Helvetica,Arial,sans-serif' width='100%'>  " +
                                                 "                                          <tbody>  " +
                                                 "                                             <tr>  " +
                                                 "                                                <td width='100%'>  " +
                                                 "                                                   <table border='0' cellspacing='0' cellpadding='0' style='font-weight:200;font-family:Helvetica,Arial,sans-serif' width='100%'>  " +
                                                 "                                                      <tbody>  " +
                                                 "                                                         <tr>  " +
                                                 "                                                            <td align='center'  style='padding:20px 48px;color:#ffffff' class='banner-color'>  " +
                                                 "                                                               <table border='0' cellspacing='0' cellpadding='0' style='font-weight:200;font-family:Helvetica,Arial,sans-serif' width='100%'>  " +
                                                 "                                                                  <tbody>  " +
                                                 "                                                                     <tr>  " +
                                                 "                                                                        <td align='center' width='100%'>  " +
                                                 "   																		<h1 style='padding:0;margin:0;color:#ffffff;font-weight:500;font-size:20px;line-height:24px'>Invitation from HospitalName</h1>  " +
                                                 "                                                                        </td>  " +
                                                 "                                                                     </tr>  " +
                                                 "                                                                  </tbody>  " +
                                                 "                                                               </table>  " +
                                                 "                                                            </td>  " +
                                                 "                                                         </tr>  " +
                                                 "                                                         <tr>  " +
                                                 "                                                            <td align='center' style='padding:20px 0 10px 0'>  " +
                                                 "                                                               <table border='0' cellspacing='0' cellpadding='0' style='font-weight:200;font-family:Helvetica,Arial,sans-serif' width='100%'>  " +
                                                 "                                                                  <tbody>  " +
                                                 "                                                                     <tr>  " +
                                                 "   																		<td align='center' width='100%' style='padding: 0 15px;text-align: justify;color: rgb(76, 76, 76);font-size: 12px;line-height: 18px;'>  " +
                                                 "   																		<tr _ngcontent-kfm-c50<td _ngcontent-kfm-c50=''><img _ngcontent-kfm-c50='' src='{ImageUrl}' style='height: 80px;'></td></tr>  " +
                                                 "   																		<h3 style='font-weight: 600; padding: 0px; margin: 0px; font-size: 16px; line-height: 24px; text-align: center;' class='title-color'>Hi, This is <strong>DoctorNameTitle &nbsp;&nbsp;DoctorName</strong></h3>" +
                                                 "                                                                           <p style='margin: 20px 0 30px 0;font-size: 15px;text-align: center;'>EmailAdditionalContent &nbsp;</p>" +
                                                 "                                                                           <p style='margin: 20px 0 30px 0;font-size: 15px;text-align: center;'>Please click this following button to join me for a secure video meeting <b>Join now</b>!</p>  " +
                                                 "                                                                           <div style='font-weight: 200; text-align: center; margin: 25px;'><a style='padding:0.6em 1em;border-radius:600px;color:#ffffff;font-size:14px;text-decoration:none;font-weight:bold' class='button-color' href='{Join}'>Join the conference</a></div>  " +
                                                 "                                                                        </td>  " +
                                                 "                                                                     </tr>  " +
                                                 "                                                                  </tbody>  " +
                                                 "                                                               </table>  " +
                                                 "   															<table border='0' cellspacing='0' cellpadding='0' style='font-weight:200;font-family:Helvetica,Arial,sans-serif' width='100%'>  " +
                                                 "   																<tbody>  " +
                                                 "   																	<tr>  " +
                                                 "   																		<td align='center' valign='middle' width='100%' style='border-top:1px solid #d9d9d9;padding:12px 0px 20px 0px;text-align:center;color:#4c4c4c;font-weight:200;font-size:12px;line-height:18px'>Regards,  " +
                                                 "   																			<br><b>Powered by © Fewa Telehealth 2020</b>  " +
                                                 "   																		</td>  " +
                                                 "   																	</tr>  " +
                                                 "   																</tbody>  " +
                                                 "   															</table>  " +
                                                 "                                                            </td>  " +
                                                 "                                                         </tr>  " +
                                                 "                                                      </tbody>  " +
                                                 "                                                   </table>  " +
                                                 "                                                </td>  " +
                                                 "                                             </tr>  " +
                                                 "                                          </tbody>  " +
                                                 "                                       </table>  " +
                                                 "                                    </td>  " +
                                                 "                                 </tr>  " +
                                                 "                                 <tr>  " +
                                                 "                                  " +
                                                 "                                 </tr>  " +
                                                 "   							   <tr>  " +
                                                 "                                    <td bgcolor='#F3F3F3' width='100%' style='background-color:#f3f3f3;padding:12px;border-bottom:1px solid #ececec'>  " +
                                                 "                                       <table border='0' cellspacing='0' cellpadding='0' style='font-weight:200;width:100%!important;font-family:Helvetica,Arial,sans-serif;min-width:100%!important' width='100%'>  " +
                                                 "                                          <tbody>  " +
                                                 "                                             <tr>  " +
                                                 "   												<td align='left' valign='middle' width='50%'><span style='margin:0;color:#4c4c4c;white-space:normal;display:inline-block;text-decoration:none;font-size:12px;line-height:20px'></span></td>  " +
                                                 "                                                <td valign='middle' width='50%' align='right' style='padding:0 0 0 10px'><span style='margin:0;color:#4c4c4c;white-space:normal;display:inline-block;text-decoration:none;font-size:12px;line-height:20px'></span></td>  " +
                                                 "                                                <td width='1'>&nbsp;</td>  " +
                                                 "                                             </tr>  " +
                                                 "                                          </tbody>  " +
                                                 "                                       </table>  " +
                                                 "                                    </td>  " +
                                                 "                                 </tr>  " +
                                                 "                              </tbody>  " +
                                                 "                           </table>  " +
                                                 "                        </center>  " +
                                                 "                     </td>  " +
                                                 "                  </tr>  " +
                                                 "               </tbody>  " +
                                                 "            </table>  " +
                                                 "         </div>  " +
                                                 "      </body>  " +
                                                 "  </html>  ",
                           ValueDataType = "string",
                           DoctorId = "doc1"
                       },
                       new
                       {
                           Id = 32,
                           ParameterGroupName = "SMSAPI",
                           ParameterName = "AccountSID",
                           ParameterValue = "****",
                           ValueDataType = "string",
                           DoctorId = "doc1"
                       },
                       new
                       {
                           Id = 33,
                           ParameterGroupName = "SMSAPI",
                           ParameterName = "AuthToken",
                           ParameterValue = "****",
                           ValueDataType = "string",
                           DoctorId = "doc1"
                       },
                       new
                       {
                           Id = 34,
                           ParameterGroupName = "SMSAPI",
                           ParameterName = "PhoneNumber",
                           ParameterValue = "****",
                           ValueDataType = "string",
                           DoctorId = "doc1"
                       },
                       new
                       {
                           Id = 35,
                           ParameterGroupName = "Doctor",
                           ParameterName = "DoctorRoomName",
                           ParameterValue = "doctor",
                           ValueDataType = "string",
                           DoctorId = "doc1"
                       },
                       new
                       {
                           Id = 36,
                           ParameterGroupName = "Server",
                           ParameterName = "ServerName",
                           ParameterValue = "https://localhost:44304/",
                           ValueDataType = "string",
                           DoctorId = "doc1"
                       },
                       new
                       {
                           Id = 37,
                           ParameterGroupName = "Hospital",
                           ParameterName = "Name",
                           ParameterValue = "Fewa Telemedicine",
                           ValueDataType = "string",
                           DoctorId = "doc2"
                       },
                       new
                       {
                           Id = 38,
                           ParameterGroupName = "Hospital",
                           ParameterName = "Description",
                           ParameterValue = "Welcome to the demo of Fewa. This is the place where you can put your hospital description. Fewa is a application which helps to connect doctors and patient using video. Patient can print advice , share documents with doctor and doctor also knows how much time he has given to attend the patient. To start using the demo login as username-doctor2,password doctor2 , send a invitation to the patient and then both can communicate. ",
                           ValueDataType = "string",
                           DoctorId = "doc2"
                       },
                       new
                       {
                           Id = 39,
                           ParameterGroupName = "Hospital",
                           ParameterName = "ContactNumber",
                           ParameterValue = "98465175",
                           ValueDataType = "string",
                           DoctorId = "doc2"
                       },
                       new
                       {
                           Id = 40,
                           ParameterGroupName = "Hospital",
                           ParameterName = "Email",
                           ParameterValue = "dummy@email.com",
                           ValueDataType = "string",
                           DoctorId = "doc2"
                       },
                       new
                       {
                           Id = 41,
                           ParameterGroupName = "Hospital",
                           ParameterName = "VideoCallPlatform",
                           ParameterValue = "jitsi",
                           ValueDataType = "string",
                           DoctorId = "doc2"
                       },
                       new
                       {
                           Id = 42,
                           ParameterGroupName = "Hospital",
                           ParameterName = "LogoPath",
                           ParameterValue = "img/logo.png",
                           ValueDataType = "string",
                           DoctorId = "doc2"
                       },
                       new
                       {
                           Id = 43,
                           ParameterGroupName = "EmailAPI",
                           ParameterName = "ApiKey",
                           ParameterValue = "****",
                           ValueDataType = "string",
                           DoctorId = "doc2"
                       },
                       new
                       {
                           Id = 44,
                           ParameterGroupName = "EmailAPI",
                           ParameterName = "Email",
                           ParameterValue = "dummy@email.com",
                           ValueDataType = "string",
                           DoctorId = "doc2"
                       },
                       new
                       {
                           Id = 45,
                           ParameterGroupName = "EmailAPI",
                           ParameterName = "Name",
                           ParameterValue = "Fewa",
                           ValueDataType = "string",
                           DoctorId = "doc2"
                       },
                       new
                       {
                           Id = 46,
                           ParameterGroupName = "EmailAPI",
                           ParameterName = "EmailPlainBody",
                           ParameterValue = "Please attend the doctor.",
                           ValueDataType = "string",
                           DoctorId = "doc2"
                       },
                       new
                       {
                           Id = 47,
                           ParameterGroupName = "EmailAPI",
                           ParameterName = "EmailSubject",
                           ParameterValue = "fewa Telemedicine call today schedule.",
                           ValueDataType = "string",
                           DoctorId = "doc2"
                       },
                        new
                         {
                             Id = 48,
                             ParameterGroupName = "EmailAPI",
                             ParameterName = "EmailAdditionalContent",
                             ParameterValue = "",
                             ValueDataType = "string",
                             DoctorId = "doc2"
                         },
                       new
                       {
                           Id = 49,
                           ParameterGroupName = "EmailAPI",
                           ParameterName = "EmailHTMLBody",
                           ParameterValue = "   <html>  " +
                                                 "      <head>  " +
                                                 "         <style>  " +
                                                 "            .banner-color {  " +
                                                 "            background-color: #eb681f!important;  " +
                                                 "            }  " +
                                                 "            .title-color {  " +
                                                 "            color: #0066cc!important;  " +
                                                 "            }  " +
                                                 "            .button-color {  " +
                                                 "            background-color: #0066cc!important;  " +
                                                 "            }  " +
                                                 "            @media screen and (min-width: 500px) {  " +
                                                 "            .banner-color {  " +
                                                 "            background-color: #eb681f!important;  " +
                                                 "            }  " +
                                                 "            .title-color {  " +
                                                 "            color: black;  " +
                                                 "            }  " +
                                                 "            .button-color {  " +
                                                 "            background-color: #0066cc!important;  " +
                                                 "            }  " +
                                                 "            }  " +
                                                 "         </style>  " +
                                                 "      </head>  " +
                                                 "      <body>  " +
                                                 "         <div style='background-color:#ececec;padding:0;margin:0 auto;font-weight:200;width:100%!important;height:100%!important'>  " +
                                                 "            <table align='center' border='0' cellspacing='0' cellpadding='0' style='table-layout:fixed;font-weight:200;font-family:Helvetica,Arial,sans-serif' width='100%'>  " +
                                                 "               <tbody>  " +
                                                 "                  <tr>  " +
                                                 "                     <td align='center'>  " +
                                                 "                        <center style='width:100%'>  " +
                                                 "                           <table bgcolor='#FFFFFF' border='0' cellspacing='0' cellpadding='0' style='margin:0 auto;max-width:512px;font-weight:200;width:inherit;font-family:Helvetica,Arial,sans-serif' width='512'>  " +
                                                 "                              <tbody style='top:21%;position:absolute'>  " +
                                                 "                                 <tr>  " +
                                                 "                                    <td bgcolor='#F3F3F3' width='100%' style='background-color:#f3f3f3;padding:12px;border-bottom:1px solid #ececec'>  " +
                                                 "                                       <table border='0' cellspacing='0' cellpadding='0' style='font-weight:200;width:100%!important;font-family:Helvetica,Arial,sans-serif;min-width:100%!important' width='100%'>  " +
                                                 "                                          <tbody>  " +
                                                 "                                             <tr>  " +
                                                 "                                                <td align='left' valign='middle' width='50%'><span style='margin:0;color:#4c4c4c;white-space:normal;display:inline-block;text-decoration:none;font-size:12px;line-height:20px'></span></td>  " +
                                                 "                                                <td valign='middle' width='50%' align='right' style='padding:0 0 0 10px'><span style='margin:0;color:#4c4c4c;white-space:normal;display:inline-block;text-decoration:none;font-size:12px;line-height:20px'>TodaysDate</span></td>  " +
                                                 "                                                <td width='1'>&nbsp;</td>  " +
                                                 "                                             </tr>  " +
                                                 "                                          </tbody>  " +
                                                 "                                       </table>  " +
                                                 "                                    </td>  " +
                                                 "                                 </tr>  " +
                                                 "                                 <tr>  " +
                                                 "                                    <td align='left' style='background-color:white'>  " +
                                                 "                                       <table border='0' cellspacing='0' cellpadding='0' style='font-weight:200;font-family:Helvetica,Arial,sans-serif' width='100%'>  " +
                                                 "                                          <tbody>  " +
                                                 "                                             <tr>  " +
                                                 "                                                <td width='100%'>  " +
                                                 "                                                   <table border='0' cellspacing='0' cellpadding='0' style='font-weight:200;font-family:Helvetica,Arial,sans-serif' width='100%'>  " +
                                                 "                                                      <tbody>  " +
                                                 "                                                         <tr>  " +
                                                 "                                                            <td align='center'  style='padding:20px 48px;color:#ffffff' class='banner-color'>  " +
                                                 "                                                               <table border='0' cellspacing='0' cellpadding='0' style='font-weight:200;font-family:Helvetica,Arial,sans-serif' width='100%'>  " +
                                                 "                                                                  <tbody>  " +
                                                 "                                                                     <tr>  " +
                                                 "                                                                        <td align='center' width='100%'>  " +
                                                 "   																		<h1 style='padding:0;margin:0;color:#ffffff;font-weight:500;font-size:20px;line-height:24px'>Invitation from HospitalName</h1>  " +
                                                 "                                                                        </td>  " +
                                                 "                                                                     </tr>  " +
                                                 "                                                                  </tbody>  " +
                                                 "                                                               </table>  " +
                                                 "                                                            </td>  " +
                                                 "                                                         </tr>  " +
                                                 "                                                         <tr>  " +
                                                 "                                                            <td align='center' style='padding:20px 0 10px 0'>  " +
                                                 "                                                               <table border='0' cellspacing='0' cellpadding='0' style='font-weight:200;font-family:Helvetica,Arial,sans-serif' width='100%'>  " +
                                                 "                                                                  <tbody>  " +
                                                 "                                                                     <tr>  " +
                                                 "   																		<td align='center' width='100%' style='padding: 0 15px;text-align: justify;color: rgb(76, 76, 76);font-size: 12px;line-height: 18px;'>  " +
                                                 "   																		<tr _ngcontent-kfm-c50<td _ngcontent-kfm-c50=''><img _ngcontent-kfm-c50='' src='{ImageUrl}' style='height: 80px;'></td></tr>  " +
                                                 "   																		<h3 style='font-weight: 600; padding: 0px; margin: 0px; font-size: 16px; line-height: 24px; text-align: center;' class='title-color'>Hi, This is <strong>DoctorNameTitle &nbsp;&nbsp;DoctorName</strong></h3>  " +      
                                                 "                                                                           <p style='margin: 20px 0 30px 0;font-size: 15px;text-align: center;'>EmailAdditionalContent &nbsp;</p>" +
                                                 "                                                                           <p style='margin: 20px 0 30px 0;font-size: 15px;text-align: center;'>Please click this following button to join me for a secure video meeting <b>Join now</b>!</p>  " +
                                                 "                                                                           <div style='font-weight: 200; text-align: center; margin: 25px;'><a style='padding:0.6em 1em;border-radius:600px;color:#ffffff;font-size:14px;text-decoration:none;font-weight:bold' class='button-color' href='{Join}'>Join the conference</a></div>  " +
                                                 "                                                                        </td>  " +
                                                 "                                                                     </tr>  " +
                                                 "                                                                  </tbody>  " +
                                                 "                                                               </table>  " +
                                                 "   															<table border='0' cellspacing='0' cellpadding='0' style='font-weight:200;font-family:Helvetica,Arial,sans-serif' width='100%'>  " +
                                                 "   																<tbody>  " +
                                                 "   																	<tr>  " +
                                                 "   																		<td align='center' valign='middle' width='100%' style='border-top:1px solid #d9d9d9;padding:12px 0px 20px 0px;text-align:center;color:#4c4c4c;font-weight:200;font-size:12px;line-height:18px'>Regards,  " +
                                                 "   																			<br><b>Powered by © Fewa Telehealth 2020</b>  " +
                                                 "   																		</td>  " +
                                                 "   																	</tr>  " +
                                                 "   																</tbody>  " +
                                                 "   															</table>  " +
                                                 "                                                            </td>  " +
                                                 "                                                         </tr>  " +
                                                 "                                                      </tbody>  " +
                                                 "                                                   </table>  " +
                                                 "                                                </td>  " +
                                                 "                                             </tr>  " +
                                                 "                                          </tbody>  " +
                                                 "                                       </table>  " +
                                                 "                                    </td>  " +
                                                 "                                 </tr>  " +
                                                 "                                 <tr>  " +
                                                 "                                  " +
                                                 "                                 </tr>  " +
                                                 "   							   <tr>  " +
                                                 "                                    <td bgcolor='#F3F3F3' width='100%' style='background-color:#f3f3f3;padding:12px;border-bottom:1px solid #ececec'>  " +
                                                 "                                       <table border='0' cellspacing='0' cellpadding='0' style='font-weight:200;width:100%!important;font-family:Helvetica,Arial,sans-serif;min-width:100%!important' width='100%'>  " +
                                                 "                                          <tbody>  " +
                                                 "                                             <tr>  " +
                                                 "   												<td align='left' valign='middle' width='50%'><span style='margin:0;color:#4c4c4c;white-space:normal;display:inline-block;text-decoration:none;font-size:12px;line-height:20px'></span></td>  " +
                                                 "                                                <td valign='middle' width='50%' align='right' style='padding:0 0 0 10px'><span style='margin:0;color:#4c4c4c;white-space:normal;display:inline-block;text-decoration:none;font-size:12px;line-height:20px'></span></td>  " +
                                                 "                                                <td width='1'>&nbsp;</td>  " +
                                                 "                                             </tr>  " +
                                                 "                                          </tbody>  " +
                                                 "                                       </table>  " +
                                                 "                                    </td>  " +
                                                 "                                 </tr>  " +
                                                 "                              </tbody>  " +
                                                 "                           </table>  " +
                                                 "                        </center>  " +
                                                 "                     </td>  " +
                                                 "                  </tr>  " +
                                                 "               </tbody>  " +
                                                 "            </table>  " +
                                                 "         </div>  " +
                                                 "      </body>  " +
                                                 "  </html>  ",
                           ValueDataType = "string",
                           DoctorId = "doc2"
                       },
                       new
                       {
                           Id = 50,
                           ParameterGroupName = "SMSAPI",
                           ParameterName = "AccountSID",
                           ParameterValue = "****",
                           ValueDataType = "string",
                           DoctorId = "doc2"
                       },
                       new
                       {
                           Id = 51,
                           ParameterGroupName = "SMSAPI",
                           ParameterName = "AuthToken",
                           ParameterValue = "****",
                           ValueDataType = "string",
                           DoctorId = "doc2"
                       },
                       new
                       {
                           Id = 52,
                           ParameterGroupName = "SMSAPI",
                           ParameterName = "PhoneNumber",
                           ParameterValue = "****",
                           ValueDataType = "string",
                           DoctorId = "doc2"
                       },
                       new
                       {
                           Id = 53,
                           ParameterGroupName = "Doctor",
                           ParameterName = "DoctorRoomName",
                           ParameterValue = "doctor",
                           ValueDataType = "string",
                           DoctorId = "doc2"
                       },
                       new
                       {
                           Id = 54,
                           ParameterGroupName = "Server",
                           ParameterName = "ServerName",
                           ParameterValue = "https://localhost:44304/",
                           ValueDataType = "string",
                           DoctorId = "doc2"
                       }
                         );
        }
    }
}
