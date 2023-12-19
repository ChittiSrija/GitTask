namespace EVOKETASK.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    // State.cs
    public class State
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
}