
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
                                       };


            if (ResourceWithSameName.Count() != 0)
            {
                return (Response.Conflict, -1);
            }

            var entity = new Resource()
            {
                Title = resource.Title,
                User = _context.Users.Where(u => u.Id == resource.User.Id).First(),
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
            var resource = await _context.Resources.FindAsync(resourceID);
            var entity = await ReadAsync(resourceID);
            
            if (resource == null)
            {
                return Response.NotFound;
            }
            
            
            foreach (var item in entity.Value.TextParagraphs)
            {
                _context.TextParagraphs.Remove(_context.TextParagraphs.Single(t => t.Paragraph == item));
            }

            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();

            return Response.Deleted;
        }

        public async Task<IReadOnlyCollection<ResourceDTO>> ReadAllAsync()
        {
            var resources = (await _context.Resources
                                 .Select(r => new ResourceDTO{Id = r.Id, Title = r.Title})
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
            var resource = _context.Resources
                                .Where(r => r.Id == resourceID)
                                .Select(r => new ResourceDetailsDTO{
                                    Id = r.Id,
                                    Title = r.Title,
                                    Created = r.Created,
                                    Updated = r.Updated,
                                    TextParagraphs = r.TextParagraphs.Select(p => p.Paragraph).ToList(),
                                    ImageUrl = r.ImageUrl
                                });
            return await resource.FirstOrDefaultAsync();
        }

        public async Task<Response> UpdateAsync(int id, ResourceUpdateDTO resource)
        {
            var entity = await _context.Resources.FirstOrDefaultAsync(c => c.Id == resource.Id);
                                    
            if (entity == null) return Response.NotFound;

            var conflict = await _context.Resources
                                         .Where(r => r.Id != id)
                                         .Where(r => r.Title == resource.Title)
                                         .Select(r => new ResourceDTO{Id = r.Id, Title = r.Title})
                                         .AnyAsync();
            
            if (conflict) return Response.Conflict;

            entity.Title = resource.Title;
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

        public async Task<IEnumerable<ResourceDTO>> Search(string s){
            if (string.IsNullOrWhiteSpace(s)){
                 var resources = (await _context.Resources
                                 .Select(r => new ResourceDTO{Id = r.Id, Title = r.Title})
                                 .ToListAsync());
                return resources;
            }
            var matches = await _context.Resources
                                            .Where(r => r.Title.ToLower().Contains(s.ToLower()))
                                            .Select(r => new ResourceDTO{Id = r.Id, Title = r.Title})
                                            .ToListAsync();
            return matches;
        }
    }
}