namespace AIDocIntelligence.WebApi.ViewModels
{
    public class SearchResult
    {
        public string Sentiment { get; set; } = string.Empty;
        public List<string> KeyPhraseResponse { get; set; } = new List<string>();
    }
}
