using System;
using System.Collections.Generic;
using System.Linq;
using MyFace.Models.Database;
using MyFace.Models.Request;

namespace MyFace.Repositories
{
    public interface IInteractionsRepo
    {
        IEnumerable<Interaction> Search(SearchRequest search);
        int Count(SearchRequest search);
        Interaction GetById(int id);
        public Interaction Create(CreateInteractionRequest create, string username);
        void Delete(int id);
        int CountLikes(int id);
        int CountDislikes(int id);
    }
    
    public class InteractionsRepo : IInteractionsRepo
    {
        private readonly MyFaceDbContext _context;

        public InteractionsRepo(MyFaceDbContext context)
        {
            _context = context;
        }
        
        public IEnumerable<Interaction> Search(SearchRequest search)
        {
            return _context.Interactions
                .Skip((search.Page - 1) * search.PageSize)
                .Take(search.PageSize);
        }

        public int Count(SearchRequest search)
        {
            return _context.Interactions.Count();
        }

        public Interaction GetById(int id)
        {
            return _context.Interactions.Single(i => i.Id == id);
        }

        public Interaction Create(CreateInteractionRequest create, string username)
        {
            var user = _context.Users.Where(u => u.Username == username).Single();
            Console.WriteLine(create.PostId);
            var insertResult = _context.Interactions.Add(new Interaction
            {
        
                Date = DateTime.Now,
                Type = create.InteractionType,
                PostId = create.PostId,
                UserId = user.Id,
            });
            _context.SaveChanges();
            return insertResult.Entity;
        }

        public void Delete(int id)
        {
            var interaction = GetById(id);
            _context.Interactions.Remove(interaction);
            _context.SaveChanges();
        }

        public int CountLikes(int id)
        {
            return _context.Interactions.Where(i => (i.PostId == id && i.Type == InteractionType.LIKE)).Count();
        }

        public int CountDislikes(int id)
        {
            return _context.Interactions.Where(i => (i.PostId == id && i.Type == InteractionType.DISLIKE)).Count();
        }
    }
}