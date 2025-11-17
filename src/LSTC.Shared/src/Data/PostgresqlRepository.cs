namespace LSTC.Shared.Data;

public class PostgresqlRepository<TEntity> : IRepository<TEntity, Guid>
{
    public async Task<TEntity?> LoadOne(Guid id)
    {
        await Task.Run(() => { });
        throw new NotImplementedException("Not Implemented");
    }

    public async Task<IEnumerable<TEntity>> LoadAll()
    {
        await Task.Run(() => { });
        throw new NotImplementedException("Not Implemented");
    }

    public async Task Save(TEntity entity)
    {
        await Task.Run(() => { });
        throw new NotImplementedException("Not Implemented");
    }

    public async Task Delete(TEntity entity)
    {
        await Task.Run(() => { });
        throw new NotImplementedException("Not Implemented");
    }

    public async Task Execute(Guid id, Func<TEntity, Task> action)
    {
        var item = await this.LoadOne(id);
        await action(item);
        await this.Save(item);
    }
}