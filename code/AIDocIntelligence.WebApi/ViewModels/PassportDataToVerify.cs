namespace AIDocIntelligence.WebApi.ViewModels
{
    public class PassportDataToVerify
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public string DateOfIssue { get; set; } = string.Empty;
        public string DateOfExpiration { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string? IssuingAuthority { get; set; }
        public string MachineReadableZone { get; set; } = string.Empty;
        public string PlaceOfBirth { get; set; } = string.Empty;
        public string Sex { get; set; } = string.Empty;
    }
}
