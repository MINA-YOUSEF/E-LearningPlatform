namespace E_LearningPlatform.Domain.Entities
{
    public class Tag : BaseEntity
    {
        public string Name { get; private set; }
        public string Slug { get; private set; }

        private Tag() { }

        public Tag(string name, string slug)
        {
            Name = name;
            Slug = slug;
        }
    }
}
