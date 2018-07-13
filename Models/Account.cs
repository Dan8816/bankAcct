using System;//added this dependcy
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Globalization;//added this dependency
using System.ComponentModel.DataAnnotations;//added this dependency
using System.ComponentModel.DataAnnotations.Schema;//added this dependency

namespace BankAccount.Models
{
    public class Account//this could be called Transaction but we instantiate a new instance for every Transaction
    {
        public int Id { get; set; }//This is the PK but it could be called TransactionID

        public double balance { get; set; }//this will be the sum of transactions
        
        public double transaction { get; set; }//amount of transaction

        public DateTime transactionDate { get; set; }//AKA updated at
 
        public int users_Id { get; set; }//FK one user has many transactions

    }
}