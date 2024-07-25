using Bean_Mind.API.Payload.Response.GoogleDrivers;
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

        public async Task<GoogleDriverResponce> UploadToGoogleDriveAsync(IFormFile fileToUpload)
        {
            var allowedExtensions = new List<string> { ".docx", ".pdf", ".mov", ".xlsx", ".mp4", ".pdf" };

            // Lấy đuôi file từ file được tải lên
            var fileExtension = Path.GetExtension(fileToUpload.FileName).ToLower();

            // Kiểm tra xem đuôi file có trong danh sách được phép không
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new InvalidOperationException("Chỉ các định dạng tệp .docx, .pdf, và .mov được phép tải lên.");
            }

            GoogleCredential credential;

            //// Đọc đường dẫn đến tệp Beanmind.json từ cấu hình ứng dụng
            //string credentialsPath = _configuration["Authentication:GoogleDrive:CredentialsPath"];

            //// Chuyển đổi đường dẫn tương đối thành đường dẫn tuyệt đối
            //string absoluteCredentialsPath = Path.Combine(Directory.GetCurrentDirectory(), credentialsPath);

            var credentialsSection = _configuration.GetSection("Authentication:GoogleDrive:CredentialsPath");
            var credentialsJson = credentialsSection.GetChildren().ToDictionary(x => x.Key, x => x.Value);
            var credentialsJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(credentialsJson);

            try
            {
                // Đọc thông tin xác thực từ tệp Beanmind.json
                //using (var stream = new FileStream(absoluteCredentialsPath, FileMode.Open, FileAccess.Read))
                //{
                //    credential = GoogleCredential.FromStream(stream).CreateScoped(new[] { DriveService.ScopeConstants.DriveFile });
                //}

                using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(credentialsJsonString)))
                {
                    credential = GoogleCredential.FromStream(stream).CreateScoped(new[] { DriveService.ScopeConstants.DriveFile });
                }

                // Khởi tạo dịch vụ Drive
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Google Drive Upload Console App"
                });

                // Kiểm tra xem tệp có tồn tại trong thư mục Google Drive không
                var listRequest = service.Files.List();
                listRequest.Q = $"name='{fileToUpload.FileName}' and '{_configuration["Authentication:GoogleDrive:FolderId"]}' in parents and trashed=false";
                listRequest.Fields = "files(id, name)";

                var fileList = await listRequest.ExecuteAsync();


                if (fileList.Files.Count > 0)
                {
                    // Nếu tệp đã tồn tại, trả về liên kết của tệp hiện có
                    string existingFileId = fileList.Files.First().Id;
                    string existingFileUrl = $"https://drive.google.com/file/d/{existingFileId}/view?usp=sharing";
                    GoogleDriverResponce googleDriverResponce = new GoogleDriverResponce{Url = existingFileUrl, Existed = true };
                    return googleDriverResponce;
                }

                // Tạo metadata cho tệp
                var fileMetaData = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = fileToUpload.FileName,
                    Parents = new List<string> { _configuration["Authentication:GoogleDrive:FolderId"] }
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
                GoogleDriverResponce googleDriverResponce1 = new GoogleDriverResponce { Url = fileUrl, Existed = false };
                return googleDriverResponce1;
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
