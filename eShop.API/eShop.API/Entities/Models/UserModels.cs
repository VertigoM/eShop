using System.ComponentModel.DataAnnotations;

namespace eShop.API.Entities.Models
{
    /*
     * Class used to access Login Information from the end-user
     */
    public class LoginUser
    {
        [Required]
        public string Username { get; set;}

        [Required]
        public string Password { get; set;}
    }

    /*
     * Class used to register a new user to the application
     */
    public class RegisterUser
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set;}

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set;}

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set;}
    }

    /*
     * Class used to crate a role for the application
     */
    public class ApplicationRole
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }

    /*
     * Class used to assign role to the user
     */
    public class UserRole
    {
        public string Username { get; set; }
        public string RoleName { get; set; }
    }

    /*
     * Class used to retrieve the user's information
     */
    public class User
    {
        public string Email { get; set; }
        public string Username { get; set; }
    }
}
