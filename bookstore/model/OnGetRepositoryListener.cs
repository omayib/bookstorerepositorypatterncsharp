using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookstore.model
{
    public interface RepositoryListener
    {
        void onGetBooksSucceed();
        void onDeleteBooksSucceed();
        void onUpdateBooksSucceed();
        void onAddBooksSucceed();
        void onGetRepositoryFailed(string message);
    }
}
