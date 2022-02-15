using FakeSmtp.Models;
using netDumbster.smtp;

namespace FakeSmtp
{
    public class Startup
    {
        public static SimpleSmtpServer SmtpServer { get; set; }
        public static bool IsSmtpServerOn { get; set; }

        public static int MaximumLimit { get; set; }
        public static List<Email> ReceivedEmails { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ReceivedEmails = new List<Email>();
            StartSmtpServer(5000, 1000);
            services.AddControllersWithViews();
            services.AddSession(options => {});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        //protected void Application_Start()
        //{
        //    //AreaRegistration.RegisterAllAreas();
        //    //RouteConfig.RegisterRoutes(RouteTable.Routes);
        //    ReceivedEmails = new List<Email>();
        //    StartSmtpServer(5000, 1000);
        //    // get the hub from the globalHost 
        //    //_messageHubContext = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
        //}

        public static void StartSmtpServer(int port, int limit)
        {
            if (ReceivedEmails.Count > limit)
            {
                ReceivedEmails.RemoveRange(limit - 1, ReceivedEmails.Count - limit);
            }

            SmtpServer = SimpleSmtpServer.Start(port);
            IsSmtpServerOn = true;
            MaximumLimit = limit;

            SmtpServer.MessageReceived += SmtpServer_MessageReceived;
        }

        public static void StopSmtpServer()
        {
            SmtpServer.MessageReceived -= SmtpServer_MessageReceived;
            SmtpServer.ClearReceivedEmail();
            SmtpServer.Stop();
            IsSmtpServerOn = false;
        }

        private static void SmtpServer_MessageReceived(object sender, MessageReceivedArgs e)
        {
            if (ReceivedEmails.Count == MaximumLimit)
            {
                ReceivedEmails.RemoveAt(ReceivedEmails.Count - 1);
            }

            var newEmailId = (ReceivedEmails.Count == 0) ? 1 : ReceivedEmails[0].Id + 1;

            ReceivedEmails.Insert(0, new Email(e.Message, newEmailId));
            SmtpServer.ClearReceivedEmail();
            //_messageHubContext.Clients.All.newMessage(ReceivedEmails[0]);
        }

        protected void Application_End()
        {
            SmtpServer.Stop();
            IsSmtpServerOn = false;
        }
    }
}
