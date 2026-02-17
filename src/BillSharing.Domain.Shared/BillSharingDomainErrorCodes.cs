namespace BillSharing;

public static class BillSharingDomainErrorCodes
{
    // Group errors
    public const string GroupNameAlreadyExists = "BillSharing:GroupNameAlreadyExists";
    public const string GroupNotFound = "BillSharing:GroupNotFound";
    public const string InvalidGroupInviteCode = "BillSharing:InvalidGroupInviteCode";

    // Expense errors
    public const string ExpenseNotFound = "BillSharing:ExpenseNotFound";
    public const string ExpenseTitleRequired = "BillSharing:ExpenseTitleRequired";
    public const string ExpenseItemRequired = "BillSharing:ExpenseItemRequired";
    public const string ExpenseSplitUserRequired = "BillSharing:ExpenseSplitUserRequired";
    public const string ExpenseAmountInvalid = "BillSharing:ExpenseAmountInvalid";
}
