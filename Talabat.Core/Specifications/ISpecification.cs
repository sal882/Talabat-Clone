using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public interface ISpecification<T> where T : BaseEntity
    {
        // this Rpresent the Where Specification or Criteria
         Expression<Func<T, bool>> Criteria { get; set; }
        //this Repersent Include Specifications
         List<Expression<Func<T, Object>>> Includes { get; set; }
        //Represent order Asyc by operator
        Expression<Func<T, object>> OrderBy { get; set; }
        //Represent order by DESC operator
        Expression<Func<T, object>> OrderByDesc { get; set; }
        //Represent Skip operator
        public int Skip { get; set; }
        //Represent Take operator
        public int Take { get; set; }
        //if Paggination is applied or not
        public bool IsPaginationEnabaled { get; set; }


    }
}
