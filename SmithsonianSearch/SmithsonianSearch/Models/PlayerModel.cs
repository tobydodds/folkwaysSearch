namespace SmithsonianSearch.Models
{
    using System;

    public class PlayerModel
    {
        public string Id { get; set; }

        public string ContainerId { get; set; }

        public string MediaUrl { get; set; }

        public PlayerModel(){}

        public PlayerModel(string mediaUrl)
        {
            if (!string.IsNullOrEmpty(mediaUrl))
            {
                this.Id = Math.Abs(mediaUrl.GetHashCode()).ToString();
                this.ContainerId = "container" + this.Id;
                this.MediaUrl = mediaUrl;
            }
        }
    }
}