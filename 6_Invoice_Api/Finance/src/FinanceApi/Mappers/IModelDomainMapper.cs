namespace FinanceApi.Mappers
{
    public interface IModelDomainMapper<TModel, TDomain>
    {
        TModel ToModel(TDomain domainInstance);
        TDomain ToDomain(TModel modelInstance, string id = null, string version = null);
    }
}