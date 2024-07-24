namespace AdminHRM.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }

        // Navigation property
        public Category Parent { get; set; }
        public ICollection<Category> Children { get; set; }
    }
}
