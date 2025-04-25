using Azure;
using Azure.Data.Tables;

namespace AIDocIntelligence.WebApi.ViewModels
{
    public class TablePassportData : PassportData, ITableEntity
    {
        public TablePassportData()
        { }

        public TablePassportData(PassportData passData)
        {
            FirstName = passData.FirstName;
            LastName = passData.LastName;
            DateOfBirth = passData.DateOfBirth;
            DateOfIssue = passData.DateOfIssue;
            DateOfExpiration = passData.DateOfExpiration;
            Nationality = passData.Nationality;
            DocumentType = passData.DocumentType;
            IssuingAuthority = passData.IssuingAuthority;
            MachineReadableZone = passData.MachineReadableZone;
            PlaceOfBirth = passData.PlaceOfBirth;
            Sex = passData.Sex;

            RowKey = passData.RowKey ?? Guid.NewGuid().ToString();
            PartitionKey = passData.PartitionKey;
        }

        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
