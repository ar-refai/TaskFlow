using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Domain.Entities;
public class Comment : Entity<CommentId>
{
    // backing fields _content
    private string _content = string.Empty;

    // public read-only properties 
    public TeamMemberId AuthorId { get; private set; }
    public TaskId TaskId { get; private set; }
    public override CommentId Id { get; protected set; }
    public string Content => _content;
    public DateTime CreatedAt { get; private set; }

    // ef core constructor
    private Comment() { }

    // internal constructor -  as only tasks can create comments
    internal Comment(string content, TeamMemberId authorId, TaskId taskId)
    {
        if (string.IsNullOrEmpty(content)) throw new ArgumentException("Comment cannot be empty.", nameof(content));
        if (content.Length > 1000) throw new ArgumentException("Comment cannot exceed 1000 character.", nameof(content));
        
        Id = CommentId.New();
        AuthorId = authorId;
        TaskId = taskId;
        _content = content;
    }
    // behavioral methods
    public void EditContent(string newContent)
    {
        if (string.IsNullOrEmpty(newContent)) throw new ArgumentException("Comment cannot be empty.", nameof(newContent));
        if (newContent.Length > 1000) throw new ArgumentException("Comment cannot exceed 1000 character.", nameof(newContent));
        _content = newContent;
    }


}