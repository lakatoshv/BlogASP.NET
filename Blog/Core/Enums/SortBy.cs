using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Blog.Core.Attributes;

namespace Blog.Core.Enums
{
    /*
     * class="list-inline">
                <li>Сортувати</li>
                <li>за: 
                    <select>
                        <option value="CreatedAt">Датою</option>
                        <option value="Title">Назвою</option>
                        <option value="Author">Автором</option>
                        <option value="Likes">Лайками</option>
                        <option value="Dislikes">Дизлайками</option>
                    </select>
                </li>
                <li>тип сортування: 
                    <select>
                        <option value="asc">За зростанням</option>
                        <option value="desc">За спаданням</option>
                    </select>
                </li>
     */
    public enum SortBy
    {
        [StringValue("Saloon / Sedan")] Saloon = 5,
        [StringValue("Coupe")] Coupe = 4,
        [StringValue("Estate / Wagon")] Estate = 6,
        [StringValue("Hatchback")] Hatchback = 8,
        [StringValue("Utility")] Ute = 1,

    }
}