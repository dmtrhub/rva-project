using Client.Models;

namespace Client.Services
{
    public class StateTracker
    {
        private readonly CruiseServiceProxy _service;

        public StateTracker(CruiseServiceProxy service)
        {
            _service = service;
        }

        public Dictionary<string, int> GetDetailedVoyageStateCounts()
        {
            try
            {
                var voyages = _service.GetAllVoyages();

                Console.WriteLine($"StateTracker: Found {voyages.Count} real voyages in database");

                var counts = new Dictionary<string, int>
                {
                    { "Scheduled", 0 },
                    { "OnTime", 0 },
                    { "Delayed", 0 },
                    { "Arrived", 0 },
                    { "Cancelled", 0 }
                };

                foreach (var voyage in voyages)
                {
                    var stateName = voyage.Status.ToString();
                    if (counts.ContainsKey(stateName))
                    {
                        counts[stateName]++;
                        Console.WriteLine($"Voyage {voyage.Code}: {stateName}");
                    }
                }

                Console.WriteLine($"State distribution: {string.Join(", ", counts.Select(kvp => $"{kvp.Key}:{kvp.Value}"))}");
                return counts;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in StateTracker: {ex.Message}");

                // Fallback na test podatke
                return new Dictionary<string, int>
                {
                    { "Scheduled", 2 },
                    { "OnTime", 1 },
                    { "Delayed", 0 },
                    { "Arrived", 0 },
                    { "Cancelled", 0 }
                };
            }
        }

        public List<VoyageStateInfo> GetVoyageStateInfo()
        {
            var voyages = _service.GetAllVoyages();
            return voyages
                .Select(v => new VoyageStateInfo
                {
                    VoyageCode = v.Code,
                    State = v.Status.ToString(),
                    Timestamp = DateTime.Now
                })
                .ToList();
        }
    }
}