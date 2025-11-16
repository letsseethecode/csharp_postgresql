namespace LSTC.Shared.Data;

public interface IRepository<TEntity, TId>
{
    /// <summary>
    /// Load a single entity from the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TEntity> LoadOne(TId id);

    /// <summary>
    /// Load all instances of an object in the database
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> LoadAll();

    /// <summary>
    /// Save an entity to the database.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task Save(TEntity entity);

    /// <summary>
    /// Delete an entity from the database.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task Delete(TEntity entity);

    /// <summary>
    /// Loads an entity from the database, executes an action against it and
    /// then saves it back to the database.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    Task Execute(TId id, Func<TEntity, Task> action);
}