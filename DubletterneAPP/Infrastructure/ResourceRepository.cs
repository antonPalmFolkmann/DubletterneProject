using Core;
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

        private IEnumerable<TextParagraph> GetParagraphs(ICollection<string> textParagraphs)
        {
            foreach (var paragraph in textParagraphs)
            {
                yield return new TextParagraph(paragraph);
            }
        }

        public Task<Response> DeleteAsync(int resourceID)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<ResourceDTO>> ReadAsync()
        {
            throw new NotImplementedException();
        }
        public Task<IReadOnlyCollection<ResourceDTO>> ReadAllByAuthorAsync(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public Task<ResourceDetailsDTO> ReadAsync(int resourceID)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(ResourceDTO resource)
        {
            throw new NotImplementedException();
        }
    }
}