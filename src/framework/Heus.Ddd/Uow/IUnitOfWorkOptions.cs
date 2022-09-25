using System.Data;
namespace Heus.Ddd.Uow
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
