using System;
using Newtonsoft.Json;

namespace GHApp.Contracts.Dto
{
    [Serializable]
    public class CommitInfo
    {
        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("committer")]
        public Committer Committer { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("tree")]
        public Tree Tree { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("comment_count")]
        public int CommentCount { get; set; }
    }
}