using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;

namespace Bean_Mind.API.Service
{
    public class GoogleDriveService
    {
        private readonly IConfiguration _configuration;

        public GoogleDriveService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> UploadToGoogleDriveAsync(IFormFile fileToUpload)
        {
            GoogleCredential credential;

            // Đọc đường dẫn đến tệp Beanmind.json từ cấu hình ứng dụng
            string credentialsPath = _configuration["Authentication:GoogleDriveKey:filePath"];

            // Chuyển đổi đường dẫn tương đối thành đường dẫn tuyệt đối
            string absoluteCredentialsPath = Path.Combine(Directory.GetCurrentDirectory(), credentialsPath);

            try
            {
                // Đọc thông tin xác thực từ tệp Beanmind.json
                using (var stream = new FileStream(absoluteCredentialsPath, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream).CreateScoped(new[] { DriveService.ScopeConstants.DriveFile });
                }

                // Khởi tạo dịch vụ Drive
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Google Drive Upload Console App"
                });

                // Tạo metadata cho tệp
                var fileMetaData = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = fileToUpload.FileName,
                    Parents = new List<string> { "1sFih0Ciu8E-itSd59tSCpZtUFaTmgdaM" }
                };

                // Tải tệp lên Google Drive
                FilesResource.CreateMediaUpload request;
                using (var stream = fileToUpload.OpenReadStream())
                {
                    request = service.Files.Create(fileMetaData, stream, fileToUpload.ContentType);
                    request.Fields = "id";
                    await request.UploadAsync();
                }

                // Lấy thông tin tệp đã tải lên
                var file = request.ResponseBody;
                string fileUrl = $"https://drive.google.com/file/d/{file.Id}/view?usp=sharing";
                return fileUrl;
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có
                Console.WriteLine($"Error uploading file to Google Drive: {ex.Message}");
                return null;
            }
        }
    }
}
