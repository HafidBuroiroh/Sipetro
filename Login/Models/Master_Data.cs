using System.ComponentModel.DataAnnotations;

namespace Login.Models
{

    public enum LOC_ASING
    {
        F, D
    }
    public class Master_Data
    {
        [Key]
        public int PERSONID { get; set; }
        public int? POPULATIONREGISTERNO { get; set; }
        public int? TAXREGISTERNO { get; set; }
        public int? PASSPORTNO { get; set; }
        public int? KITAS { get; set; }
        public string? FULL_NAME { get; set; }
        public int PHONE { get; set; }
        public int GENDER { get; set; }
        public string? EMAIL { get; set; }
        public DateTime BIRTHDATE { get; set; }
        public string? HIGHSCHOOL { get; set; }
        public string? UNIVERSITY { get; set; }
        public string? UNIVERSITYMAJOR { get; set; }
        public string? MSSCHOOL { get; set; }
        public string? MSMAJOR { get; set; }
        public string? PHDSCHOOL { get; set; }
        public string? PHDMAJOR { get; set; }
        public string? POSITION { get; set; }
        public string? COUNTRYCODE { get; set; }
        public int PROVINCECODE { get; set; }
        public string? ADDRESS { get; set; }
        public int CITY { get; set; }
        public int? POSTALCODE { get; set; }
        public int? BUSINESS_REG_NO { get; set; }
        public int? OCCUPATION { get; set; }
        public LOC_ASING? LOC_ASING { get; set; }
        public string? NASIONALITY { get; set; }
        public DateTimeOffset DateCreated { get; set; }

        public ICollection<Master_Data>? MasterData { get; set; }
      
    }
 
}
