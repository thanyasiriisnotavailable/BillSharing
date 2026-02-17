using BillSharing.Expenses;
using System;
using Xunit;

namespace BillSharing.EntityFrameworkCore.Expenses;

[Collection(BillSharingTestConsts.CollectionDefinitionName)]
public class EfCoreExpenseAppService_Tests : ExpenseAppService_Tests<BillSharingEntityFrameworkCoreTestModule>
{
}
