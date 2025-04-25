using Azure;
using Azure.Data.Tables;

namespace AIDocIntelligence.WebApi.ViewModels
{
    public class OcrPassportData : ITableEntity
    {
        public string PartitionKey { get; set; } = string.Empty;
        public string RowKey { get; set; } = string.Empty;
        public string DocumentName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public string DateOfIssue { get; set; } = string.Empty;
        public string DateOfExpiration { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public string OcrResult { get; set; } = string.Empty;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string IssuingAuthority { get; set; } = string.Empty;
        public string MachineReadableZone { get; set; } = string.Empty;
        public string PlaceOfBirth { get; set; } = string.Empty;
        public string Sex { get; set; } = string.Empty;
    }
}
