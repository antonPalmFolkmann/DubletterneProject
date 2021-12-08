
namespace Infrastructure
{
    public class ResourceRepository : IResourceRepository
    {

        private ILearningContext _context;

        public ResourceRepository(ILearningContext context)
        {
            _context = context;
        }

        public async Task<(Response, int resourceID)> CreateAsync(ResourceCreateDTO resource)
        {
            var ResourceWithSameName = from c in _context.Resources
                                       where c.Title == resource.Title
                                       select new ResourceDTO{
                                           Id = c.Id,
                                           Title = c.Title,
                                           User = c.User
                                       };


            if (ResourceWithSameName.Count() != 0)
            {
                return (Response.Conflict, -1);
            }

            var entity = new Resource()
            {
                Title = resource.Title,
                User = resource.User,
                Created = resource.Created,
                TextParagraphs = await GetTextParagraphsAsync(resource.TextParagraphs).ToListAsync(),
                ImageUrl = resource.ImageUrl

            };

            _context.Resources.Add(entity);

            await _context.SaveChangesAsync();

            return (Response.Created, entity.Id);
        }

        public async Task<Response> DeleteAsync(int resourceID)
        {
            var entity = await _context.Resources.FindAsync(resourceID);
            
            if (entity == null)
            {
                return Response.NotFound;
            }

            _context.Resources.Remove(entity);
            await _context.SaveChangesAsync();

            return Response.Deleted;
        }

        public async Task<IReadOnlyCollection<ResourceDTO>> ReadAllAsync()
        {
            var resources = (await _context.Resources
                                 .Select(r => new ResourceDTO{Id = r.Id, Title = r.Title, User = r.User})
                                 .ToListAsync())
                                 .AsReadOnly();
            return resources;
        }

        public Task<IReadOnlyCollection<ResourceDTO>> ReadAllByAuthorAsync(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public async Task<Option<ResourceDetailsDTO>> ReadAsync(int resourceID)
        {
            var resource = await _context.Resources
                                .Where(r => r.Id == resourceID)
                                .Select(r => new ResourceDetailsDTO{
                                    Id = r.Id,
                                    Title = r.Title,
                                    User = r.User,
                                    Created = r.Created,
                                    Updated = r.Updated,
                                    TextParagraphs = r.TextParagraphs.Select(p => p.Paragraph).ToList(),
                                    ImageUrl = r.ImageUrl
                                }).FirstOrDefaultAsync();
            return resource;
        }

        public async Task<Response> UpdateAsync(int id, ResourceUpdateDTO resource)
        {
            var entity = await _context.Resources.FirstOrDefaultAsync(c => c.Id == resource.Id);
                                    
            if (entity == null) return Response.NotFound;

            var conflict = await _context.Resources
                                         .Where(r => r.Id != id)
                                         .Where(r => r.Title == resource.Title)
                                         .Select(r => new ResourceDTO{Id = r.Id, Title = r.Title, User = r.User})
                                         .AnyAsync();
            
            if (conflict) return Response.Conflict;

            entity.Title = resource.Title;
            entity.User = resource.User;
            entity.Created  = resource.Created;
            entity.TextParagraphs = await GetTextParagraphsAsync(resource.TextParagraphs).ToListAsync();
            entity.Updated = DateTime.Now;
            entity.ImageUrl = resource.ImageUrl;

            await _context.SaveChangesAsync();

            return Response.Updated;
        }

        private async IAsyncEnumerable<TextParagraph> GetTextParagraphsAsync(IEnumerable<string> textParagraphs)
        {
            var existing = await _context.TextParagraphs.Where(t => textParagraphs.Contains(t.Paragraph)).ToDictionaryAsync(t => t.Paragraph);
            
            foreach (var item in textParagraphs)
            {
                yield return existing.TryGetValue(item, out var t) ? t : new TextParagraph(item);
            }
        }
    }
}