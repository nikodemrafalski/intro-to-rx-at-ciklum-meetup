using System;
using Newtonsoft.Json;

namespace GHApp.Contracts.Dto
{
    [Serializable]
    public class Parent
    {
        [JsonProperty("sha")]
        public string Sha { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }
    }
}