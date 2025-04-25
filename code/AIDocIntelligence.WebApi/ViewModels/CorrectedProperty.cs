namespace AIDocIntelligence.WebApi.ViewModels
{
    public class CorrectedProperty
    {
        public string PropertyName { get; set; } = string.Empty;
        public string SuggestedValue { get; set; } = string.Empty;
        public bool? IsValid { get; set; } = null;
    }
}
