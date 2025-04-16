public class DonationFilter
{
    public string FullName { get; set; }
    public List<string> BloodType { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public List<int> DonationTypeIds { get; set; }
    public List<int> RegionIds { get; set; }
    public List<int> CityIds { get; set; }
    public List<int> DonationCenterIds { get; set; }
    public List<int> Status { get; set; }
    public List<int> BloodVolume { get; set; }
    public PageFilter Page { get; set; }
}