namespace AwesomeAngularMVCApp.Models
{
    /// <summary>
    /// This represents the implementation of IdentityUser for the application.
    /// </summary>
    /// <remarks>This extends the Entity Framework implementation of the IUser interface from the
    /// core AspNet.Identity package. While the IdentityUser could be used directly, this allows you to extend the data model and 
    /// protect your internal classes from leaking the EntityFramework into your app-specific code.</remarks>
    public class ApplicationUser : Microsoft.AspNet.Identity.EntityFramework.IdentityUser
    {
        public ApplicationUser() : base() { }

        //properties on this class will create columns in the EntityFramework code-first database
    }
}