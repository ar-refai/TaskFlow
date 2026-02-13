using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Domain.Entities
{
    public class TeamMember : Entity<TeamMemberId>
    {
        // Backing fields
        private string _name = string.Empty;
        private Email _email;
        
        // Public read-only properties 
        public override TeamMemberId Id { get; protected set; }
        public string Name => _name;
        public Email Email => _email;

        // audit field
        public DateTime CreatedAt { get; private set; }

        // EF Core Constructor
        private TeamMember() { }

        // Public Constructor
        public TeamMember(string name, Email email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Team member name cannot be empty.", nameof(name));

            if (name.Length > 100)
                throw new ArgumentException("Team member name cannot exceed 100 characters.", nameof(name));

            Id = TeamMemberId.New();
            _name = name;
            _email = email;
        }

        // Behavioral methods
        public void ChangeName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Team member name cannot be empty.", nameof(newName));

            if (newName.Length > 100)
                throw new ArgumentException("Team member name cannot exceed 100 characters.", nameof(newName));

            _name = newName;
        }

        public void ChangeEmail(Email newEmail)
        {
            _email = newEmail;
        }
    }
}
