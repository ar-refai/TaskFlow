using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TaskFlow.Domain.ValueObjects
{
    public readonly record struct Email
    {
        private static readonly Regex EmailRegex = 
            new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public string Value { get; }
        public Email(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Email cannot be empty.", nameof(value));
            value = value.Trim().ToLowerInvariant();
            if (!IsValid(value)) throw new ArgumentException("Invalid email format.",nameof(value));
            Value = value;
        }
        private static bool IsValid(string email) => EmailRegex.IsMatch(email);
        public override string ToString() => Value;
        public static implicit operator string(Email email) => email.Value;

    }
}
