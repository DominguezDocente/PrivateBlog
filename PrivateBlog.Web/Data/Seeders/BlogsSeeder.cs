using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Data.Entities;

namespace PrivateBlog.Web.Data.Seeders
{
    public class BlogsSeeder
    {
        private readonly DataContext _context;
        public BlogsSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            Section section = await _context.Sections.FirstOrDefaultAsync();

            List<Blog> blogs = new List<Blog>()
            {
                new Blog { Id = Guid.NewGuid(), Name = "Blog 1", Content = "<p> Blog 1 </p>", SectionId = section.Id },
                new Blog { Id = Guid.NewGuid(), Name = "Blog 2", Content = "<p> Blog 2 </p>", SectionId = section.Id },
                new Blog { Id = Guid.NewGuid(), Name = "Blog 3", Content = "<p> Blog 3 </p>", SectionId = section.Id },
            };

            foreach (Blog blog in blogs)
            {
                bool exists = await _context.Blogs.AnyAsync(s => s.Name == blog.Name);

                if (!exists)
                {
                    await _context.Blogs.AddAsync(blog);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
