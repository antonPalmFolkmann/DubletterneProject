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
                TextParagraphs = resource.TextParagraphs as ICollection<TextParagraph>,
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
            var res = (await _context.Resources
                                 .Select(r => new ResourceDTO{Id = r.Id, Title = r.Title, User = r.User})
                                 .ToListAsync())
                                 .AsReadOnly();
            return res;
        }
        public Task<IReadOnlyCollection<ResourceDTO>> ReadAllByAuthorAsync(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public async Task<ResourceDetailsDTO> ReadAsync(int resourceID)
        {
            var res = from c in _context.Resources
                            where c.Id == resourceID
                            select new ResourceDetailsDTO
                            {
                                Created = DateTime.Today,
                                Updated = DateTime.Now,
                                TextParagraphs = new List<string>(),
                                ImageUrl = "image2.com"
                            };
            return await res.FirstOrDefaultAsync();
        }

        public async Task<Response> UpdateAsync(int id, ResourceUpdateDTO resource)
        {
            var conflict = await _context.Resources
                                         .Where(r => r.Id != id)
                                         .Where(r => r.Title == resource.Title)
                                         .Where(r => r.User == resource.User)
                                         .Select(r => new ResourceDTO{Id = r.Id, Title = r.Title, User = r.User})
                                         .AnyAsync();
            
            if(conflict)
            {
                return Response.Conflict;
            }

            var entity = await _context.Resources.FirstOrDefaultAsync(c => c.Id == resource.Id);
                                    
            if (entity == null)
            {
                return Response.NotFound;
            }

            entity.Title = resource.Title;
            entity.User = resource.User;
            entity.Created  = resource.Created;
            entity.TextParagraphs = resource.TextParagraphs as ICollection<TextParagraph>;
            entity.Updated = DateTime.Now;
            entity.ImageUrl = resource.ImageUrl;

            await _context.SaveChangesAsync();

            return Response.Updated;
        }
    }
}