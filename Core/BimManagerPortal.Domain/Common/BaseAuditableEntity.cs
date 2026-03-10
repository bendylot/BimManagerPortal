namespace BimManagerPortal.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    protected BaseAuditableEntity() { }
    protected BaseAuditableEntity(string UserCreater) : base()
    {
        this.UserCreater = UserCreater;
    }
    public string UserCreater { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UserUpdater { get; set; }
}