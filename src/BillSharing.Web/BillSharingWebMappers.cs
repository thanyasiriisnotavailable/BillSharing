using BillSharing.Expenses;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace BillSharing.Web;

[Mapper]
public partial class CreateExpenseViewModelToDtoMapper
    : MapperBase<
        Pages.Expenses.CreateModalModel.CreateExpenseViewModel,
        CreateExpenseDto>
{
    public override partial CreateExpenseDto Map(
        Pages.Expenses.CreateModalModel.CreateExpenseViewModel source);

    public override partial void Map(
        Pages.Expenses.CreateModalModel.CreateExpenseViewModel source,
        CreateExpenseDto destination);
}
