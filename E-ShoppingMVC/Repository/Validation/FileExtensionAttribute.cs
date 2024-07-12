using System.ComponentModel.DataAnnotations;

namespace E_ShoppingMVC.Repository.Validation
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                string[] extensionsValid = { "jpg", "png", "jpeg" };

                bool res = extensionsValid.Any(x => extension.EndsWith(x));
                if (!res)
                {
                    return new ValidationResult("Chon cac file jpg, jpeg, png.");
                }

            }
            return ValidationResult.Success;
        }
    }
}
