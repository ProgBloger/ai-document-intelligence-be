namespace AIDocIntelligence.WebApi.ViewModels
{
    public class PassportData
    {
        public string RowKey { get; set; } = string.Empty;
        public string PartitionKey { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime DateOfExpiration { get; set; }
        public string Nationality { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string? IssuingAuthority { get; set; }
        public string MachineReadableZone { get; set; } = string.Empty;
        public string PlaceOfBirth { get; set; } = string.Empty;
        public string Sex { get; set; } = string.Empty;
    }
}
