using Microsoft.Xrm.Sdk;

namespace BulkBoost.Console.Model
{
    public class Contact
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }

        public Entity ToEntity()
        {
            var entity = new Entity("contact")
            {
                ["firstname"] = FirstName,
                ["lastname"] = LastName,
                ["emailaddress1"] = Email
            };

            return entity;
        }
    }
}
