using Catalyst;


using Mosaik.Core;



namespace Vault.Tasks
{
    public class NlpService
    {
        private Pipeline? _nlp;
        private readonly ILogger<NlpService> _logger;
        private bool _isLoaded = false;

        public NlpService(ILogger<NlpService> logger)
        {
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            if (_isLoaded) return;

            try 
            {
                _logger.LogInformation("Loading English NLP Models...");
                
                // 1. Register the language model
                Catalyst.Models.English.Register(); 

                // 2. Load the pipeline (Model ~200MB, downloads automatically)
                _nlp = await Pipeline.ForAsync(Language.English);
                
                _isLoaded = true;
                _logger.LogInformation("NLP Models Loaded Successfully.");
            }
            catch (Exception ex)
            {
                // Log the FULL exception to see the NullReference source
                _logger.LogError(ex, "CRITICAL: Failed to load NLP models.");
            }
        }

        public Dictionary<string, List<string>> ExtractEntities(string text)
        {
            var results = new Dictionary<string, List<string>>();
            
            // Safety Check: If model failed to load or text is empty, return empty
            if (!_isLoaded || _nlp == null || string.IsNullOrWhiteSpace(text)) 
            {
                return results; 
            }

            try
            {
                // Create document
                var doc = new Document(text, Language.English);
                
                // Process (This is where the magic happens)
                _nlp.ProcessSingle(doc);

                // Extract all valid entities
                var entities = doc.SelectMany(span => span.GetEntities()).ToList();
                
                // [DEBUG LOGGING]
                _logger.LogInformation($"DEBUG: NLP Found {entities.Count} raw entities.");
                foreach(var e in entities)
                {
                    _logger.LogInformation($"DEBUG Entity: '{e.Value}' | Type: '{e.EntityType.Type}'");
                }

                // 1. Persons
                var persons = entities
                    .Where(e => e.EntityType.Type == "PER" || e.EntityType.Type == "Person")
                    .Select(e => e.Value)
                    .Distinct()
                    .ToList();
                if (persons.Any()) results["persons"] = persons;

                // 2. Locations
                var locations = entities
                    .Where(e => e.EntityType.Type == "LOC" || e.EntityType.Type == "Loc" || e.EntityType.Type == "GPE")
                    .Select(e => e.Value)
                    .Distinct()
                    .ToList();
                if (locations.Any()) results["locations"] = locations;

                // 3. Organizations
                var orgs = entities
                    .Where(e => e.EntityType.Type == "ORG" || e.EntityType.Type == "Org")
                    .Select(e => e.Value)
                    .Distinct()
                    .ToList();
                if (orgs.Any()) results["organizations"] = orgs;

                // 4. Emails / URLs (Catalyst detects these as 'EmailOrURL')
                var emails = entities
                    .Where(e => (e.EntityType.Type == "EmailOrURL" || e.EntityType.Type == "Email") && e.Value.Contains("@"))
                    .Select(e => e.Value)
                    .Distinct()
                    .ToList();
                if (emails.Any()) results["emails"] = emails;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during NLP extraction");
            }

            return results;
        }
    }
}
