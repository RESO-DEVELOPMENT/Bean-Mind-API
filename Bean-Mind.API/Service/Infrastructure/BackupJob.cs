using Bean_Mind.API.Service;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bean_Mind_Business.Infrastructure
{
    public class BackupJob : IJob
    {
        private readonly AzureDatabaseService _azureDatabaseService;
        private readonly GoogleDriveService _googleDriveService;

        public BackupJob(AzureDatabaseService azureDatabaseService, GoogleDriveService googleDriveService)
        {
            _azureDatabaseService = azureDatabaseService;
            _googleDriveService = googleDriveService;
        }

        // Phương thức được gọi khi công việc được lên lịch thực thi
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                // Gọi phương thức sao lưu từ AzureDatabaseService
                var fileToUpload = await _azureDatabaseService.BackupToExcel();

                // Upload file lên Google Drive nếu sao lưu thành công
                if (fileToUpload != null)
                {
                    var fileUrl = await _googleDriveService.UploadToGoogleDriveAsync(fileToUpload);
                    Console.WriteLine($"File uploaded to Google Drive: {fileUrl}");
                }
                else
                {
                    Console.WriteLine("Backup failed or no file generated.");
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có lỗi trong quá trình sao lưu hoặc upload
                Console.WriteLine($"Error during backup and upload: {ex.Message}");
            }
        }
    }
}
