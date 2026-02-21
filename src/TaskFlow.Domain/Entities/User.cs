using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Domain.Entities
{
    public class User : Entity<UserId>
    {
        private Email _email;
        private string _passwordHash = string.Empty;
        private UserRole _role;

        public override UserId Id { get; protected set; }
        public Email Email => _email;
        public string PasswordHash => _passwordHash;
        public UserRole Role => _role;

        public TeamMemberId? TeamMemberId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private User() { }

        public User(Email email, string passwordHash, UserRole role)
        {
            if(string.IsNullOrEmpty(passwordHash))
                throw new ArgumentNullException("Password hash cannot be empty.",nameof(passwordHash));

            Id = UserId.New();
            _email = email;
            _passwordHash = passwordHash;
            _role = role;
            CreatedAt = DateTime.UtcNow;
        }

        public void LinkToTeamMember(TeamMemberId memberId)
        {
            TeamMemberId = memberId;
        }

        public void ChangeRole(UserRole newRole)
        {
            _role = newRole;
        }

    }
}
