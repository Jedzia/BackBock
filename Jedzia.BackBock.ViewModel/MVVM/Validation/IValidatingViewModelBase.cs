using System;

namespace Jedzia.BackBock.ViewModel.MVVM.Validation
{
    public interface IValidatingViewModelBase
    {
        void AddError(string propertyName, string valdationError);
        void RemoveError(string propertyName, string validationError);
        void RemoveErrors(string propertyName);
    }
}
