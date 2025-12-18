using System.Text.Json.Serialization;

namespace tempSMK.Models
{
    public class Job
    {
        [JsonPropertyName("jobId")]
        public int JobId { get; set; }
        
        [JsonPropertyName("customerId")]
        public int CustomerId { get; set; }
        
        [JsonPropertyName("startLat")]
        public double StartLat { get; set; }
        
        [JsonPropertyName("startLng")]
        public double StartLng { get; set; }
    }

    public class Vehicle
    {
        [JsonPropertyName("vehicleId")]
        public int VehicleId { get; set; }
        
        [JsonPropertyName("capacity")]
        public int Capacity { get; set; }
    }

    public class Depot
    {
        [JsonPropertyName("depotId")]
        public int DepotId { get; set; }
        
        [JsonPropertyName("lat")]
        public double Lat { get; set; }
        
        [JsonPropertyName("lng")]
        public double Lng { get; set; }
    }

    public class AllocationRequest
    {
        [JsonPropertyName("jobs")]
        public List<Job> Jobs { get; set; } = new List<Job>();
        
        [JsonPropertyName("vehicles")]
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        
        [JsonPropertyName("depots")]
        public List<Depot> Depots { get; set; } = new List<Depot>();
    }

    public class AllocationResponse
    {
        [JsonPropertyName("receivedJobs")]
        public int ReceivedJobs { get; set; }
        
        [JsonPropertyName("receivedVehicles")]
        public int ReceivedVehicles { get; set; }
        
        [JsonPropertyName("receivedDepots")]
        public int ReceivedDepots { get; set; }
        
        [JsonPropertyName("jobs")]
        public List<Job> Jobs { get; set; } = new List<Job>();
        
        [JsonPropertyName("vehicles")]
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        
        [JsonPropertyName("depots")]
        public List<Depot> Depots { get; set; } = new List<Depot>();
    }
}
