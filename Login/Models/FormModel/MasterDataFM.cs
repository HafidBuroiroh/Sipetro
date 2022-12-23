using DocumentFormat.OpenXml.Wordprocessing;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;


namespace Login.Models.FormModel
{
    public class MasterDataFM : IValidatableObject
    {
        public List<Master_Data> Master_Data { get; set; }

        [Display(Name = "From :")]
        public DateTime? DateFrom { get; set; }
        [Display(Name = "Until :")]

        public DateTime? DateUntil { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateFrom == null && DateUntil == null)
            {
                yield return new ValidationResult("input salah satu search!");
                //Memberi validation jika sebuah kolom pencarian tidak diisi!
            }
        }
    }
   
}
