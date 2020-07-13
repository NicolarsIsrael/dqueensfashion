using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DQueensFashion.Core
{

    public class BeginWIthAlphabeth : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string strValue = value as string;

            if (strValue.Length < 1)
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));

            if (!char.IsLetter(strValue[0]))
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));

            return ValidationResult.Success;
        }
    }

    public class BeginWIthAlphaNumeric : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string strValue = value as string;

            if (strValue.Length < 1)
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));

            if (!char.IsLetterOrDigit(strValue[0]))
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));

            return ValidationResult.Success;
        }
    }

    public class CustomPasswordValidator : IIdentityValidator<string>
    {
        public int MinLength { get; set; }
        public CustomPasswordValidator(int minLength)
        {
            MinLength = minLength;
        }

        // validate password: count how many types of characters exists in the password  
        public Task<IdentityResult> ValidateAsync(string password)
        {
            if (String.IsNullOrEmpty(password) || password.Length < MinLength)
            {
                return Task.FromResult(IdentityResult.Failed("Password too short"));
            }

            int counter = 0;
            List<string> patterns = new List<string>();
            patterns.Add(@"[a-z]"); // lowercase  
            patterns.Add(@"[A-Z]"); // uppercase  
            patterns.Add(@"[0-9]"); // digits  
            patterns.Add(@"[!@#$%^&*\(\)_\+\-\={}<>,\.\|""'~`:;\\?\/\[\] ]"); // special symbols

            // count type of different chars in password  
            foreach (string p in patterns)
            {
                if (Regex.IsMatch(password, p))
                {
                    counter++;
                }
            }

            if (counter < 2)
            {
                return Task.FromResult(IdentityResult.Failed("Password should contain atleast two of the following: lowercase, uppercase, digits, special symbols"));
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }

    public class CustomPasswordVal : ValidationAttribute
    {
        public int MinLength { get; set; }
        public CustomPasswordVal()
        {
            MinLength = 6;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string password = value as string;

            if (String.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password) || password.Length < MinLength)
            {
                return new ValidationResult("Password should be atleast 6 characters long");
            }

            int counter = 0;
            List<string> patterns = new List<string>();
            patterns.Add(@"[a-z]"); // lowercase  
            patterns.Add(@"[A-Z]"); // uppercase  
            patterns.Add(@"[0-9]"); // digits  
            patterns.Add(@"[!@#$%^&*\(\)_\+\-\={}<>,\.\|""'~`:;\\?\/\[\] ]"); // special symbols

            // count type of different chars in password  
            foreach (string p in patterns)
            {
                if (Regex.IsMatch(password, p))
                {
                    counter++;
                }
            }

            if (counter < 2)
            {
                return new ValidationResult("Password should contain atleast two of the following: lowercase, uppercase, digits, special symbols");
            }
            return ValidationResult.Success;
        }

    }

    public class AcceptTermsAndCondition : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool? termsAndcondition = value as bool?;

            if(termsAndcondition==null)
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));

            if(termsAndcondition==false)
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));

            return ValidationResult.Success;
        }
    }

}
