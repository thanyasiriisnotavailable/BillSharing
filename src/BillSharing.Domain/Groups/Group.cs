using BillSharing.Expenses;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Entities.Auditing;

namespace BillSharing.Groups;
public class Group : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }

    public string InviteCode { get; set; }
    public DateTime? InviteCodeExpiry { get; set; }

    public ICollection<GroupMember> Members { get; set; }
    public ICollection<Expense> Expenses { get; set; }

    protected Group() { }

    public Group(string name, string description)
    {
        Name = name;
        Description = description;

        GenerateInviteCode();
    }

    public void GenerateInviteCode(int expiryDays = 7)
    {
        InviteCode = CreateRandomCode(8);
        InviteCodeExpiry = DateTime.UtcNow.AddDays(expiryDays);
    }

    private string CreateRandomCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        var random = new Random();

        return new string(
            Enumerable.Range(0, length)
                .Select(x => chars[random.Next(chars.Length)])
                .ToArray()
        );
    }

    public void AddMember(Guid userId)
    {
        Members ??= new List<GroupMember>();
        if (Members.Any(m => m.UserId == userId)) return;

        Members.Add(new GroupMember { UserId = userId, JoinedAt = DateTime.UtcNow });
    }
}
