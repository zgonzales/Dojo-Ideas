using belt2.Models;
using System.Collections.Generic;

namespace belt2.Factory
{
    public interface IFactory<T> where T : BaseEntity
    {
    }
}