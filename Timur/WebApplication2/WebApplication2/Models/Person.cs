namespace WebApplication2.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Pesel { get; set; }
        public static List<Person> GetPeople()
        {
            return new List<Person>()
            {
                new Person { Id = 1, Name = "Jan", Surname = "Kowalski", Pesel = "90010112345" },
                new Person { Id = 2, Name = "Anna", Surname = "Nowak", Pesel = "92030554321" },
                new Person { Id = 3, Name = "Piotr", Surname = "Wiśniewski", Pesel = "88071298765" }
            };



        }
    };
}
