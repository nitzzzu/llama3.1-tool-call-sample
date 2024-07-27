﻿using System.Text.Json.Serialization;

namespace SemanticKernelTest.Models
{
    public class Ticket
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public required string Title { get; set; }

        [JsonPropertyName("description")]
        public required string Description { get; set; }
    }
}
