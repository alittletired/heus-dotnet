using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heus.Ddd.Infrastructure
{
    public interface IUnitOfWorkOptions
    {
        public IsolationLevel? IsolationLevel { get; set; }
        public bool? IsTransactional { get; set; }

        /// <summary>
        /// Milliseconds
        /// </summary>
        public int? Timeout { get; set; }
    }
}
