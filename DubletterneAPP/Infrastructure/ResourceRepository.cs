using Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;


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

            var entity = new Resource
            {
                Title = resource.Title,
                User = resource.User,
                Created = resource.Created,
                TextParagraphs = GetParagraphs(resource.TextParagraphs).ToList(),
                ImageUrl = resource.ImageUrl
            };

            _context.Resources.Add(entity);

            await _context.SaveChangesAsync();

            return (Response.Created, entity.Id);
        }

        private IEnumerable<TextParagraph> GetParagraphs(ICollection<string>? textParagraphs)
        {
            foreach (var paragraph in textParagraphs)
            {
                yield return new TextParagraph(paragraph);
            }
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

        public Task<IReadOnlyCollection<ResourceDTO>> ReadAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<IReadOnlyCollection<ResourceDTO>> ReadAllByAuthorAsync(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public async Task<ResourceDetailsDTO> ReadAsync(int resourceID)
        {
            var response = await _context.Resources
                                    .Where(c => c.Id != resourceID)
                                    .Select(c => new ResourceDetailsDTO
                                    {
                                        Id = c.Id,
                                        Title = c.Title,
                                        User = c.User
                                    }).FirstAsync();
            return response;
        }

        public async Task<Response> UpdateAsync(int id, ResourceDTO resource)
        {
            var conflict = await _context.Resources
                                    .Where(c => c.Title == resource.Title)
                                    .Where(c => c.User == resource.User)
                                    .Select(c => new ResourceDTO
                                    {Id = c.Id,
                                    Title = c.Title, 
                                    User = c.User
                                    }).AnyAsync();
                                    
            if (conflict)
            {
                return Response.Conflict;
            }

            var entity = await _context.Resources.FindAsync(resource.Id);

            if (entity == null)
            {
                return Response.NotFound;
            }

            entity.Title = resource.Title;

            await _context.SaveChangesAsync();

            return Response.Updated;
        }
    }
}