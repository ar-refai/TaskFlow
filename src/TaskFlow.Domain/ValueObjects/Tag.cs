using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.ValueObjects
{
    public record Tag
    {
        public string Value { get; init; }
        public Tag(string value)
        {
            if (string.IsNullOrEmpty(Value)) throw new ArgumentException("Tag can't be empty or whitespace.", nameof(value));
            if (value.Length > 30) throw new ArgumentException("Tag cannot excced 30 characters.", nameof(value));
            Value = value.Trim();
        }
        public static implicit operator string(Tag tag) => tag.Value;
    }
}
