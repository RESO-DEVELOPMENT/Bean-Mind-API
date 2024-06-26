using AutoMapper;
using Bean_Mind.API.Service.Implement;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using System.Data;

namespace Bean_Mind.API.Service
{
    public class AzureDatabaseService : BaseService<AzureDatabaseService>
    {
        public AzureDatabaseService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<AzureDatabaseService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<IFormFile> BackupToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            try
            {
                // Lấy danh sách các bảng trong database
                var tables = _unitOfWork.Context.Model.GetEntityTypes()
                    .Select(t => t.GetTableName())
                    .Distinct()
                    .ToList();

                using (var package = new ExcelPackage())
                {
                    foreach (var table in tables)
                    {
                        // Đọc dữ liệu từ từng bảng
                        var query = $"SELECT * FROM {table}";
                        var dataTable = await GetDataFromDatabase(query);

                        // Tạo worksheet cho từng bảng
                        var worksheet = package.Workbook.Worksheets.Add(table);

                        // Đổ dữ liệu vào worksheet
                        worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);
                    }

                    // Lưu file Excel vào memory stream
                    using (var memoryStream = new MemoryStream())
                    {
                        await package.SaveAsAsync(memoryStream);

                        // Đặt lại vị trí của memory stream về đầu để đọc lại từ đầu
                        memoryStream.Position = 0;

                        // Tạo bản sao của memory stream để sử dụng cho FormFile
                        var memoryStreamCopy = new MemoryStream(memoryStream.ToArray());

                        // Tạo đối tượng IFormFile từ memory stream
                        var fileName = $"backup_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                        var formFile = new FormFile(memoryStreamCopy, 0, memoryStreamCopy.Length, null, fileName)
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                        };

                        return formFile;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi trong quá trình sao lưu: {ex.Message}");
                return null;
            }
        }

        private async Task<DataTable> GetDataFromDatabase(string query)
        {
            var dataTable = new DataTable();

            // Thực hiện truy vấn và lấy dữ liệu từ cơ sở dữ liệu
            using (var command = _unitOfWork.Context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                await _unitOfWork.Context.Database.OpenConnectionAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }

            return dataTable;
        }
    }
}
