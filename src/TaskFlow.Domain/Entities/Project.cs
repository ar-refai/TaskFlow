using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Domain.Entities
{
    public class Project : Entity<ProjectId>
    {
        // Backing fields
        private string _name = string.Empty;
        private string? _description;

        // Public read_only properties
        public override ProjectId Id { get; protected set; }
        public string Name => _name;
        public string? Description => _description;

        // Audit Properties
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        // EF Constructor
        private Project() { }

        public Project(string name, string? description = null)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Project name cannot be empty.", nameof(name));
            if (name.Length > 200) throw new ArgumentException("Project name cannot exceed 200 characters.", nameof(name));
            Id = ProjectId.New();
            _name = name;
            _description = description;
        }

        // Behavioral methods
        public void ChangeName(string newName)
        {
            if (string.IsNullOrEmpty(newName)) throw new ArgumentException("Project name cannot be empty.", nameof(newName));
            if (newName.Length > 200) throw new ArgumentException("Project name cannot exceed 200 characters.", nameof(newName));
            _name = newName;
        }

        public void ChangeDescription(string? newDescription)
        {
            _description = newDescription;
        }

    }
}
