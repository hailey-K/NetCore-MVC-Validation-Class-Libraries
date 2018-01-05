using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using HKClassLibrary;

namespace a2OEC.Models
{
   
    [ModelMetadataTypeAttribute(typeof(FarmMetadata))]
    public partial class Farm : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            OECContext _context = OEC_Singleton.Context();

            FarmId = Convert.ToInt32(FarmId.ToString().Trim());
            if(Name != null)
            {
                Name = Name.Trim();
                Name = HKValidations.HKCapitalize(Name);
            }
            if (Address != null)
            {
                Address = Address.Trim();
                Address = HKValidations.HKCapitalize(Address);
            }
            if (Town != null)
            {
                Town = Town.Trim();
                Town = HKValidations.HKCapitalize(Town);
            }
            if (County != null)
            {
                County = County.Trim();
                County = HKValidations.HKCapitalize(County);
            }
            if (ProvinceCode != null)
            {
                ProvinceCode = ProvinceCode.Trim();
                ProvinceCode = ProvinceCode.ToUpper();
            }
            if (PostalCode != null)
            {
                PostalCode = PostalCode.Trim();
            }
            if (HomePhone != null)
            {
                HomePhone = HomePhone.Trim();
            }
            if (CellPhone != null)
            {
                CellPhone = CellPhone.Trim();
            }
            if (Email != null)
            {
                Email = Email.Trim();
            }
            if (Directions != null)
            {
                Directions = Directions.Trim();
            }

            if(String.IsNullOrWhiteSpace(Name) || String.IsNullOrWhiteSpace(ProvinceCode))
            {
                if (String.IsNullOrWhiteSpace(Name))
                {
                    yield return new ValidationResult("Name is required", new[] { "Name"});
                }
                if(String.IsNullOrWhiteSpace(ProvinceCode))
                {
                    yield return new ValidationResult("Province Code is required", new[] { "ProvinceCode" });
                }
            }
          
            if (String.IsNullOrWhiteSpace(Town) && String.IsNullOrWhiteSpace(County))
            {
                yield return new ValidationResult("Either the town or county must be provided.", new[] { "Town", "County" });
            }
        
            if (String.IsNullOrWhiteSpace(Email))
            {
                if (string.IsNullOrWhiteSpace(Address))
                {
                    yield return new ValidationResult("Address must be provided.", new[] { "Address" });
                }
                if (string.IsNullOrWhiteSpace(PostalCode))
                {
                    yield return new ValidationResult("Postal Code must be provided.", new[] { "PostalCode" });
                }
            }
                if (!String.IsNullOrEmpty(PostalCode)&& !String.IsNullOrWhiteSpace(ProvinceCode))
                {
                string countryCode = "";
                if (ProvinceCode.Length == 2)
                {
                    countryCode = _context.Province.SingleOrDefault(p => p.ProvinceCode == ProvinceCode).CountryCode;
                }
                else
                {
                    countryCode = _context.Province.SingleOrDefault(p => p.Name == ProvinceCode).CountryCode;
                }
                  
                    if (countryCode == "CA")
                    {
                        string pCode = PostalCode;

                        if (!HKValidations.HKPostalCodeValidation(ref pCode))
                        {
                            yield return new ValidationResult("Please enter valid postal code.", new[] { "PostalCode" });
                        }
                    PostalCode = pCode;
                    }
                    else if (countryCode == "US")
                    {
                        string zCode = PostalCode;
                        if (!HKValidations.HKZipCodeValidation(ref zCode))
                        {
                            yield return new ValidationResult("Please enter valid zip code.", new[] { "PostalCode" });
                        }
                    PostalCode = zCode;
                }
            }
        
            if (String.IsNullOrWhiteSpace(HomePhone) && String.IsNullOrWhiteSpace(CellPhone))
            {
                yield return new ValidationResult("Either the home phone number or cell phone number must be provided.", new[] { "HomePhone", "CellPhone" });
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(HomePhone))
                {
                    string hPhone = HomePhone;
                    if (!HKValidations.HKPhoneNumberValidation(ref hPhone))
                    {
                        yield return new ValidationResult("Home phone number must be 10 digits only.", new[] { "HomePhone" });
                    }
                    HomePhone = hPhone;
                }
                if (!String.IsNullOrWhiteSpace(CellPhone))
                {
                    string cPhone = CellPhone;
                    if (!HKValidations.HKPhoneNumberValidation(ref cPhone))
                    {
                        yield return new ValidationResult("Cell phone number must be 10 digits only.", new[] { "CellPhone" });
                    }
                    HomePhone = cPhone;
                }

            }

            yield return ValidationResult.Success;
        }
    }
    public class FarmMetadata
    {
        public int FarmId { get; set; }
        [Display(Name = "Farm Name")]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        [Display(Name = "Province")]
        [Remote("checkProvinceCode", "HKRemote")]
        public string ProvinceCode { get; set; }
        public string PostalCode { get; set; }
        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }
        [Display(Name = "Cell Phone")]
        public string CellPhone { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Directions { get; set; }
        [Display(Name = "Date Joined")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [HKDateNotInFutureAttribute()]
        public DateTime? DateJoined { get; set; }
        [Display(Name = "Last Contact")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: dd MMM yyyy}")]
        [HKDateNotInFutureAttribute()]
        [Remote("CheckDates", "HKRemote", AdditionalFields = "DateJoined")]
        public DateTime? LastContactDate { get; set; }
        [Display(Name = "Province")]
        public Province ProvinceCodeNavigation { get; set; }
        public ICollection<Plot> Plot { get; set; }

    }
}
