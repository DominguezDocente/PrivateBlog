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

            List<Blog> blogs = new List<Blog>
            {
                new Blog { Name  = "Blog 1", SectionId = section.Id, Content = "<p> Blog 1 </p>", IsPublished = true },
                new Blog { Name  = "Blog 2", SectionId = section.Id, Content = "<p> Blog 2 </p>", IsPublished = true },
                new Blog { Name  = "Blog 3", SectionId = section.Id, Content = "<p> Blog 3 </p>", IsPublished = true }
            };

            foreach (Blog blog in blogs)
            {
                bool exists = await _context.Blogs.AnyAsync(b => b.Name == blog.Name && b.SectionId == blog.SectionId);

                if (!exists)
                {
                    await _context.Blogs.AddAsync(blog);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
