namespace Bean_Mind.API.Payload.Response.Schools
{
    public class GetSchoolResponse
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }
        public string? Email { get; set; } = null!;

        public string? Logo { get; set; }

        public string? Description { get; set; }


        //public int NumberOfSchools { get; set; } = 0;
        
        public GetSchoolResponse(Guid id, string name, string address, string phone, string email, string logo, string description)
        {
            Id = id;
            Name = name;
            Email = email;
            Address = address;
            Phone = phone;
            Logo = logo;
            Description = description;

        }

    }
}
