using Bean_Mind.API.Service.Implement;
using Bean_Mind.API.Service.Interface;
using Bean_Mind_Business.Repository.Implement;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace Bean_Mind.API.Extensions
{
    public static class DependencyServices
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork<BeanMindContext>, UnitOfWork<BeanMindContext>>();
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            services.AddDbContext<BeanMindContext>(options => options.UseSqlServer(CreateConnectionString(configuration)));
            return services;
        }

        private static string CreateConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("ConnectionStrings:MyConnectionString");
            return connectionString;
        }

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
        {
            // string firebaseCred = config.GetValue<string>("Authentication:FirebaseKey");
            // // string firebaseCred = config.GetValue<string>("AIzaSyCFJOGAnHOQaWntVhN1a16QINIAjVpWaXI");
            // FirebaseApp.Create(new AppOptions()
            // {
            //     Credential = GoogleCredential.FromJson(firebaseCred)
            // }, "[DEFAULT]");

            //Account
            services.AddScoped<IAccountService, AccountService>();
            //School
            services.AddScoped<ISchoolService, SchoolService>();
            //Teacher
            services.AddScoped<ITeacherService, TeacherService>();
            //Parent
            services.AddScoped<IParentService, ParentService>();
            //Student
            services.AddScoped<IStudentService, StudentService>();
            //Subject
            services.AddScoped<ISubjectService, SubjectService>();
            //Chapter
            services.AddScoped<IChapterService, ChapterService>();
            //Topic
            services.AddScoped<ITopicService, TopicService>();
            //Course
            services.AddScoped<ICourseService, CourseService>();
            //Curriculum
            services.AddScoped<ICurriculumService, CurriculumService>();
            //Activity
            services.AddScoped<IActivityService, ActivityService>();
            //Video
            services.AddScoped<IVideoService, VideoService>();
            //Document
            services.AddScoped<IDocumentService, DocumentService>();
            //QuestionLevel
            services.AddScoped<IQuestionLevelService, QuestionLevelService>();

            return services;
        }

        public static IServiceCollection AddJwtValidation(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = "BeanMindSystem",
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes("0102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f"))
                };
            });
            return services;
        }

        public static IServiceCollection AddConfigSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo() { Title = "BeanMind System", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
                });
                options.MapType<TimeOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "time",
                    Example = OpenApiAnyFactory.CreateFromJson("\"13:45:42.0000000\"")
                });
            });
            return services;
        }
    }
}
