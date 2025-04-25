using Azure;
using Azure.Data.Tables;

namespace ProcessImage
{
    public class OcrResultEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string DocumentName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string DateOfIssue { get; set; }
        public string DateOfExpiration { get; set; }
        public string DocumentNumber { get; set; }
        public string Nationality { get; set; }
        public string OcrResult { get; set; }
        public string OcrValues { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string DocumentType { get; set; }
        public string IssuingAuthority { get; set; }
        public string MachineReadableZone { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Sex { get; set; }
    }
}
