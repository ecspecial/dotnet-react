using Plorg.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plorg.Repo.DTO
{
    /// <summary>
    /// Task element DTO interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IElementDTO<T>
    {
        public T FromDTO();

        public void ToDTO(T element);
    }
}
