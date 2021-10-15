using System;
using System.ComponentModel.DataAnnotations;

namespace MyFace.Models.Database
{
    public enum InteractionType
    {
        LIKE = 0,
        DISLIKE = 1,
    }

    public class Interaction
    {
        public int Id { get; set; }

        public InteractionType Type { get; set; }
       // [Key]
        public int PostId { get; set; }
        public Post Post { get; set; }
        //[Key]
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
    }
}
