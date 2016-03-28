using Root.Models;

namespace Root.Data.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
		private CGMContext _dataContext;

		public CGMContext Get()
        {
			return _dataContext ?? (_dataContext = new CGMContext());
        }

        protected override void DisposeCore()
        {
            if (_dataContext != null)
            {
                _dataContext.Dispose();
            }
        }
    }
}
