using PaymentContext.Domain.Entities;

namespace PaymentContext.Domain.Repositories 
{
    public interface IStudentRepository
    {
        bool DocumentExits(string doc);
        bool EmailExits(string email);
        void CreateSubscription(Student student);
        
    }
}