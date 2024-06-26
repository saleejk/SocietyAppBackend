﻿using System.ComponentModel.DataAnnotations;

namespace SocietyAppBackend.ModelEntity
{
    public class Comment
    {
        [Key]
        public int CommentId {  get; set; }
        public int PostId {  get; set; }
        public int UserId {  get; set; }
        public string Text {  get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;
        public Post Post { get; set; }
        public User User { get; set; }


    }
}
