using Blog.Core.Attributes;

namespace Blog.Core.Enums
{
    /*
    Сортувати
    за: 
    <option value="CreatedAt">Датою</option>
    <option value="Title">Назвою</option>
    <option value="Author">Автором</option>
    <option value="Likes">Лайками</option>
    <option value="Dislikes">Дизлайками</option>
    тип сортування: 
    <option value="asc">За зростанням</option>
    <option value="desc">За спаданням</option>
     */

    /// <summary>
    /// Sort by.
    /// </summary>
    public enum SortBy
    {
        [StringValue("Saloon / Sedan")] Saloon = 5,
        [StringValue("Coupe")] Coupe = 4,
        [StringValue("Estate / Wagon")] Estate = 6,
        [StringValue("Hatchback")] Hatchback = 8,
        [StringValue("Utility")] Ute = 1,

    }
}