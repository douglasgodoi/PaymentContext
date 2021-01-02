using System;
using Flunt.Notifications;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Handlers;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHandler : 
        Notifiable, 
        IHandler<CreateBoletoSubscriptionCommand>,
        IHandler<CreatePaypalSubscriptionCommand>
    {
        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;
        public SubscriptionHandler(IStudentRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }
        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {
            //Fail Fast Validations.
            command.Validate();
            if(command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar a assinatura, por favor verifique os dados");
            }
            // Verificar se documento já está cadastrado.
            if(_repository.DocumentExits(command.Document))
            {
                AddNotification("Document", "Este CPF já está em uso");
            }

            //Verificar se e-mail já está cadastrado.
            if(_repository.EmailExits(command.Email))
            {
                AddNotification("Email", "Este E-mail já está em uso");
            }

            // Gerar os VO's
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);
            
            //Gerar as entidades.
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new BoletoPayment(
                command.BarCode, 
                command.BoletoNumber,
                command.PaidDate, 
                command.ExpireDate, 
                command.Total, 
                command.TotalPaid, 
                command.Payer, 
                document, 
                address, 
                email);
        
            //Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);
            
           //Agrupar validações.
           AddNotifications(name, document, email, address, student, subscription, payment);

            //Checar as notificações
            if(Invalid)
                return new CommandResult(false, "Não foi possível realizar a assinatura");

           //Salvar as informações.
           _repository.CreateSubscription(student);

           //Enviar e-mail.
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem vindo ao Balta.io", "Sua assinatura foi criada");

           //Retornas informações. 
           return new CommandResult(true, "Assinatura realizada com sucesso");
        }

        public ICommandResult Handle(CreatePaypalSubscriptionCommand command)
        {
            //Fail Fast Validations.
            command.Validate();
            if(command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar a assinatura, por favor verifique os dados");
            }
            // Verificar se documento já está cadastrado.
            if(_repository.DocumentExits(command.Document))
            {
                AddNotification("Document", "Este CPF já está em uso");
            }

            //Verificar se e-mail já está cadastrado.
            if(_repository.EmailExits(command.Email))
            {
                AddNotification("Email", "Este E-mail já está em uso");
            }

            // Gerar os VO's
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);
            
            //Gerar as entidades.
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new PaypalPayment(
                command.TransactionCode,
                command.PaidDate, 
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                address,
                document,
                command.Payer,
                email);
        
            //Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);
            
            //Agrupar validações.
            AddNotifications(name, document, email, address, student, subscription, payment);
            
            //Checar as notificações
            if(Invalid)
                return new CommandResult(false, "Não foi possível realizar a assinatura");

           //Salvar as informações.
           _repository.CreateSubscription(student);

           //Enviar e-mail.
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem vindo ao Balta.io", "Sua assinatura foi criada");

           //Retornas informações. 
           return new CommandResult(true, "Assinatura realizada com sucesso");
        }
    }
}