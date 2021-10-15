using System;
using System.Text.Json.Serialization;
using MyFace.Models.Database;

namespace MyFace.Models.Response
{
    public class InteractionResponse
    {
        private readonly Interaction _interaction;
    

        public InteractionResponse(Interaction interaction)
        {
            _interaction = interaction;
            
        }

        public InteractionResponse(Interaction interaction, int likes, int dislikes)
        {
            _interaction = interaction;
            Likes = likes;
            Dislikes = dislikes;
        }

        public int Id => _interaction.Id;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public InteractionType Type => _interaction.Type;
        public DateTime Date => _interaction.Date;
        public int PostId => _interaction.PostId;
        public int UserId => _interaction.UserId;

        public int Likes { get; set;}
        public int Dislikes { get; set; }
    }
}