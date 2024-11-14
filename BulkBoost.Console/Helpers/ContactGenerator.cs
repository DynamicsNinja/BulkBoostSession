using Bogus;
using BulkBoost.Console.Model;

namespace BulkBoost.Console.Helpers
{
    public static class ContactGenerator
    {
        public static List<Contact> Generate(int number)
        {
            var contactGenerator = new Faker<Contact>()
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName));

            var contacts = contactGenerator.Generate(number);

            return contacts;
        }
    }
}
